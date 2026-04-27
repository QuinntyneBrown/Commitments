# Commitments

Commitments is a sample application for managing personal commitments and tracking daily activity against them. The backend is a **modular monolith**: a single ASP.NET Core host composes four feature modules (Commitments, Identity, Dashboard, DigitalAssets) that share a process but keep their domain models, database schemas, and `DbContext`s isolated.

## Repository Layout

```
.
├── backend/                            # .NET 9 solution (modular monolith)
│   ├── Commitments.sln
│   ├── src/
│   │   ├── Commitments.Api/            # ASP.NET Core host (Program.cs, Hubs, Realtime, Middleware)
│   │   ├── Commitments.Shared/         # Shared kernel
│   │   └── Modules/
│   │       ├── Commitments/            # Commitments module (Commitments, Activities, Behaviours, Frequencies)
│   │       ├── Identity/               # Identity module (Users, Profiles)
│   │       ├── Dashboard/              # Dashboard module (Dashboards, Cards, CardLayouts)
│   │       └── DigitalAssets/          # DigitalAssets module
│   └── tests/                          # xUnit test projects (one per module + Shared, Core, Infrastructure, Api)
└── frontend/                           # Angular 21 workspace
    └── projects/
        ├── commitments-app/            # Application shell
        ├── commitments-ui/             # Reusable UI library
        ├── dashboard-framework/        # Dashboard host/runtime library
        └── commitments-dashboard-plugin/ # Commitments-specific dashboard tiles
```

## Architecture

```
Commitments.Api (single ASP.NET Core host)
├── Commitments.Shared        — BaseEntity, BaseDbContext, ValidationBehavior,
│                               InMemoryEventBus, integration events,
│                               IRealtimePublisher, exception filters
├── Modules/
│   ├── Commitments           — Commitments, Activities, Behaviours, Frequencies, Achievements, GoalProgress
│   ├── Identity              — Users, Profiles
│   ├── Dashboard             — Dashboards, Cards, CardLayouts, DashboardCards
│   └── DigitalAssets         — Digital asset storage and retrieval
├── SignalR Hub (/hub)        — Per-profile real-time updates (goal progress, dashboard tile
│                               invalidation, note/tag envelopes)
└── SQL Server (one DbContext per module; separate schemas)
```

Each module:
- Is a class library that owns its **domain models**, **`DbContext`**, **MediatR features** (commands/queries), and **controllers**.
- Exposes an `AddXxxModule(IConfiguration)` extension that the host calls once at startup.
- Communicates with other modules in-process via `IEventBus` (an in-memory pub/sub of `IIntegrationEvent`s) — no Redis, no cross-module DbContext access.

Module layout:

```
Modules/<Module>/
├── Controllers/      # thin HTTP adapters
├── Data/             # EF Core DbContext and module persistence contract
├── Domain/           # entities owned by the module (organized by aggregate)
├── Features/         # vertical command/query slices (MediatR)
└── ModuleExtensions.cs
```

## Real-time

The host exposes a SignalR hub at `/hub`. On connect, clients are placed into a per-profile group (`profile:<profileId>`), keyed off the `ProfileId` header (or query string for SignalR negotiation). Hosted services publish to that group:

- `GoalProgressUpdatedRealtimeNotifier` — pushes goal progress updates.
- `DashboardTileInvalidationNotifier` — invalidates dashboard tile snapshots when relevant data changes.
- `NoteTagRealtimeNotifier` — broadcasts note/tag envelope events.

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

Swagger UI is served at the root of the host. A health check is exposed at `/health`.

To apply migrations and seed reference data on startup:

```
dotnet run --project src/Commitments.Api -- migratedb seeddb
```

Other CLI args supported by `Program.cs`: `dropdb`, `migratedb`, `seeddb`, `stop`, and `ci` (a shortcut for `dropdb migratedb seeddb stop`).

### Run the frontend

```
cd frontend
npm install
npm start
```

The app serves on `http://localhost:4200` and points at the API at `http://localhost:52748/`.

Common scripts:

- `npm run build` / `npm run build:prod` — build the four Angular projects in dependency order.
- `npm test` — Jest unit tests.
- `npm run e2e` — Playwright end-to-end tests.
- `npm run storybook` — Storybook for UI components.
- `npm run lint` / `npm run format` — ESLint and Prettier.

## Testing

Backend:

```
cd backend
dotnet test
```

Frontend:

```
cd frontend
npm test          # Jest unit tests
npm run e2e       # Playwright end-to-end tests
```

## Architecture Notes

- **Vertical-slice + CQRS via MediatR** in each module: request/response types, validators, handlers, and DTO mapping stay close to the feature they support.
- EF Core is used directly from feature handlers through the module `DbContext` contract. Repositories are only added when there is a real persistence boundary to hide.
- **Soft-delete and audit** are handled by `BaseDbContext` in `Commitments.Shared`, which intercepts `SavingChanges` to set `IsDeleted` flags and audit timestamps (`CreatedOn`, `LastModifiedOn`).
- **API versioning** uses `Asp.Versioning.Mvc`; the default version is `1.0` and is reported in responses.
- **Validation** is handled by FluentValidation via a MediatR pipeline behavior (`ValidationBehavior<,>`).
- **Integration events** between modules are dispatched in-process by `InMemoryEventBus` (e.g., `ProfileCreatedEvent`, `ProfileDeletedEvent`).
- **Cross-module references** are by plain `Guid` (no cross-module navigation properties, no cross-module `Include`s).
- **Profile context** is read from the `ProfileId` request header via `HttpContextAccessorExtensions.GetProfileId()`.
- **Auth** uses JWT bearer tokens; `JwtQueryStringAuthMiddleware` lifts the token from the query string for SignalR negotiation.
- **Logging** uses Serilog (console sink, configuration-driven).
- **Frontend** is an Angular 21 workspace consuming the API and the SignalR hub via `@microsoft/signalr`. Dashboards are composed from a `dashboard-framework` runtime plus pluggable tile sets (`commitments-dashboard-plugin` and host-provided tile components).
