# Commitments

Commitments is a modular sample application for managing personal commitments and tracking daily activity against them. The backend is decomposed into four independently deployable microservices orchestrated by .NET Aspire, communicating via Redis PubSub integration events, each with its own SQL Server database.

## Architecture

```
Commitments.AppHost (.NET Aspire Orchestrator)
├── Redis (PubSub message broker)
├── SQL Server
│   ├── CommitmentsDb
│   ├── DashboardDb
│   ├── IdentityDb
│   └── DigitalAssetsDb
├── Commitments.Api     → Commitments, Activities, Behaviours, Frequencies
├── Dashboard.Api       → Dashboards, Cards, CardLayouts, DashboardCards
├── Identity.Api        → Users, Profiles
└── DigitalAssets.Api   → Digital asset storage and retrieval
```

## Solution Structure

### Source Projects (`src/`)

| Project | Description |
|---|---|
| **Commitments.AppHost** | .NET Aspire orchestrator. Provisions Redis, SQL Server (4 databases), and all four API services. |
| **Commitments.ServiceDefaults** | Shared Aspire service defaults: OpenTelemetry, health checks, service discovery, HTTP resilience. |
| **Commitments.Shared** | Shared kernel: `BaseEntity`, `BaseDbContext` (soft-delete/audit), `ValidationBehavior`, `IEventBus`/`RedisEventBus`, integration events, HTTP filters, extensions. |
| **Commitments.Api** | Commitment-domain API: Commitments, Activities, Behaviours, BehaviourTypes, Frequencies, FrequencyTypes, Achievements. |
| **Commitments.Core** | Commitment-domain model: aggregate roots and `ICommitmentsDbContext`. |
| **Commitments.Infrastructure** | EF Core `CommitmentsDbContext` for the Commitments bounded context. |
| **Dashboard.Api** | Dashboard-domain API: Dashboards, DashboardCards, Cards, CardLayouts. |
| **Dashboard.Core** | Dashboard-domain model: `DashboardEntity`, `DashboardCard`, `Card`, `CardLayout`, `IDashboardDbContext`. |
| **Dashboard.Infrastructure** | EF Core `DashboardDbContext` for the Dashboard bounded context. |
| **Identity.Api** | Identity-domain API: Users, Profiles. Publishes `ProfileCreatedEvent`/`ProfileDeletedEvent`. |
| **Identity.Core** | Identity-domain model: `User`, `Profile`, `IIdentityDbContext`. |
| **Identity.Infrastructure** | EF Core `IdentityDbContext` for the Identity bounded context. |
| **DigitalAssets.Api** | DigitalAssets-domain API: CRUD and file upload for digital assets. |
| **DigitalAssets.Core** | DigitalAssets-domain model: `DigitalAsset`, `IDigitalAssetsDbContext`. |
| **DigitalAssets.Infrastructure** | EF Core `DigitalAssetsDbContext` for the DigitalAssets bounded context. |

### Test Projects (`tests/`)

| Project | Coverage |
|---|---|
| **Commitments.Api.Tests** | Controller tests for the Commitments API |
| **Commitments.Core.Tests** | Aggregate, DTO, and handler tests for Commitments domain |
| **Commitments.Infrastructure.Tests** | DbContext integration tests |
| **Dashboard.Api.Tests** | Controller tests for the Dashboard API |
| **Identity.Api.Tests** | Controller tests for the Identity API |
| **DigitalAssets.Api.Tests** | Controller tests for the DigitalAssets API |

### Frontend

- **Commitments.App** (`src/Commitments.App/`) -- Angular/TypeScript front-end (not part of the .NET solution).

## Key Concepts

- **Commitments & Activities** -- A commitment defines a target behaviour at a given frequency; activities represent what was actually done.
- **Frequencies** -- Model how often something should happen (e.g., per day) and link to commitments via `CommitmentFrequency`.
- **Profiles** -- Represent the person for whom commitments and activities are tracked. Managed by the Identity service; referenced by ID across service boundaries.
- **Dashboards & Cards** -- Provide a configurable view over commitments and related data. Managed by the Dashboard service.
- **Integration Events** -- Services communicate asynchronously via Redis PubSub (e.g., `ProfileCreatedEvent`, `ProfileDeletedEvent`).
- **Cross-service references** -- Services reference entities in other bounded contexts by plain `Guid` (no cross-service navigation properties).

## Getting Started

### Prerequisites

- .NET 9 SDK
- Node.js and npm (for the Angular front-end)
- Docker (recommended, for Aspire-managed Redis and SQL Server containers)

### Running with .NET Aspire

1. Restore and build:
   ```
   dotnet restore
   dotnet build
   ```

2. Run the Aspire orchestrator:
   ```
   dotnet run --project src/Commitments.AppHost
   ```
   This starts Redis, SQL Server (with 4 databases), and all four API services. The Aspire Dashboard will be available to monitor all services.

3. Each service exposes its own Swagger UI at its assigned port.

### Frontend: Angular App

1. Navigate to `src/Commitments.App`.
2. Install dependencies: `npm install`.
3. Run the dev server: `npm start`.

## Testing

Run all backend tests:
```
dotnet test
```

## Architecture Notes

- Each service follows **Clean Architecture** with CQRS via MediatR: Api (controllers + feature handlers), Core (domain model + DbContext interface), Infrastructure (EF Core implementation).
- **Soft-delete** is handled by `BaseDbContext` in `Commitments.Shared`, which intercepts `SavingChanges` to set `IsDeleted` flags and audit timestamps (`CreatedOn`, `LastModifiedOn`).
- **API versioning** uses `Asp.Versioning.Mvc`.
- **Validation** is handled by FluentValidation via a MediatR pipeline behavior (`ValidationBehavior<,>`).
