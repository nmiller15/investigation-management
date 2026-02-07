# Investigation Management - AI Agent Guidelines

This file contains guidelines for AI coding agents working on this investigation management system.

## Project Overview

This is a .NET 10.0 ASP.NET Core web application following clean architecture principles with PostgreSQL database. The system manages investigation cases with users, contacts, subjects, tasks, and related entities.

**Architecture Layers:**
- `Investigations.Web/` - Presentation layer (ASP.NET Core Razor Pages)
- `Investigations.App/` - Application/Business logic layer  
- `Investigations.Models/` - Domain/Entities layer
- `Investigations.Infrastructure/` - Infrastructure layer (data access, auth)
- `Investigations.Database/` - Database utilities/console app

## Build & Development Commands

```bash
# Build entire solution
dotnet build investigation-management.sln

# Build specific project
dotnet build Investigations.Web/Investigations.Web.csproj

# Run web application (development)
dotnet run --project Investigations.Web

# Run database console application
dotnet run --project Investigations.Database

# Clean and rebuild
dotnet clean && dotnet build

# No test projects currently exist - add testing strategy before writing tests
```

## Code Style & Conventions

### Project Configuration
- **Target Framework**: .NET 10.0
- **Nullable Reference Types**: Enabled globally
- **Implicit Usings**: Enabled
- **Package References**: Use latest stable versions

### Naming Conventions
- **Classes/Interfaces**: PascalCase (e.g., `UsersRepository`, `IUsersService`)
- **Methods/Properties**: PascalCase (e.g., `GetUserById`, `FirstName`)
- **Private Fields**: camelCase with underscore prefix (e.g., `_connectionStrings`)
- **Constants**: PascalCase (e.g., `DefaultTimeout`)
- **Namespaces**: Match folder structure (e.g., `Investigations.App.Users`)

### Code Organization
- **File-per-class**: One class per file
- **Namespace declarations**: First line, no blank line before
- **Using statements**: 
  - System namespaces first, then third-party, then project namespaces
  - Group by namespace, no sorting required (implicit usings enabled)
  - Remove unused usings

### Class Design Patterns
- **Base Classes**: Use `BaseAuditModel` for entities requiring audit trails
- **Response Wrapper**: Use `MethodResponse<T>` for service method returns
- **Repository Pattern**: Inherit from `BaseSqlRepository`, implement interfaces
- **Dependency Injection**: Register services in `ServiceCollectionExtensions`
- **Service Lifetime**: Use `Scoped` for repositories/services, `Singleton` for configuration

### Error Handling
- **Service Layer**: Return `MethodResponse<T>.Failure(message)` for errors
- **Web Layer**: Use `ExceptionHandlingMiddleware` in production
- **Validation**: Check for null payloads in success responses
- **Logging**: Use Serilog for structured logging

### Database & Data Access
- **Connection**: Use `IConnectionStrings` injection, access via `BaseSqlRepository`
- **SQL Operations**: Use stored procedures/functions via `DataCallSettings`
- **Parameterization**: Use `DbParam` extensions for safe SQL parameters
- **Data Parsing**: Implement `ISqlDataParser<T>` for custom object mapping

### Authentication & Security
- **Scheme**: Cookie-based authentication with secure settings
- **Security**: 
  - `HttpOnly = true`
  - `SecurePolicy = Always` 
  - `SameSite = Lax`
  - 8-hour sliding expiration
- **Authorization**: Use ASP.NET Core authorization attributes

### Async Programming
- **Database Operations**: Use async/await consistently
- **Service Methods**: Return `Task<MethodResponse<T>>`
- **Naming**: Append `Async` suffix to async methods

### Configuration Management
- **Connection Strings**: Environment-specific appsettings files
- **Extensions**: Use extension methods in `ConfigurationExtensions`
- **Dependency Setup**: Centralized in `ServiceCollectionExtensions`

### Testing Guidelines
- **No Tests Currently**: This project lacks test infrastructure
- **Recommended Setup**:
  - Add xUnit test projects for each layer
  - Use integration tests for database operations
  - Mock repositories for unit tests
  - Test both success and failure paths for `MethodResponse<T>`

## Development Workflow

### Adding New Features
1. **Domain Model**: Add entity in `Investigations.Models/`, inherit from `BaseAuditModel` if needed
2. **Interface**: Define repository/service interfaces in appropriate `Interfaces/` folder
3. **Implementation**: 
   - Repository in `Investigations.Infrastructure/Data/`
   - Service in `Investigations.App/[FeatureName]/`
4. **Registration**: Add to `ServiceCollectionExtensions`
5. **Web Layer**: Add Razor Page or API endpoint if needed

### Database Changes
- **Schema**: Use PostgreSQL stored procedures/functions
- **Connection**: All access through `BaseSqlRepository` pattern
- **Parameters**: Use `DbParam` extensions for type safety

### Code Quality Checks
- **Build**: Always run `dotnet build` before committing
- **Nullable**: Ensure all nullable warnings are addressed
- **Dependencies**: Keep NuGet packages updated to latest stable versions

## Important Notes

- **No Linting Configured**: Consider adding EditorConfig or StyleCop for consistent formatting
- **Database Console App**: `Investigations.Database` appears incomplete
- **Logging**: Serilog configured with Console, File, and HTTP sinks
- **Production Ready**: Exception handling and security configurations in place
- **Clean Architecture**: Follows SOLID principles with clear layer separation