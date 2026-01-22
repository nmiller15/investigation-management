# Investigation Management - Agent Guidelines

This repository contains a full-stack .NET application with PostgreSQL database for an investigations management system. The solution includes a Razor Pages web UI, service layer, shared models layer, and database migration runner, all built on a comprehensive PostgreSQL database schema with raw SQL access.

## .NET Project Structure

### Architecture Overview
The solution follows a layered architecture with four main projects:

1. **Investigations.Models** - Class library containing domain models, DTOs, service interfaces, and data access interfaces
2. **Investigations.App** - Service layer with business logic and data access implementations
3. **Investigations.Web** - Razor Pages web UI that consumes shared services from App project
4. **Investigations.Db** - Console application for running database migrations and setup

### Dependency Flow
```
Investigations.Web → Investigations.App → Investigations.Models
                ↘ Investigations.Db → (database)
```

- **Web project** depends on App and Models (consumes services directly)
- **App project** depends on Models (contains service layer and data repositories)
- **Models project** has minimal dependencies (Npgsql for data interfaces)
- **Db project** is standalone (runs migrations against database)

### Project Organization
- **Models/**: Domain entities, DTOs, request/response models, service interfaces
- **App/Services/**: Business logic and service implementations
- **App/Repositories/**: Data access repositories using raw SQL
- **Web/Pages/**: Razor Pages UI
- **Web/Services/**: UI-specific service adapters (if needed)
- **Db/Migrations/**: Database schema migration files
- **Db/Seeds/**: Database seed data files
- **Db/Setup/**: Database setup and configuration scripts

## Build/Development Commands

### .NET Commands
```bash
# Build entire solution
dotnet build

# Run specific projects
dotnet run --project Investigations.App
dotnet run --project Investigations.Web

# Run tests (when test projects are added)
dotnet test

# Create new migrations (when Entity Framework is added)
dotnet ef migrations add MigrationName --project Investigations.App
dotnet ef database update --project Investigations.App
```

### Database Commands
This project uses PostgreSQL with SQL migrations. Commands are typically run via psql:

```bash
# Create database and setup roles
psql -U postgres -f db/setup/00_create_db.sql
psql -U postgres -d core -f db/setup/01_roles.sql
psql -U postgres -d core -f db/setup/02_permissions.sql

# Run migrations using the custom .NET migration runner
dotnet run --project Investigations.Db

# Run production migrations
dotnet run --project Investigations.Db -- --prod

# Seed data execution (run after migrations)
# IMPORTANT: Complete manual codes in 001_codes.sql before running seeds
psql -U app_user -d core -f db/seeds/001_codes.sql
psql -U app_user -d core -f db/seeds/002_admin_user.sql
psql -U app_user -d core -f db/seeds/003_sample_clients.sql
psql -U app_user -d core -f db/seeds/004_sample_addresses.sql
psql -U app_user -d core -f db/seeds/005_sample_contacts.sql
```

## Database Schema Structure

### Core Tables
- `users` - System users with authentication and roles
- `cases` - Investigation cases linking subjects and clients
- `subjects` - People being investigated
- `clients` - Clients requesting investigations
- `case_notes` - Notes and documentation for cases
- `tasks` - Task management for cases
- `notifications` - System notifications
- `addresses` - Address management
- `codes` - Lookup tables for codes (case types, roles, etc.)

### Seed Data
- **Geographic Data**: Complete ISO-compliant US states (50), DC, territories (PR, GU, VI, AS, MP), and North American countries (US, CA, MX, GT, BN, SV, HN, NI, CR, PA)
- **Reference Codes**: Geographic codes are pre-loaded; business-specific codes (roles, case types, marital status, gender, contact methods) must be added manually to 001_codes.sql
- **Admin User**: System administrator with default password "admin" (ASP.NET hash placeholders to be completed)
- **Sample Data**: 3 sample clients (Acme Corporation, Metro Insurance Group, Jane Doe Law Firm) with addresses and contacts for development testing

### Link Tables
- `cases_subjects` - Many-to-many relationship
- `subjects_addresses_links` - Subject address relationships
- `users_addresses_links` - User address relationships
- `clients_addresses_links` - Client address relationships
- `contacts_addresses_links` - Contact address relationships
- `clients_contacts_links` - Client contact relationships

### Views
- Views are created for all major entities (v_cases, v_subjects, v_clients, etc.)
- Views provide simplified access to related data

## C# Code Style Guidelines

### Naming Conventions
- **Classes**: PascalCase (e.g., `InvestigationCase`, `UserService`)
- **Interfaces**: PascalCase with `I` prefix (e.g., `IUserService`, `IRepository<T>`)
- **Methods**: PascalCase (e.g., `GetCaseById`, `CreateUser`)
- **Properties**: PascalCase (e.g., `CaseId`, `FirstName`, `IsActive`)
- **Fields**: camelCase with underscore prefix for private fields (e.g., `_dbContext`, `_logger`)
- **Constants**: PascalCase (e.g., `MaxRetryCount`, `DefaultPageSize`)
- **Namespaces**: PascalCase (e.g., `Investigations.Models`, `Investigations.App.Services`)

### File Organization
- **One class per file** with matching filename
- **Folder structure** follows namespace hierarchy
- **Models**: `Investigations.Models/Entities/`, `Investigations.Models/DTOs/`, `Investigations.Models/Interfaces/`
- **Services**: `Investigations.App/Services/`, `Investigations.App/Repositories/`
- **Pages**: `Investigations.Web/Pages/` organized by feature

### Code Patterns
- **Async/Await**: Use async methods for I/O operations (database, HTTP calls)
- **Dependency Injection**: Constructor injection for services
- **Repository Pattern**: Use BaseSqlRepository for stored procedure access
- **DTO Pattern**: Separate domain models from API contracts
- **Validation**: Use Data Annotations or FluentValidation
- **Data Access**: Use ISqlDataParser<T> interface for mapping SQL results to objects

### Database Access Guidelines
- **Raw SQL**: Use stored procedures with BaseSqlRepository for all data access
- **Connection Management**: Use Npgsql for PostgreSQL connectivity
- **Parameter Handling**: Use DataCallSettings for procedure parameters
- **Result Mapping**: Implement ISqlDataParser<T> for mapping database results
- **Transactions**: Use Npgsql transactions for multi-operation scenarios
- **Stored Procedures**: Follow naming convention `sp_{table}_{action}` (e.g., `sp_users_get_by_id`)

### API Development
- **Service Layer**: Business logic is implemented in Services layer of App project
- **Data Access**: All database operations go through Repository pattern with stored procedures
- **Error Handling**: Use proper exception handling and logging
- **Future APIs**: When adding APIs, use Minimal APIs in App project
- **Documentation**: Include XML comments for future OpenAPI generation

## SQL Code Style Guidelines

### Naming Conventions
- **Tables**: snake_case, plural (e.g., `users`, `case_notes`)
- **Columns**: snake_case, descriptive (e.g., `inserted_datetime`, `updated_by_user_key`)
- **Primary Keys**: `{table}_key` (e.g., `user_key`, `case_key`)
- **Foreign Keys**: `{referenced_table}_key` (e.g., `user_key`, `case_key`)
- **Constraints**: descriptive names (e.g., `fk_cases_subject_id`)
- **Views**: prefixed with `v_` (e.g., `v_cases`, `v_subjects`)

### Data Types
- **Primary Keys**: `INT` with `GENERATED ALWAYS AS IDENTITY` (starting at 100)
- **Text fields**: `VARCHAR(n)` with appropriate length limits
- **Dates/Times**: `DATE` for dates, `TIMESTAMPTZ` for timestamps
- **Booleans**: `BOOLEAN`
- **Authentication**: `BYTEA` for password hashes/salts

### Column Patterns
- **Audit Fields**: All tables include:
  - `inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now()`
  - `inserted_by_user_key INT`
  - `updated_datetime TIMESTAMPTZ`
  - `updated_by_user_key INT`
- **Status Fields**: Use appropriate codes from `codes` table
- **Auto-increment**: Use `GENERATED ALWAYS AS IDENTITY` for sequence numbers

### SQL Formatting
- Use uppercase for SQL keywords (`CREATE`, `TABLE`, `NOT NULL`)
- Use lowercase for table/column names
- Indent consistently (4 spaces recommended)
- One column per line for better readability
- Include comments for complex business logic

### Migration Files
- **Naming**: `###_{description}.sql` (e.g., `002_create_table_users.sql`)
- **Order**: Sequential numbering ensures proper execution order
- **Content**: Each migration should be idempotent where possible
- **Schema Migrations**: Track migration versions in `schema_migrations` table

### Seed Data Files
- **Location**: `db/seeds/` directory
- **Naming**: `###_{description}.sql` (e.g., `001_codes.sql`)
- **Execution Order**: Must run after migrations and in sequence
- **Dependency Chain**: Geographic data → business codes → admin user → sample data
- **Manual Setup**: Business-specific codes in 001_codes.sql must be completed before running seed data

### Security & Permissions
- Use `app_user` role for application access
- Grant minimal necessary permissions
- Separate admin and application roles
- Password management handled out of band (not in version control)

### Database Design Principles
- Use identity columns for primary keys starting at 100
- Implement proper foreign key relationships
- Create views for complex joins and frequently accessed data
- Use codes table for lookup values instead of ENUMs
- Include audit trails on all data modifications

## Testing

### .NET Testing
```bash
# Run all tests in solution
dotnet test

# Run tests for specific project
dotnet test Investigations.Models.Tests
dotnet test Investigations.App.Tests
dotnet test Investigations.Web.Tests

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Database Testing
- Schema validation against DBML diagram
- Data integrity testing with sample data
- Performance testing on views and queries
- Migration testing (up/down scenarios)

## Notes for Agents

- This is a PostgreSQL-only project with .NET application layer
- Database schema is complete; application code needs to be implemented
- All database changes should be made via new migration files
- Never modify existing migrations directly
- Follow the established naming and formatting conventions for both SQL and C#
- Consider data privacy and security when making changes
- Complete business-specific codes in seeds/001_codes.sql before running seed data
- ASP.NET Identity password hashes must be generated via application code for admin user
- Use dependency injection for all services and repositories
- Follow async/await patterns for database operations
- Use BaseSqlRepository pattern with stored procedures for data access
