# Contributing

## Prerequisites

- .NET 9 SDK
- Node.js and npm (for the Angular front-end)
- SQL Server (LocalDB, SQLEXPRESS, or container)

## Building

```
cd backend
dotnet restore
dotnet build
```

## Running

```
cd backend
dotnet run --project src/Commitments.Api
```

To apply migrations and seed reference data on startup:

```
dotnet run --project src/Commitments.Api -- migratedb seeddb
```

## Testing

```
cd backend
dotnet test
```

## Project Layout

The backend is a modular monolith. Each module is a class library that owns its
domain model, EF Core `DbContext`, MediatR features, and controllers:

```
backend/src/Commitments.Api/        ASP.NET Core host; composes modules in Program.cs
backend/src/Commitments.Shared/     Shared kernel
backend/src/Modules/<Module>/       Module project (Controllers, Features, Model, Data)
backend/tests/<Module>.Api.Tests/   Unit tests
```

Shared concerns live in `Commitments.Shared`:

- `BaseEntity`, `BaseDbContext` (soft-delete + audit)
- `ValidationBehavior` (MediatR pipeline behavior wrapping FluentValidation)
- `IEventBus` / `InMemoryEventBus` for in-process integration events
- HTTP filters (`HttpGlobalExceptionFilter`) and CORS settings

## Conventions

- **CQRS with MediatR** -- Each feature is a self-contained file under `Features/<Entity>/Commands/` or `Features/<Entity>/Queries/` containing the request, response, optional validator, and handler.
- **No cross-module navigation properties** -- Modules reference entities in other modules by `Guid` only.
- **Soft-delete** -- All entities inherit from `BaseEntity` which provides `IsDeleted`, `CreatedOn`, and `LastModifiedOn`. Deletion sets the flag rather than removing the row.
- **API versioning** -- Controllers use `[ApiVersion("1.0")]` via `Asp.Versioning.Mvc`.
- **Integration events** -- Cross-module communication uses `IEventBus`. Events are defined in `Commitments.Shared/IntegrationEvents.cs`.

## Adding a New Feature

1. Add the request, response, and handler in `Features/<Entity>/Commands/` or `Queries/` inside the module project.
2. Add a FluentValidation validator if the request needs validation.
3. Add the controller endpoint to the module's `Controllers/` folder.
4. Add unit tests in the corresponding test project.

## Adding a New Module

1. Create a new module project under `backend/src/Modules/<Module>/`.
2. Add `Controllers/`, `Features/`, `Model/`, `Data/` (with the module's `DbContext`), and a `ModuleExtensions.cs` exposing `AddXxxModule(IConfiguration)`.
3. Reference the module from `backend/src/Commitments.Api/Commitments.Api.csproj` and call `services.AddXxxModule(configuration)` in `Program.cs`.
4. Add the module to `backend/Commitments.sln`.
5. Create a test project under `backend/tests/`.
