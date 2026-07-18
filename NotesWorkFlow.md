# Web API Development Workflow

> A complete development guide

---

# Overall Development Workflow

```text
Requirements
      │
      ▼
Database Design
      │
      ▼
Create Entity Models
      │
      ▼
Create DbContext
      │
      ▼
Configure OnModelCreating
      │
      ▼
Configure SQL Server
      │
      ▼
Register Services (Dependency Injection)
      │
      ▼
Create Migration
      │
      ▼
Update Database
      │
      ▼
Verify Database
      │
      ▼
Create Repository
      │
      ▼
Create Service
      │
      ▼
Create DTOs
      │
      ▼
Add Validation
      │
      ▼
Create Controllers
      │
      ▼
Authentication
      │
      ▼
Authorization
      │
      ▼
Middleware
      │
      ▼
Testing
      │
      ▼
Deployment
```

---

# Runtime Request Flow

```text
Client
   │
HTTP Request
   │
   ▼
Middleware
   │
   ▼
Authentication
   │
   ▼
Authorization
   │
   ▼
Controller
   │
   ▼
Service
   │
   ▼
Repository
   │
   ▼
EF Core DbContext
   │
   ▼
SQL Server
   │
   ▲
Repository
   │
   ▲
Service
   │
   ▲
Controller
   │
HTTP Response
   │
Client
```

---

# Project Structure

```text
IAM.API
│
├── Authentication
│   ├── JwtService.cs
│   └── PasswordHasher.cs
│
├── Authorization
│   ├── PermissionRequirement.cs
│   └── PermissionHandler.cs
│
├── Controllers
│   ├── AuthController.cs
│   ├── UsersController.cs
│   ├── RolesController.cs
│   └── PermissionsController.cs
│
├── Data
│   └── IAMContext.cs
│
├── DTOs
│   ├── Auth
│   ├── Users
│   ├── Roles
│   └── Permissions
│
├── Entities
│   ├── User.cs
│   ├── Role.cs
│   ├── Permission.cs
│   ├── UserRole.cs
│   ├── RolePermission.cs
│   ├── RefreshToken.cs
│   └── AuditLog.cs
│
├── Middleware
│   └── ExceptionMiddleware.cs
│
├── Repositories
│   ├── UserRepository.cs
│   ├── RoleRepository.cs
│   └── PermissionRepository.cs
│
├── Services
│   ├── AuthService.cs
│   ├── UserService.cs
│   ├── RoleService.cs
│   └── PermissionService.cs
│
├── Migrations
│
├── appsettings.json
├── Program.cs
└── IAM.API.csproj
```

---

# Phase 1 — Requirements

Before writing code, understand the feature.

Questions:

- What problem am I solving?
- What data is needed?
- Who can access it?
- What API endpoints are required?

Example:

```
Feature

Create User

↓

Requires

Users table

↓

POST /api/users
```

Checklist

- [ ] Feature understood
- [ ] API endpoints identified
- [ ] Business rules identified

---

# Phase 2 — Database Design

Design the database before coding.

Tables

```
Users
Roles
Permissions
UserRoles
RolePermissions
RefreshTokens
AuditLogs
```

Relationship

```text
User
 |
 └── UserRole
          |
          └── Role
                  |
                  └── RolePermission
                            |
                            └── Permission
```

Checklist

- [ ] Tables
- [ ] Columns
- [ ] Primary Keys
- [ ] Foreign Keys
- [ ] Relationships
- [ ] Required Fields
- [ ] Default Values

---

# Phase 3 — Create Entity Models

Create one entity per table.

Example

```csharp
public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = "";

    public string Email { get; set; } = "";

    public string PasswordHash { get; set; } = "";

    public ICollection<UserRole> UserRoles { get; set; } = [];
}
```

Checklist

- [ ] One Entity per table
- [ ] Properties match columns
- [ ] Navigation properties added

---

# Phase 4 — Create DbContext

Location

```
Data/IAMContext.cs
```

Responsibilities

- Register DbSets
- Configure EF Core
- Connect to SQL Server

Example

```csharp
public class IAMContext : DbContext
{
    public IAMContext(DbContextOptions<IAMContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
}
```

Checklist

- [ ] DbContext created
- [ ] DbSets registered
- [ ] Project builds

---

# Phase 5 — Configure OnModelCreating

Purpose

Configure database rules that EF Core cannot infer automatically.

Common Uses

- Composite Keys
- Foreign Keys
- One-to-Many
- Many-to-Many
- Cascade Delete
- Default Values
- Unique Indexes
- Seed Data

Example

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<UserRole>()
        .HasKey(x => new
        {
            x.UserId,
            x.RoleId
        });

    modelBuilder.Entity<RolePermission>()
        .HasKey(x => new
        {
            x.RoleId,
            x.PermissionId
        });
}
```

Checklist

- [ ] Composite Keys
- [ ] Foreign Keys
- [ ] Relationships
- [ ] Delete Behavior
- [ ] Seed Data

---

# Phase 6 — Configure SQL Server

appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=IAMDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Program.cs

```csharp
builder.Services.AddDbContext<IAMContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});
```

Checklist

- [ ] SQL Server installed
- [ ] Connection String added
- [ ] DbContext registered

---

# Phase 7 — Register Dependency Injection

Register repositories and services.

Example

```csharp
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthService, AuthService>();
```

Checklist

- [ ] Repository registered
- [ ] Service registered

---

# Phase 8 — Create Migration

Build first.

```bash
dotnet build
```

Create migration.

```bash
dotnet ef migrations add InitialCreate
```

Generated

```
Migrations

InitialCreate.cs

IAMContextModelSnapshot.cs
```

Checklist

- [ ] Build succeeds
- [ ] Migration created
- [ ] Migration looks correct

---

# Phase 9 — Update Database

Apply migration.

```bash
dotnet ef database update
```

Verify

```
IAMDb

Users

Roles

Permissions

UserRoles

RolePermissions

RefreshTokens

AuditLogs
```

Checklist

- [ ] Database exists
- [ ] Tables created
- [ ] Foreign Keys created

---

# Phase 10 — Repository Layer

Purpose

Database access only.

Flow

```text
Repository

↓

DbContext

↓

Database
```

Responsibilities

- CRUD
- LINQ Queries

No business logic.

Checklist

- [ ] CRUD implemented
- [ ] Database logic only

---

# Phase 11 — Service Layer

Purpose

Business logic.

Flow

```text
Register User

↓

Validate Request

↓

Check Email

↓

Hash Password

↓

Assign Role

↓

Save User
```

Responsibilities

- Validation
- Business Rules
- Transactions

Checklist

- [ ] Business logic complete

---

# Phase 12 — DTOs

Purpose

Separate API models from database models.

Examples

```
CreateUserDto

UpdateUserDto

LoginRequestDto

RoleDto
```

Checklist

- [ ] Request DTOs
- [ ] Response DTOs
- [ ] Entities never exposed

---

# Phase 13 — Validation

Validate incoming requests.

Options

- DataAnnotations
- FluentValidation

Example

```text
CreateUserDto

↓

Validate

↓

Service
```

Checklist

- [ ] DTO validation
- [ ] Validation messages

---

# Phase 14 — Controllers

Responsibilities

- Receive HTTP Request
- Call Service
- Return Response

No business logic.

Flow

```text
HTTP Request

↓

Controller

↓

Service
```

Checklist

- [ ] Routes
- [ ] Swagger
- [ ] Correct Status Codes

---

# Phase 15 — Authentication

Components

```
JWT

Refresh Tokens

Password Hasher
```

Flow

```text
Login

↓

Find User

↓

Verify Password

↓

Generate JWT

↓

Generate Refresh Token

↓

Return Tokens
```

Checklist

- [ ] Register
- [ ] Login
- [ ] JWT
- [ ] Refresh Token

---

# Phase 16 — Authorization

Implement

- Roles
- Permissions
- Policies

Example

```
Admin

Users.Create

Users.Read

Users.Update

Users.Delete
```

Checklist

- [ ] Roles
- [ ] Permissions
- [ ] Protected Endpoints

---

# Phase 17 — Middleware

Responsibilities

- Global Exception Handling
- Logging
- Standard Error Responses

Flow

```text
Request

↓

Middleware

↓

Controller
```

Checklist

- [ ] Exception Handling
- [ ] Logging

---

# Phase 18 — Testing

Tools

- Swagger
- Postman
- xUnit
- Moq

Test

- Authentication
- Authorization
- CRUD
- Validation

Checklist

- [ ] Unit Tests
- [ ] API Tests

---

# Phase 19 — Deployment

Checklist

- [ ] Production Connection String
- [ ] Environment Variables
- [ ] Run Migration
- [ ] Logging Enabled
- [ ] Publish API

---

# Feature Development Workflow

Every new feature should follow this order.

```text
Requirement
      │
      ▼
Database Design
      │
      ▼
Entity
      │
      ▼
DbContext
      │
      ▼
OnModelCreating
      │
      ▼
Migration
      │
      ▼
Database Update
      │
      ▼
Repository
      │
      ▼
Service
      │
      ▼
DTO
      │
      ▼
Validation
      │
      ▼
Controller
      │
      ▼
Swagger/Postman Test
      │
      ▼
Unit Test
```

---

# Daily EF Core Workflow

When changing the database model:

```text
Modify Entity
      │
      ▼
Modify OnModelCreating (if needed)
      │
      ▼
dotnet build
      │
      ▼
dotnet ef migrations add MigrationName
      │
      ▼
dotnet ef database update
      │
      ▼
Verify SQL Server
```

---

# Mental Model

Whenever implementing a feature, think in this order.

```text
Requirement
      │
      ▼
Database
      │
      ▼
Entity
      │
      ▼
DbContext
      │
      ▼
Migration
      │
      ▼
Database
      │
      ▼
Repository
      │
      ▼
Service
      │
      ▼
DTO
      │
      ▼
Validation
      │
      ▼
Controller
      │
      ▼
HTTP Response
```

---

# Tech Stack

Backend

- .NET 8
- ASP.NET Core Web API
- C# 12
- Entity Framework Core 8
- SQL Server Express

Authentication

- JWT
- Refresh Tokens
- Password Hashing

Authorization

- Role-Based Access Control (RBAC)
- Permission-Based Authorization

Validation

- FluentValidation / DataAnnotations

Logging

- Serilog

Testing

- xUnit
- Moq

Documentation

- Swagger / OpenAPI
