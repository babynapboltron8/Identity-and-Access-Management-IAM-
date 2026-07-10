# ==========================================

# ASP.NET Core 8 + EF Core 8 Setup

# ==========================================

# Check .NET SDK Version

dotnet --version

# Check installed SDKs

dotnet --list-sdks

# Check installed runtimes

dotnet --list-runtimes

# ==========================================

# Install EF Core 8 Packages

# ==========================================

dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0

dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0

dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0

dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 8.0.23

# ==========================================

# Install dotnet-ef 8 CLI Tool

# ==========================================

dotnet tool uninstall -g dotnet-ef

dotnet tool install -g dotnet-ef --version 8.0

dotnet tool update -g dotnet-ef --version 8.0

# Check EF Core CLI Version

dotnet ef --version

# ==========================================

# Install ASP.NET Core Code Generator 8

# ==========================================

dotnet tool uninstall -g dotnet-aspnet-codegenerator

dotnet tool install -g dotnet-aspnet-codegenerator --version 8.0

dotnet tool update -g dotnet-aspnet-codegenerator --version 8.0

# Check Code Generator Version

dotnet aspnet-codegenerator --version

# ==========================================

# Verify Installed Packages

# ==========================================

dotnet list package

# ==========================================

# Restore, Clean, Build

# ==========================================

dotnet restore

dotnet clean

dotnet build

# ==========================================

# Run Application

# ==========================================

dotnet watch

# ==========================================

# EF Core Migration Commands

# ==========================================

# Create Migration

dotnet ef migrations add InitialCreate

# Update Database

dotnet ef database update

# List Migrations

dotnet ef migrations list

# Remove Last Migration

dotnet ef migrations remove

# ==========================================

# Scaffolding (dotnet aspnet-codegenerator)

# ==========================================

dotnet aspnet-codegenerator controller -name UserController -async -api -m User -dc IAMContext -outDir \_Controllers
