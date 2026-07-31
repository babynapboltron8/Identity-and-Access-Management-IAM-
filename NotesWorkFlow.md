# ASP.NET Core Web API Development Guide

> Production workflow guide for building ASP.NET Core Web APIs.

---

# Section 1 — Initial Configuration (One-Time Setup)

Configuration that is normally completed once when creating a new ASP.NET Core Web API project.

---

# 1. Project Initialization

Task                       Command / Purpose


Create Project             Create ASP.NET Core Web API project

```bash
dotnet new webapi -n IAM.API
```


Enter Project Folder       Move into project directory

```bash
cd IAM.API
```


Run Application            Verify API works

```bash
dotnet run
```


Verify:

- API starts successfully
- Swagger opens
- Project builds without errors


Checklist:

☐ Project created

☐ API runs

☐ Swagger works

---

# 2. Install Required Packages

Package                                  Purpose


Microsoft.EntityFrameworkCore.SqlServer  SQL Server database provider


Microsoft.EntityFrameworkCore.Design     EF Core migration commands


Swashbuckle.AspNetCore                   Swagger/OpenAPI documentation


Install:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet add package Swashbuckle.AspNetCore
```

---

# 3. Recommended Project Structure


Folder                     Responsibility


Controllers                Handles HTTP requests and responses


Data                       Contains EF Core DbContext


Entities                   Represents database tables


DTOs                       Defines API request and response objects


Repositories               Handles database operations


Services                   Contains business logic


Authentication             JWT and password handling


Authorization              Roles and permissions


Middleware                 Global request processing


Migrations                 Stores database migration history



Structure:

```text
IAM.API

├── Authentication
│   ├── JwtService.cs
│   └── PasswordHasher.cs
│
├── Authorization
│   ├── PermissionRequirement.cs
│   └── PermissionHandler.cs
│
├── Controllers
│
├── Data
│   └── IAMContext.cs
│
├── DTOs
│
├── Entities
│
├── Middleware
│
├── Repositories
│
├── Services
│
├── Migrations
│
├── appsettings.json
├── Program.cs
└── IAM.API.csproj
```

---

# 4. Configure SQL Server Connection


File                       Responsibility


appsettings.json           Stores database connection details


Program.cs                 Registers database services



Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=IAMDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```


Connection Flow:

```text
ASP.NET Core

↓

Connection String

↓

Entity Framework Core

↓

SQL Server
```

---

# 5. Create DbContext


Location:

```text
Data/IAMContext.cs
```


Purpose:

Responsibility              Description


DbSet                       Represents database tables


Configuration               Defines EF Core behavior


Connection                  Communicates with SQL Server



Example:

```csharp
using Microsoft.EntityFrameworkCore;

public class IAMContext : DbContext
{
    public IAMContext(
        DbContextOptions<IAMContext> options)
        : base(options)
    {
    }


    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Permission> Permissions => Set<Permission>();
}
```

---

# 6. Register DbContext


File:

```text
Program.cs
```


Example:

```csharp
builder.Services.AddDbContext<IAMContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration
        .GetConnectionString("DefaultConnection"));
});
```


Flow:

```text
Controller

↓

Service

↓

Repository

↓

DbContext

↓

SQL Server
```

---

# 7. Configure Entity Relationships


Method:

```text
OnModelCreating()
```


Purpose:

Configure database rules EF Core cannot automatically determine.


Used For:

Feature                    Example


Composite Keys             UserRole(UserId, RoleId)


Foreign Keys               User → Role


Relationships              One-to-Many / Many-to-Many


Indexes                    Unique Email


Default Values             CreatedDate


Seed Data                  Default Roles



Example:

```csharp
protected override void OnModelCreating(
    ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);


    modelBuilder.Entity<UserRole>()
        .HasKey(x => new
        {
            x.UserId,
            x.RoleId
        });
}
```

---

# 8. Configure Dependency Injection


Purpose:

Allow classes to receive dependencies automatically.


Registration:

```csharp
builder.Services
    .AddScoped<IUserRepository, UserRepository>();

builder.Services
    .AddScoped<IUserService, UserService>();
```


Lifetime:

Type              Usage


Scoped            One instance per HTTP request


Transient         New instance every request


Singleton         One instance for application lifetime



Common API usage:

```text
Controller

↓

Service

↓

Repository

↓

DbContext
```

---

# 9. Create Initial Database


Before migration:

```bash
dotnet build
```


Create migration:

```bash
dotnet ef migrations add InitialCreate
```


Apply migration:

```bash
dotnet ef database update
```


Result:

```text
IAMDb

├── Users
├── Roles
├── Permissions
├── UserRoles
├── RolePermissions
├── RefreshTokens
└── AuditLogs
```


Checklist:

☐ Build successful

☐ Migration created

☐ Database created

☐ Tables verified

---

# End of Section 1


# Section 2 — Feature Development Workflow

> This section is repeated for every new feature you build.

Example:

```text
Feature:

Create User

↓

Endpoint:

POST /api/users
```

---

# Feature Development Order


Step                       Action


1                          Understand Requirement


2                          Design Database


3                          Create Entity Models


4                          Update DbContext


5                          Configure Relationships


6                          Create Migration


7                          Update Database


8                          Create Repository


9                          Create Service


10                         Create DTOs


11                         Add Validation


12                         Create Controller


13                         Test API


14                         Add Authentication/Authorization if needed



---

# Step 1 — Understand Requirement


Before writing code, understand what you are building.


Questions:

Question                    Purpose


What problem is solved?     Understand the feature goal


What data is needed?        Identify database requirements


Who can access it?          Define permissions


What API is needed?         Define endpoints



Example:

```text
Requirement:

Admin can create users


Database:

Users table


API:

POST /api/users
```


Checklist:

☐ Feature understood

☐ Endpoint identified

☐ Business rules identified

---

# Step 2 — Database Design


Design database changes before coding.


Consider:


Item                       Example


Tables                     Users


Columns                    Email, PasswordHash


Primary Key                UserId


Foreign Key                RoleId


Constraints                Unique Email


Relationships              User → Role



Example:

```text
Users

Id
Username
Email
PasswordHash
CreatedDate
```

---

# Step 3 — Create Entity Models


Database table:

```text
Users
```


becomes:

```text
Entities/User.cs
```


Example:

```csharp
public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = "";

    public string Email { get; set; } = "";

    public string PasswordHash { get; set; } = "";


    public ICollection<UserRole> UserRoles { get; set; }
        = [];
}
```


Rule:

```text
One Database Table = One Entity
```


Checklist:

☐ Properties match database

☐ Navigation properties added

☐ Entity follows naming conventions

---

# Step 4 — Update DbContext


Register new entity:

```csharp
public DbSet<User> Users => Set<User>();
```


Update:

```text
OnModelCreating()
```


when adding:

- Relationships
- Keys
- Indexes
- Constraints

---

# Step 5 — Create Migration


After changing entities:


Build:

```bash
dotnet build
```


Create migration:

```bash
dotnet ef migrations add AddUserFeature
```


Apply:

```bash
dotnet ef database update
```


Verify SQL Server:

```text
Database

↓

Table Created

↓

Relationship Created
```

---

# Step 6 — Repository Layer


Purpose:

Handle database communication only.


Repository responsibilities:


Responsibility              Example


CRUD                        Create, Read, Update, Delete


Queries                     LINQ queries


Database Access             EF Core DbContext



Flow:

```text
Service

↓

Repository

↓

DbContext

↓

SQL Server
```


Example:

```csharp
public async Task<User?> GetById(Guid id)
{
    return await _context.Users
        .FirstOrDefaultAsync(x => x.Id == id);
}
```


Do NOT put:

- Password rules
- Permission checks
- Business decisions

inside repository.

---

# Step 7 — Service Layer


Purpose:

Contains business logic.


Responsibilities:


Logic                       Example


Validation                  Check required data


Business Rules              Prevent duplicate email


Security                    Hash passwords


Workflow                    Create related records



Flow:

```text
Controller

↓

Service

↓

Repository
```


Example:

Create User:


```text
Receive request

↓

Validate information

↓

Check duplicate email

↓

Hash password

↓

Assign role

↓

Save user
```

---

# Step 8 — Create DTOs


Purpose:

Separate API models from database models.


Never expose Entities directly.


Structure:

```text
DTOs

├── Users
│   ├── CreateUserDto.cs
│   ├── UpdateUserDto.cs
│   └── UserResponseDto.cs
│
└── Auth
    ├── LoginRequestDto.cs
    └── TokenResponseDto.cs
```


Flow:

```text
HTTP Request

↓

Request DTO

↓

Service

↓

Entity

↓

Database
```


Benefits:

- Security
- Cleaner API contracts
- Easier changes

---

# Step 9 — Validation


Validate incoming data before processing.


Options:


Tool                       Usage


DataAnnotations            Simple validation


FluentValidation           Advanced validation



Example:

```csharp
public class CreateUserDto
{
    [Required]
    public string Email { get; set; } = "";
}
```


Validate:

- Required fields
- Email format
- String length
- Business rules

---

# Step 10 — Controller Layer


Controller responsibilities:


Responsibility              Description


Receive Request             HTTP input


Call Service                Execute business operation


Return Response             HTTP result



Flow:

```text
HTTP Request

↓

Controller

↓

Service

↓

Response
```


Example:

```csharp
[HttpPost]
public async Task<IActionResult> Create(
    CreateUserDto request)
{
    var result = await _userService.Create(request);

    return Ok(result);
}
```


Controller should NOT contain:

- Database queries
- Password logic
- Complex business rules

---

# Step 11 — Authentication


Purpose:

Identify who the user is.


Components:


Component                  Purpose


JWT                        Access token


Refresh Token              Renew access


Password Hashing           Secure passwords



Login Flow:


```text
User Login

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


---

# Step 12 — Authorization


Purpose:

Control what users can do.


Types:


Authorization Type          Example


Role-Based                  Admin


Permission-Based            Users.Create


Policy-Based                Custom rules



Example:

```text
Admin

├── Users.Create

├── Users.Read

├── Users.Update

└── Users.Delete
```

---

# Step 13 — Middleware


Purpose:

Handle requests globally.


Common middleware:


Middleware                  Purpose


Exception Handler           Global errors


Logging                    Track requests


Authentication             Validate identity


Authorization              Check access



Flow:

```text
Request

↓

Middleware

↓

Controller

↓

Response
```

---

# Step 14 — Testing


Tools:


Tool                       Usage


Swagger                    Manual API testing


Postman                    API scenarios


xUnit                      Unit testing


Moq                        Mock dependencies



Test:

☐ CRUD operations

☐ Validation

☐ Authentication

☐ Authorization

☐ Error handling

---

# Step 15 — Deployment


Before production:


Task                       Description


Connection String          Production database


Environment Variables      Secure configuration


Migration                  Update database


Logging                    Monitor application


Publish                    Deploy API



---

# Complete Feature Workflow


```text
Requirement

↓

Database Design

↓

Entity

↓

DbContext

↓

OnModelCreating

↓

Migration

↓

Database Update

↓

Repository

↓

Service

↓

DTO

↓

Validation

↓

Controller

↓

Swagger/Postman Test

↓

Deployment
```

---

# Runtime Request Flow


When a client calls your API:


```text
Client

↓

HTTP Request

↓

Middleware

↓

Authentication

↓

Authorization

↓

Controller

↓

Service

↓

Repository

↓

DbContext

↓

SQL Server

↓

HTTP Response

↓

Client
```

---

# Daily EF Core Workflow


When changing database models:


```text
Modify Entity

↓

Modify OnModelCreating

↓

dotnet build

↓

dotnet ef migrations add MigrationName

↓

dotnet ef database update

↓

Verify SQL Server
```

---

# Mental Model


When building any feature, think:


```text
Requirement

↓

Database

↓

Entity

↓

DbContext

↓

Migration

↓

Database

↓

Repository

↓

Service

↓

DTO

↓

Validation

↓

Controller

↓

HTTP Response
```

---

# Technology Stack


Category                   Technology


Framework                  ASP.NET Core 8


Language                   C# 12


ORM                        Entity Framework Core 8


Database                   SQL Server Express


Authentication             JWT + Refresh Tokens


Authorization              RBAC + Permissions


Validation                 FluentValidation


Logging                    Serilog


Testing                    xUnit + Moq


Documentation              Swagger / OpenAPI

# End of Section 2


# Section 3: Complete ASP.NET Core Web API Example

> User API Flow

Implement:

```
POST /api/users
```

Purpose:

> Create a new user with a role.

---

# Request Flow

```
HTTP Request

      |
      v

Controller

      |
      v

Service

      |
      v

Repository

      |
      v

DbContext

      |
      v

SQL Database
```

---

# Additional Components

```
Authentication
    -> Password hashing and JWT handling

Authorization
    -> Check user roles and permissions

Middleware
    -> Global request processing

DTOs
    -> Control API input and output

Entities
    -> Represent database tables

Migrations
    -> Track database changes
```

---

# Project Structure

```
IAM_API

Controllers
 └── UsersController.cs

Data
 └── IAMContext.cs

Entities
 ├── User.cs
 ├── Role.cs
 └── UserRole.cs

DTOs
 ├── CreateUserDto.cs
 └── UserResponseDto.cs

Repositories
 ├── IUserRepository.cs
 └── UserRepository.cs

Services
 ├── IUserService.cs
 └── UserService.cs

Authentication
 └── PasswordHasherService.cs

Authorization
 └── RoleChecker.cs

Middleware
 └── ExceptionMiddleware.cs

Migrations
 └── Migration files
```

---

# 1. Entity Layer

Entities represent database tables.

## User.cs

```csharp
public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = "";

    public string Email { get; set; } = "";

    public string PasswordHash { get; set; } = "";


    public ICollection<UserRole> UserRoles { get; set; }
        = new List<UserRole>();
}
```

Database:

```
Users
----------------
Id
Username
Email
PasswordHash
```

---

## Role.cs

```csharp
public class Role
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
}
```

Database:

```
Roles
----------------
Id
Name
```

---

## UserRole.cs

Junction table between User and Role.

```csharp
public class UserRole
{
    public Guid UserId { get; set; }

    public int RoleId { get; set; }


    public User User { get; set; } = null!;

    public Role Role { get; set; } = null!;
}
```

Database:

```
UserRoles
----------------
UserId
RoleId
```

---

# 2. DTO Layer

DTOs define what the API receives and returns.

The API should not directly expose Entity classes.

---

## CreateUserDto.cs

Request:

```json
{
    "username": "john",
    "email": "john@gmail.com",
    "password": "123456",
    "roleId": 2
}
```

Class:

```csharp
public class CreateUserDto
{
    public string Username { get; set; } = "";

    public string Email { get; set; } = "";

    public string Password { get; set; } = "";

    public int RoleId { get; set; }
}
```

---

## UserResponseDto.cs

Response:

```json
{
    "id": "abc123",
    "username": "john",
    "email": "john@gmail.com"
}
```

Class:

```csharp
public class UserResponseDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = "";

    public string Email { get; set; } = "";
}
```

---

# 3. Data Layer

DbContext connects Entity Framework Core to SQL Server.

```csharp
public class IAMContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }


    public IAMContext(
        DbContextOptions<IAMContext> options)
        : base(options)
    {

    }
}
```

Responsibility:

```
Application
      |
      v
DbContext
      |
      v
SQL Database
```

---

# 4. Repository Layer

Repository handles database operations only.

It should not contain business rules.

---

## IUserRepository.cs

```csharp
public interface IUserRepository
{
    Task Add(User user);

    Task<bool> EmailExists(string email);
}
```

---

## UserRepository.cs

```csharp
public class UserRepository : IUserRepository
{
    private readonly IAMContext _context;


    public UserRepository(IAMContext context)
    {
        _context = context;
    }


    public async Task Add(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();
    }


    public async Task<bool> EmailExists(string email)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email);
    }
}
```

Repository responsibility:

```
Service

    |

Repository

    |

SQL Query
```

Example:

```
INSERT INTO Users
```

---

# 5. Authentication Layer

Handles password security and JWT.

Example password hashing:

```csharp
public class PasswordHasherService
{
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt
            .HashPassword(password);
    }
}
```

Before:

```
123456
```

After:

```
$2a$11$92hd83hd83h....
```

Database stores the hash, not the original password.

---

# 6. Service Layer

Service contains business logic.

This is the brain of the application.

Responsibilities:

```
Validate data

      |

Hash password

      |

Create Entity

      |

Call Repository
```

Example:

```csharp
public class UserService
{
    private readonly IUserRepository _repository;

    private readonly PasswordHasherService _hasher;


    public UserService(
        IUserRepository repository,
        PasswordHasherService hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }


    public async Task<UserResponseDto> CreateUser(
        CreateUserDto dto)
    {

        if(await _repository.EmailExists(dto.Email))
        {
            throw new Exception("Email exists");
        }


        var user = new User
        {
            Id = Guid.NewGuid(),

            Username = dto.Username,

            Email = dto.Email,

            PasswordHash =
                _hasher.Hash(dto.Password)
        };


        await _repository.Add(user);


        return new UserResponseDto
        {
            Id = user.Id,

            Username = user.Username,

            Email = user.Email
        };
    }
}
```

---

# 7. Controller Layer

Controller handles HTTP requests only.

```csharp
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{

    private readonly UserService _service;


    public UsersController(UserService service)
    {
        _service = service;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateUserDto dto)
    {

        var result =
            await _service.CreateUser(dto);


        return Ok(result);

    }
}
```

Controller responsibility:

```
Receive JSON

      |

Call Service

      |

Return HTTP Response
```

---

# 8. Authorization Layer

Controls access using roles.

Example:

Only Admin can create users.

```csharp
[Authorize(Roles = "Admin")]
[HttpPost]
public async Task<IActionResult> Create(...)
```

JWT contains:

```
User:
john

Role:
Admin
```

Flow:

```
JWT Token

      |

Check Role

      |

Allow or Deny Request
```

---

# 9. Middleware Layer

Middleware handles global request processing.

Example:

Global exception handling.

Instead of:

```csharp
try
{

}
catch
{

}
```

inside every controller.

Create:

```csharp
public class ExceptionMiddleware
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            context.Response.StatusCode = 500;

            await context.Response
                .WriteAsync(ex.Message);
        }
    }
}
```

Pipeline:

```
Request

 |

Exception Middleware

 |

Authentication

 |

Authorization

 |

Controller
```

---

# 10. Migration Layer

Migrations track database changes.

Create migration:

```bash
dotnet ef migrations add InitialCreate
```

Creates:

```
Migrations

 |

20260729_InitialCreate.cs
```

Apply migration:

```bash
dotnet ef database update
```

Creates SQL tables.

Flow:

```
Entity Changes

      |

Migration

      |

Database Update
```

---

# Complete Request Example

Client sends:

```
POST /api/users
```

Body:

```json
{
    "username": "john",
    "email": "john@test.com",
    "password": "123456"
}
```

Execution:

```
1. Middleware receives request

2. Controller receives DTO

3. Controller calls UserService

4. UserService validates email

5. UserService hashes password

6. Repository saves User

7. DbContext sends SQL INSERT

8. SQL Server saves data

9. Response DTO returns to client
```

---

# Architecture Summary

```
Controller
    |
    | HTTP handling

Service
    |
    | Business logic

Repository
    |
    | Database operations

DbContext
    |
    | EF Core

SQL Database
```

Each layer has one responsibility.

# End of Section 3