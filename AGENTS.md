# Investigation Management - AI Agent Guidelines

This file contains guidelines for AI coding agents working on this investigation management system.

**Note:** This repository is being phased out. Do not create new features here.

## Architecture - Vertical Slices (Preferred) & Legacy Clean Architecture

This codebase is in transition. Newer code uses **Vertical Slices** while older code follows **Clean Architecture**.

### Vertical Slices (Preferred)
- **Location**: `Investigations/Features/`
- **Pattern**: Query/Command records + Handler + DTOs + Parsers per feature
- **Data Access**: Direct SQL via `NpgsqlDataProvider.ExecuteRaw()` with `DataCallSettings`
- **Example structure**:
  ```
  Features/Auth/Login.cs, Register.cs
  Features/Tasks/CreateTask.cs
  Features/Cases/ListCases.cs
  ```

### Legacy (Being Phased Out)
- `Investigations/Infrastructure/Data/Repositories/` - Repositories (will be removed)

## Build Commands

```bash
dotnet build investigation-management.sln
dotnet build Investigations/Investigations.csproj
dotnet run --project Investigations
dotnet run --project Investigations.Database
dotnet clean && dotnet build
dotnet restore investigation-management.sln
```

## Testing Commands

```bash
dotnet test
dotnet test Investigations.Tests.Unit
dotnet test Investigations.Tests.Integration
dotnet test --filter "FullyQualifiedName~CreateTaskTests.CreateTask"
dotnet test -v detailed
dotnet test --collect:"XPlat Code Coverage"
```

## Code Style & Conventions

### Project Configuration
- **Target Framework**: .NET 10.0
- **Nullable Reference Types**: Enabled globally
- **Implicit Usings**: Enabled

### Naming Conventions
- **Classes/Interfaces**: PascalCase (e.g., `UsersRepository`)
- **Methods/Properties**: PascalCase (e.g., `GetUserById`)
- **Private Fields**: camelCase with underscore (e.g., `_connectionStrings`)
- **Namespaces**: Match folder structure

### Vertical Slice Structure
Each feature file has:
- **Query/Command records**: Request input with nested Response records
- **Handler class**: `Handle(Query)` and/or `Handle(Command)` methods
- **DTOs**: Nested classes for data transfer
- **Parsers**: Implement `ISqlDataParser<T>`

```csharp
public class CreateTask
{
    public record Query { ... }
    public record Command { ... }

    public class Handler(IConnectionStrings connectionStrings)
    {
        public async Task<MethodResponse<Query.Response>> Handle(Query query) { ... }
    }
}
```

### Formatting
- **Braces**: K&R style
- **Indentation**: 4 spaces
- **Line length**: Under 120 characters
- **File encoding**: UTF-8 with BOM

### Response Wrapper
- **Always use**: `MethodResponse<T>`
- **Success**: `MethodResponse<T>.Success(data)`
- **Failure**: `MethodResponse<T>.Failure(message)`

### Error Handling
- **Service Layer**: Return `MethodResponse<T>.Failure(message)`
- **Web Layer**: Use `ExceptionHandlingMiddleware`
- **Validation**: Use `Validate(out string? message)` pattern on Query/Command
- **Logging**: Use Serilog

### Database & Data Access
- **Use**: `NpgsqlDataProvider.ExecuteRaw()` with `DataCallSettings`
- **Parameters**: Use `dcs.AddParameter()` for safe parameterized queries
- **Parsers**: Implement `ISqlDataParser<T>`

### Authentication
- **Scheme**: Cookie-based
- **Security**: `HttpOnly = true`, `SecurePolicy = Always`, `SameSite = Lax`, 8-hour sliding expiration

### Types & Nullable
- **Reference Types**: Use `string?` for nullable
- **Value Types**: Use `int?` for nullable
- **Collections**: `IReadOnlyList<T>` for read-only, `IList<T>` for modifiable
- **Records**: Use `record` for immutable DTOs

### Testing Guidelines
- **Integration Tests**: Use `TestFixture` from `Investigations.Tests.Integration/Utilities/`
- **MethodResponse Pattern**: Always test success and failure paths:
  ```csharp
  var response = await _handler.Handle(query);
  Assert.True(response.Success);
  Assert.NotNull(response.Data);
  ```

## Development Workflow

### Adding New Features
1. Create folder `Investigations/Features/[FeatureName]/`
2. Add feature.cs with Query/Command records, Handler, DTOs, Parsers
3. Add Razor Page in `Investigations/Pages/`

### Code Quality
- Run `dotnet build` before committing
- Address all nullable warnings

## Important Notes
- **Repository Status**: Being phased out - use direct SQL via `NpgsqlDataProvider`
- **No Linting**: Consider adding EditorConfig or StyleCop
- **Logging**: Serilog with Console, File, and HTTP sinks
- **Single Project**: All code in `Investigations/` (not layered)
