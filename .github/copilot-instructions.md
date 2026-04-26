# Copilot Instructions for Commitments

## Project Overview
- .NET 9 + Angular solution for tracking personal commitments and daily activities.
- Backend is a **modular monolith**: a single ASP.NET Core host composes feature modules (Commitments, Identity, Dashboard, DigitalAssets) that share a process but keep their domain models, schemas, and `DbContext`s isolated.

## Repository Layout
- **backend/** — .NET 9 solution.
  - **src/Commitments.Api/** — ASP.NET Core host: composes modules, configures Swagger, CORS, MediatR, validation, etc.
  - **src/Commitments.Shared/** — Shared kernel: `BaseEntity`, `BaseDbContext` (soft-delete + audit), `ValidationBehavior`, `IEventBus` / `InMemoryEventBus`, integration events, HTTP filters.
  - **src/Modules/<Module>/** — One project per module containing `Controllers/`, `Features/`, `Model/`, `Data/<Module>DbContext.cs`, and `ModuleExtensions.cs`.
  - **tests/** — xUnit test projects.
- **frontend/** — Angular workspace (`ng`, Jest, Playwright).

## Key Patterns & Conventions
- **Aggregates** live in `Modules/<Module>/Model/<Aggregate>Aggregate/*`.
- **CQRS with MediatR** — Each feature is a self-contained file under `Modules/<Module>/Features/<Entity>/Commands/` or `Queries/` containing the request, response, optional validator, and handler.
- **Controllers** in `Modules/<Module>/Controllers/` map to MediatR requests.
- **EF Core** — One `DbContext` per module under `Modules/<Module>/Data/`. Each module has its own SQL Server schema.
- **Soft-delete** — `BaseDbContext` intercepts `SavingChanges` to set `IsDeleted` flags and audit timestamps (`CreatedOn`, `LastModifiedOn`).
- **Integration events** — Modules communicate in-process via `IEventBus`. Default implementation is `InMemoryEventBus`.
- **No cross-module navigation properties** — Modules reference entities in other modules by `Guid` only.
- **API versioning** uses `Asp.Versioning.Mvc`.

## Developer Workflows
- **Build backend**: `cd backend && dotnet restore && dotnet build`.
- **Run API**: `cd backend && dotnet run --project src/Commitments.Api`. Optional CLI args: `migratedb`, `seeddb`, `dropdb`, `stop`, `ci` (= `dropdb migratedb seeddb stop`).
- **Database setup**: Edit connection strings in `backend/src/Commitments.Api/appsettings.json` (`CommitmentsDb`, `IdentityDb`, `DashboardDb`, `DigitalAssetsDb`).
- **Test backend**: `cd backend && dotnet test`.
- **Frontend**: `cd frontend && npm install && npm start`. Tests: `npm test` (Jest), `npm run e2e` (Playwright).

## Adding a New Feature
1. Add the request, response, and handler in `Modules/<Module>/Features/<Entity>/Commands/` or `Queries/`.
2. Add a FluentValidation validator if the request needs validation.
3. Add the controller endpoint in `Modules/<Module>/Controllers/`.
4. Add unit tests in `backend/tests/<Module>.Api.Tests/`.

## Adding a New Module
1. Create `backend/src/Modules/<Module>/<Module>.Module.csproj` referencing `Commitments.Shared`.
2. Add `Controllers/`, `Features/`, `Model/`, `Data/<Module>DbContext.cs`, and `ModuleExtensions.cs` exposing `AddXxxModule(IConfiguration)`.
3. Reference the module from `backend/src/Commitments.Api/Commitments.Api.csproj` and call `services.AddXxxModule(configuration)` in `Program.cs`.
4. Add the module project to `backend/Commitments.sln`.
