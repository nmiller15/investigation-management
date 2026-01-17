# Investigation Management - Agent Guidelines

This repository contains a full-stack .NET application with PostgreSQL database for an investigations management system. The solution includes a Razor Pages web UI, minimal API backend, and shared models layer, all built on a comprehensive PostgreSQL database schema.

## .NET Project Structure

### Architecture Overview
The solution follows a layered architecture with three main projects:

1. **Investigations.Models** - Class library containing domain models, DTOs, and service interfaces
2. **Investigations.App** - Minimal API project with business logic and service implementations
3. **Investigations.Web** - Razor Pages web UI that consumes shared services from App project

### Dependency Flow
```
Investigations.Web → Investigations.App → Investigations.Models
```

- **Web project** depends on App and Models (can consume services directly or call API endpoints)
- **App project** depends on Models (contains service layer and API endpoints)
- **Models project** has no dependencies (pure domain models and interfaces)

### Project Organization
- **Models/**: Domain entities, DTOs, request/response models, service interfaces
- **App/Services/**: Business logic and service implementations
- **App/Controllers/**: Minimal API endpoints
- **Web/Pages/**: Razor Pages UI
- **Web/Services/**: UI-specific service adapters (if needed)

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

# Run migrations (in order)
psql -U app_user -d core -f db/migrations/001_create_table_schema_migrations.sql
psql -U app_user -d core -f db/migrations/002_create_table_users.sql
# ... continue through all migrations

# Alternative: Use a migration tool like sqitch or flyway if configured
# sqitch deploy
# flyway migrate

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
- **Controllers**: `Investigations.App/Controllers/`
- **Pages**: `Investigations.Web/Pages/` organized by feature

### Code Patterns
- **Async/Await**: Use async methods for I/O operations (database, HTTP calls)
- **Dependency Injection**: Constructor injection for services
- **Repository Pattern**: Use generic repositories for data access
- **DTO Pattern**: Separate domain models from API contracts
- **Validation**: Use Data Annotations or FluentValidation

### Entity Framework Guidelines
- **DbContext**: Named `InvestigationDbContext` in App project
- **Entities**: Map to database tables with proper navigation properties
- **Migrations**: Use descriptive names and review before applying
- **Queries**: Use async methods (ToListAsync, FirstOrDefaultAsync, etc.)
- **Transactions**: Use DbContext transaction for multi-entity operations

### API Development
- **Minimal APIs**: Use endpoint definitions in App project
- **HTTP Methods**: Proper use of GET, POST, PUT, DELETE
- **Status Codes**: Return appropriate HTTP status codes
- **Error Handling**: Use problem details for API errors
- **Documentation**: Include XML comments for OpenAPI generation

## SQL Code Style Guidelines

### Naming Conventions
- **Tables**: snake_case, plural (e.g., `users`, `case_notes`)
- **Columns**: snake_case, descriptive (e.g., `inserted_datetime`, `updated_by_user_id`)
- **Primary Keys**: `{table}_id` (e.g., `user_id`, `case_id`)
- **Foreign Keys**: `{referenced_table}_id` (e.g., `user_id`, `case_id`)
- **Constraints**: descriptive names (e.g., `fk_cases_subject_id`)
- **Views**: prefixed with `v_` (e.g., `v_cases`, `v_subjects`)

### Data Types
- **Primary Keys**: `UUID` with `gen_random_uuid()` default
- **Text fields**: `VARCHAR(n)` with appropriate length limits
- **Dates/Times**: `DATE` for dates, `TIMESTAMPTZ` for timestamps
- **Booleans**: `BOOLEAN`
- **Authentication**: `BYTEA` for password hashes/salts

### Column Patterns
- **Audit Fields**: All tables include:
  - `inserted_datetime TIMESTAMPTZ NOT NULL DEFAULT now()`
  - `inserted_by_user_id UUID`
  - `updated_datetime TIMESTAMPTZ`
  - `updated_by_user_id UUID`
- **Status Fields**: Use appropriate codes from `codes` table
- **Auto-increment**: Use `DEFAULT INCREMENT` for sequence numbers

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
- Use UUIDs for primary keys to avoid sequence conflicts
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
- Entity Framework should be added to replace raw SQL queries in application code
