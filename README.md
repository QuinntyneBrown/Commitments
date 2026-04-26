# Commitments

Commitments is a sample application for managing personal commitments and tracking daily activity against them. The backend is a **modular monolith**: a single ASP.NET Core host composes four feature modules (Commitments, Identity, Dashboard, DigitalAssets) that share a process but keep their domain models, schemas, and `DbContext`s isolated.

## Repository Layout

```
.
├── backend/         # .NET 9 solution (modular monolith)
│   ├── Commitments.sln
│   ├── src/
│   │   ├── Commitments.Api/         # ASP.NET Core host
│   │   ├── Commitments.Shared/      # Shared kernel
│   │   └── Modules/
│   │       ├── Commitments/         # Commitments module
│   │       ├── Identity/            # Identity module
│   │       ├── Dashboard/           # Dashboard module
│   │       └── DigitalAssets/       # DigitalAssets module
│   └── tests/
└── frontend/        # Angular workspace
```

## Architecture

```
Commitments.Api (single ASP.NET Core host)
├── Commitments.Shared        — BaseEntity, BaseDbContext, ValidationBehavior,
│                               InMemoryEventBus, integration events, filters
├── Modules/
│   ├── Commitments           — Commitments, Activities, Behaviours, Frequencies
│   ├── Identity              — Users, Profiles
│   ├── Dashboard             — Dashboards, Cards, CardLayouts, DashboardCards
│   └── DigitalAssets         — Digital asset storage and retrieval
└── SQL Server (one DbContext per module; separate schemas)
```

Each module:
- Is a class library that owns its **aggregate roots**, **`DbContext`**, **MediatR features** (commands/queries), and **controllers**.
- Exposes an `AddXxxModule(IConfiguration)` extension that the host calls once at startup.
- Communicates with other modules in-process via `IEventBus` (an in-memory pub/sub of `IIntegrationEvent`s) — no Redis, no cross-module DbContext access.

## Getting Started

### Prerequisites

- .NET 9 SDK
- Node.js and npm (for the Angular front-end)
- SQL Server (LocalDB, SQLEXPRESS, or container)

### Run the backend

```
cd backend
dotnet restore
dotnet build
dotnet run --project src/Commitments.Api
```

Swagger UI is served at the root of the host.

To apply migrations and seed reference data on startup:

```
dotnet run --project src/Commitments.Api -- migratedb seeddb
```

### Run the frontend

```
cd frontend
npm install
npm start
```

## Testing

```
cd backend
dotnet test
```

## Architecture Notes

- Each module follows **Clean Architecture** with CQRS via MediatR: controllers, feature handlers, domain model, and EF Core `DbContext` live together inside the module project.
- **Soft-delete** is handled by `BaseDbContext` in `Commitments.Shared`, which intercepts `SavingChanges` to set `IsDeleted` flags and audit timestamps (`CreatedOn`, `LastModifiedOn`).
- **API versioning** uses `Asp.Versioning.Mvc`.
- **Validation** is handled by FluentValidation via a MediatR pipeline behavior (`ValidationBehavior<,>`).
- **Integration events** between modules are dispatched in-process by `InMemoryEventBus` (e.g., `ProfileCreatedEvent`, `ProfileDeletedEvent`).
- **Cross-module references** are by plain `Guid` (no cross-module navigation properties).
