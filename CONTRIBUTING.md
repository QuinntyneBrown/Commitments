# Contributing

## Prerequisites

- .NET 9 SDK
- Docker (for Aspire-managed Redis and SQL Server containers)

## Building

```
dotnet restore
dotnet build
```

## Running

```
dotnet run --project src/Commitments.AppHost
```

This starts all four API services along with Redis and SQL Server via .NET Aspire. The Aspire Dashboard provides observability across all services.

## Testing

```
dotnet test
```

## Project Layout

The solution follows a consistent structure per bounded context:

```
src/<Service>.Api/            Controllers, MediatR feature handlers, DTOs, Program.cs
src/<Service>.Core/           Domain model (aggregates), DbContext interface
src/<Service>.Infrastructure/ EF Core DbContext implementation
tests/<Service>.Api.Tests/    Unit tests
```

Shared concerns live in:

- `Commitments.Shared` -- Base types, validation pipeline, event bus, HTTP filters
- `Commitments.ServiceDefaults` -- Aspire service defaults (telemetry, health checks)
- `Commitments.AppHost` -- Aspire orchestration (Redis, SQL Server, service wiring)

## Conventions

- **CQRS with MediatR** -- Each feature is a self-contained file under `Features/<Entity>/Commands/` or `Features/<Entity>/Queries/` containing the request, response, optional validator, and handler.
- **No cross-service navigation properties** -- Services reference entities in other bounded contexts by `Guid` only.
- **Soft-delete** -- All entities inherit from `BaseEntity` which provides `IsDeleted`, `CreatedOn`, and `LastModifiedOn`. Deletion sets the flag rather than removing the row.
- **API versioning** -- Controllers use `[ApiVersion("1.0")]` via `Asp.Versioning.Mvc`.
- **Integration events** -- Cross-service communication uses `IEventBus` (Redis PubSub). Events are defined in `Commitments.Shared/IntegrationEvents.cs`.

## Adding a New Feature

1. Add the request, response, and handler in `Features/<Entity>/Commands/` or `Queries/`.
2. Add a FluentValidation validator if the request needs validation.
3. Add the controller endpoint.
4. Add unit tests in the corresponding test project.

## Adding a New Service

1. Create `<Service>.Core`, `<Service>.Infrastructure`, and `<Service>.Api` projects following the existing pattern.
2. Register the service and its database in `Commitments.AppHost/Program.cs`.
3. Add all three projects to `Commitments.sln`.
4. Create a test project under `tests/`.
