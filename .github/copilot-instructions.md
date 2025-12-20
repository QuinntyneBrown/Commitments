# Copilot Instructions for Commitments

## Project Overview
- Modular .NET 8 + Angular solution for tracking personal commitments and daily activities.
- Clean separation: API (ASP.NET Core), domain logic (Core), infrastructure (EF Core), and front-end (Angular).

## Solution Structure
- **src/Commitments.Api/**: ASP.NET Core Web API controllers, startup, config.
- **src/Commitments.Core/**: Domain aggregates (Commitment, Activity, Behaviour, Frequency, Profile), MediatR handlers, cross-cutting services (messaging, security, validation, telemetry).
- **src/Commitments.Infrastructure/**: EF Core DbContext, migrations, data access.
- **src/Commitments.App/**: Angular front-end, TypeScript, e2e tests.

## Key Patterns & Conventions
- **Aggregates**: Each domain concept (e.g., Commitment, Activity) is an aggregate in `Core/AggregateModel/*`.
- **MediatR**: Query/command handlers for business logic.
- **Services**: Messaging, security, validation, and telemetry in `Core/Services/*`.
- **Controllers**: API endpoints in `Api/Controllers/*` map to aggregates and MediatR requests.
- **EF Core**: Migrations and DbContext in `Infrastructure/Data` and `Migrations`.
- **Angular**: App structure in `App/src/app`, tests in `App/e2e`.

## Developer Workflows
- **Build backend**: `dotnet restore` then `dotnet build` from solution root.
- **Run API**: `dotnet run` in `src/Commitments.Api`.
- **Database setup**: Edit connection string in `Api/appsettings.json`, run `dotnet ef database update` in `Infrastructure`.
- **Build front-end**: `npm install` then `npm start` or `ng serve` in `src/Commitments.App`.
- **Test backend**: `dotnet test` in test project directories.
- **Test front-end**: `npm test` for unit, `npm run e2e` for e2e/Playwright.

## Integration & Communication
- **API ↔ Core**: Controllers delegate to MediatR handlers and domain services.
- **Core ↔ Infrastructure**: Domain logic uses EF Core for persistence.
- **App ↔ API**: Angular front-end calls API endpoints for data.
- **Messaging/Telemetry**: UDP-based service bus abstraction in `Core/Services/Messaging`.
- **Security**: JWT authentication and helpers in `Core/Services/Security`.

## Project-Specific Notes
- Legacy code in `BuildingBlocks/` is mostly superseded by `Core/Services/*`.
- Dashboard/Card aggregates exist in Core but are partially removed from schema.
- TypeScript compilation for the front-end can be triggered via MSBuild when building the solution.

## Example: Adding a New Domain Concept
1. Create aggregate in `Core/AggregateModel/`.
2. Add MediatR handlers in `Core`.
3. Expose endpoints in `Api/Controllers/`.
4. Update DbContext/migrations in `Infrastructure`.
5. Add UI in `App/src/app` if needed.

---
For unclear or missing conventions, review `README.md` and key files in each project. Ask for feedback if any section is incomplete or ambiguous.
