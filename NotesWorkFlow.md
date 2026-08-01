# Feature Add Workflow

Adding API, follow this order:

1. Entities
2. DbContext
3. Relationships
4. Migrations
5. Repository
6. Service
7. Dependency Injection
8. Controller
9. Test with Swagger/Postman

## Simple Flow

### 1. Entities

Create the data model and add the new entity class in the `Entities` folder.

### 2. Data/DbContext

Update the `DbContext` and add the new `DbSet` if needed.

### 3. Data/OnModel (EF Relationships)

Configure how the new entity connects to other entities.

### 4. Migrations

Create and apply the migration:

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

### 5. Repository

Add the repository layer for database access.

### 6. Service

Add the business logic in the service layer.

### 7. Dependency Injection

Register the repository and service in `Program.cs`.

```csharp
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
```

### 8. Controller

Create or update the controller endpoint for the feature.

### 9. Test with Swagger/Postman

Build, run, and test the API.

```bash
dotnet build
dotnet run
```

Then verify the endpoint using Swagger or Postman.

## Quick Reminder

Feature flow:

**Entities → DbContext → Relationships → Migrations → Repository → Service → Dependency Injection → Controller → Build → Run → Test**
