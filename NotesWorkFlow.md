# Feature Add Workflow

When adding a new API feature, follow this order:

1. Entities
2. DbContext
3. Relationships / EF Configuration _(In OnModelCreating)_
4. Migrations
5. Seeder _(if default/initial data is needed)_ _(In Program.cs)_
6. Repository
7. Service
8. Dependency Injection _(In Program.cs)_
9. Controller
10. Build & Test with Swagger/Postman

---

## Simple Flow

### 1. Entities

Create the data model and add the new entity class in the `Entities` folder.

> Optional: Create Request/Response DTOs if the API should not expose the entity directly.

---

### 2. Data / DbContext

Update the `DbContext` and add the new `DbSet` if needed.

```csharp
public DbSet<Role> Roles { get; set; }
```

---

### 3. Data / EF Relationships

Configure how the new entity connects to other entities.

Configure:

- Relationships
- Primary keys
- Foreign keys
- Composite keys
- Constraints
- Indexes

This can be done in `OnModelCreating` or separate EF configuration classes.

---

### 4. Migrations

Create and apply the migration:

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

Migrations are used to **create or change the database structure**.

For example:

```text
Entity
  ↓
Migration
  ↓
Database table/columns/relationships
```

> You need a migration when the **database schema changes**.

---

### 5. Seeder

If the feature requires default or initial data, create or update a seeder.

For example:

```text
RoleSeeder
    ↓
Admin
Employee
```

The seeder inserts **default data** into existing database tables.

> A seeder does not replace migrations. Migrations create/change the database structure, while seeders insert default data.

### Migration vs Seeder

| Migration                  | Seeder                      |
| -------------------------- | --------------------------- |
| Changes database structure | Inserts default data        |
| Creates tables             | Inserts rows                |
| Adds columns               | Inserts initial records     |
| Adds relationships         | Creates default roles       |
| Changes constraints        | Creates default permissions |

Example:

```text
Migration
    ↓
Creates Roles table
    ↓
Seeder
    ↓
Inserts Admin and Employee
```

---

### 6. Repository

Add the repository layer for database access.

For example:

```text
IRoleRepository
RoleRepository
```

The repository handles database operations such as:

- Get
- Get by ID
- Add
- Update
- Delete

---

### 7. Service

Add the service layer for business logic.

For example:

```text
IRoleService
RoleService
```

The service handles business rules and coordinates operations between the controller and repository.

---

### 8. Dependency Injection

Register the repository and service in `Program.cs`.

```csharp
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
```

---

### 9. Controller

Create or update the controller endpoint for the feature.

For example:

```text
RolesController
```

> Optional: Accept Request DTOs and return Response DTOs.

Example endpoints:

```text
GET    /api/roles
GET    /api/roles/{id}
POST   /api/roles
PUT    /api/roles/{id}
DELETE /api/roles/{id}
```

---

### 10. Build & Test with Swagger/Postman

Build the application:

```bash
dotnet build
```

Run the application:

```bash
dotnet run
```

Then verify the endpoint using Swagger or Postman.

Test:

- Successful requests
- Validation errors
- Not found responses
- Duplicate data
- Unauthorized/forbidden requests when applicable
- Database changes

---

## Quick Reminder

### Database Structure Flow

**Entities → DbContext → Relationships → Migration → Database**

### Default Data Flow

**Seeder → Database**

### API Request Flow

**HTTP Request → Controller → Service → Repository → DbContext → Database**

### Complete Feature Flow

**Entities → DbContext → Relationships → Migration → Seeder (if needed) → Repository → Service → Dependency Injection → Controller → Build → Run → Test**
