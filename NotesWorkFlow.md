### Development Workflow

## Architecture Flow

```
Client Application
        |
        ↓
Controller
        |
        ↓
Service
        |
        ↓
Repository
        |
        ↓
EF Core DbContext
        |
        ↓
SQL Server Database
```

---

## Development Order

```
1. Database Design
2. Create Entities (Models)
3. Create DbContext
4. Configure Relationships
5. Configure SQL Server
6. Create Migration
7. Update Database
8. Verify SQL Tables
9. Create Repositories
10. Create Services
11. Create DTOs
12. Create Controllers
13. Add Authentication
14. Add Authorization
15. Add Middleware
16. Testing
17. Deployment
```

---

## Phase 1 — Database Design

Before coding, design the database.

# Tables

```
Users

Roles

Permissions

UserRoles

RolePermissions

RefreshTokens

AuditLogs
```

---

# Database Checklist

- [ ] Define tables
- [ ] Define columns
- [ ] Define primary keys
- [ ] Define foreign keys
- [ ] Define relationships
- [ ] Define required fields
- [ ] Define default values

---

## Phase 2 — Create Entities (Models)

Location:

```
Entities
│
├── User.cs
├── Role.cs
├── Permission.cs
├── UserRole.cs
├── RolePermission.cs
├── RefreshToken.cs
└── AuditLog.cs
```

Entities represent database tables.

Example:

```csharp
public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }


    public ICollection<UserRole> UserRoles { get; set; }
}
```

Checklist:

- [ ] Every table has an Entity
- [ ] Properties match database columns
- [ ] Navigation properties added
- [ ] Entity relationships planned

---

## Phase 3 — Create DbContext

Location:

```
Data
└── IAMContext.cs
```

Purpose:

- Connect Entity Framework Core to SQL Server
- Register entities
- Configure database behavior

Example:

```csharp
public class IAMContext : DbContext
{
    public IAMContext(
        DbContextOptions<IAMContext> options)
        : base(options)
    {
    }


    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Permission> Permissions { get; set; }
}
```

Checklist:

- [ ] DbContext created
- [ ] DbSets added
- [ ] Project builds successfully

---

## Phase 4 — Configure Relationships

Location:

```
IAMContext.cs
```

Configure:

- Primary Keys
- Foreign Keys
- One-to-many relationships
- Many-to-many relationships

Example:

```csharp
protected override void OnModelCreating(
    ModelBuilder builder)
{

    builder.Entity<UserRole>()
        .HasKey(x => new
        {
            x.UserId,
            x.RoleId
        });

}
```

Checklist:

- [ ] User → UserRole configured
- [ ] Role → UserRole configured
- [ ] Role → Permission configured
- [ ] Delete behavior configured

---

## Phase 5 — Configure SQL Server

# appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=IAMDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

---

# Program.cs

```csharp
builder.Services.AddDbContext<IAMContext>(
options =>
options.UseSqlServer(
builder.Configuration
.GetConnectionString("DefaultConnection")
));
```

Checklist:

- [ ] SQL Server installed
- [ ] Connection string added
- [ ] DbContext registered
- [ ] Application starts

---

## Phase 6 — Create Migration

Migration converts C# models into database instructions.

Command:

```bash
dotnet ef migrations add InitialCreate
```

Generated:

```
Migrations

├── 20260711_InitialCreate.cs
└── IAMContextModelSnapshot.cs
```

Checklist:

- [ ] Migration created
- [ ] No EF Core errors
- [ ] Migration contains expected tables

---

## Phase 7 — Create SQL Server Database

Apply migration:

```bash
dotnet ef database update
```

Result:

```
SQL Server

IAMDB

Tables:

Users

Roles

Permissions

UserRoles

RolePermissions

RefreshTokens

AuditLogs
```

Checklist:

- [ ] Database created
- [ ] Tables exist
- [ ] Foreign keys exist
- [ ] Relationships are correct

---

## Phase 8 — Verify Database

Check using:

```
SQL Server Management Studio
```

Verify:

```
Users
Roles
Permissions
UserRoles
RolePermissions
RefreshTokens
AuditLogs
```

Checklist:

- [ ] Database structure correct
- [ ] No missing tables
- [ ] No wrong relationships

---

## Phase 9 — Create Repositories

Location:

```
Repositories

├── UserRepository.cs
├── RoleRepository.cs
└── PermissionRepository.cs
```

Responsibility:

```
Repository

↓

DbContext

↓

Database
```

Handles:

- Query data
- Insert data
- Update data
- Delete data

Checklist:

- [ ] CRUD completed
- [ ] Only database logic exists here

---

## Phase 10 — Create Services

Location:

```
Services

├── AuthService.cs
├── UserService.cs
├── RoleService.cs
└── PermissionService.cs
```

Responsibility:

- Business logic
- Validation
- Application rules

Example:

```
Create User

↓

Validate Email

↓

Hash Password

↓

Save User

↓

Assign Role
```

Checklist:

- [ ] Business rules implemented
- [ ] Validation added

---

## Phase 11 — Create DTOs

Purpose:

- Protect database entities
- Control API requests
- Control API responses

Structure:

```
DTOs

├── Auth
│   ├── LoginRequestDto.cs
│   └── RegisterRequestDto.cs
│
├── Users
│   ├── CreateUserDto.cs
│   └── UpdateUserDto.cs
│
├── Roles
│   ├── CreateRoleDto.cs
│   └── RoleDto.cs
│
└── Permissions
    └── PermissionDto.cs
```

Checklist:

- [ ] Request DTOs created
- [ ] Response DTOs created
- [ ] Entities are not exposed directly

---

## Phase 12 — Create Controllers

Location:

```
Controllers

├── AuthController.cs
├── UsersController.cs
├── RolesController.cs
└── PermissionsController.cs
```

Flow:

```
HTTP Request

↓

Controller

↓

Service

↓

Repository

↓

Database
```

Checklist:

- [ ] Routes created
- [ ] Swagger tested
- [ ] HTTP responses correct

---

## Phase 13 — Authentication

Location:

```
Authentication

├── JwtService.cs
└── PasswordHasher.cs
```

Flow:

```
Login Request

↓

Find User

↓

Check Password

↓

Generate JWT

↓

Return Token
```

Checklist:

- [ ] Register works
- [ ] Login works
- [ ] JWT generated
- [ ] Refresh token works

---

## Phase 14 — Authorization

Location:

```
Authorization

├── PermissionRequirement.cs
└── PermissionHandler.cs
```

Implement:

- Role-Based Access Control
- Permission-Based Authorization

Example:

```
Admin

Users.Create
Users.Read
Users.Update
Users.Delete
```

Checklist:

- [ ] Roles work
- [ ] Permissions work
- [ ] Protected endpoints tested

---

## Phase 15 — Middleware

Location:

```
Middleware

└── ExceptionMiddleware.cs
```

Handles:

- Global errors
- Logging
- Standard responses

Checklist:

- [ ] Exceptions handled
- [ ] Logs created

---

## Phase 16 — Testing

Tools:

```
xUnit

Moq

Postman

Swagger
```

Test:

```
Authentication

Users

Roles

Permissions

Authorization
```

Checklist:

- [ ] Unit tests created
- [ ] API endpoints tested

---

## Phase 17 — Deployment

Checklist:

- [ ] Production connection string
- [ ] Database migration
- [ ] Environment variables
- [ ] Logging configured
- [ ] API deployed

---

## Final Mental Model

When building features:

```
Requirement
      |
      ↓
Database Table
      |
      ↓
Entity Model
      |
      ↓
DbContext
      |
      ↓
Migration
      |
      ↓
SQL Database
      |
      ↓
Repository
      |
      ↓
Service
      |
      ↓
DTO
      |
      ↓
Controller
      |
      ↓
API Response
```

---
