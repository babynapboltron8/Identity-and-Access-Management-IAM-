# Feature Add Workflow

When adding another feature to this API, follow this order:

1. Entities
2. DbContext
3. Relationships
4. Migrations
5. Repository
6. Service
7. Controller
8. Test with Swagger/Postman

## Simple Flow

### 1. Entities

Create the data model and add the new entity class in the Entities folder.

### 2. DbContext

Update the DbContext and add the new DbSet if needed.

### 3. Relationships

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

### 7. Controller

Create or update the controller endpoint for the feature.

### 8. Test with Swagger/Postman

Run the endpoint and verify the request and response.

## Quick Reminder

Feature flow:

Entities -> DbContext -> Relationships -> Migrations -> Repository -> Service -> Controller -> Test
