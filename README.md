# Commitments

Commitments is a small, modular sample application for managing personal commitments and tracking daily activity against them. It demonstrates a typical .NET + Angular stack with a clean separation between API, domain core, infrastructure, and front‑end.

## Solution Structure

- **Commitments.Api** – ASP.NET Core Web API hosting the HTTP endpoints.
- **Commitments.Core** – Domain model, application logic, and cross‑cutting services:
  - Aggregate roots like **Commitment**, **Activity**, **Behaviour**, **Frequency**, **Profile**.
  - Query/command handlers using MediatR.
  - Cross‑cutting services in `Services/*` for **messaging**, **security**, **validation**, **kernel utilities**, and **telemetry**.
- **Commitments.Infrastructure** – EF Core DbContext, migrations, and data access wiring.
- **Commitments.App** – Angular/TypeScript front‑end for interacting with the API.
- **BuildingBlocks/** – Legacy/shared building‑block projects kept for compatibility (many of their concerns have been moved into `Commitments.Core.Services.*`).

## Key Concepts

- **Commitments & Activities** – A commitment defines a target behaviour at a given frequency; activities represent what was actually done.
- **Frequencies** – Model how often something should happen (e.g., per day) and link to commitments via `CommitmentFrequency`.
- **Profiles** – Represent the person for whom commitments and activities are tracked.
- **Dashboards & Cards** – Provide a configurable view over commitments and related data (currently partially removed from the schema but aggregates remain in Core).
- **Messaging & Telemetry** – Background messaging and telemetry producers built on top of a UDP‑based service bus abstraction.
- **Security** – JWT authentication, token generation, and supporting helpers in `Commitments.Core.Services.Security`.

## Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js and npm (for the Angular front‑end)
- SQL Server instance (localdb or full SQL Server) for EF Core

### Backend: API & Core

1. Restore and build the backend projects:
	- From the `Commitments` root: `dotnet restore` then `dotnet build`.
2. Apply or update the database:
	- Ensure the connection string in `src/Commitments.Api/appsettings.json` (and/or `Commitments.Infrastructure` configuration) points at your SQL Server.
	- Run EF Core migrations from `Commitments.Infrastructure` (e.g., `dotnet ef database update` from that project).
3. Run the API:
	- From `src/Commitments.Api`: `dotnet run`.

### Front‑end: Angular App

1. Navigate to `src/Commitments.App`.
2. Install dependencies: `npm install`.
3. Run the dev server: `npm start` or `ng serve` (depending on the Angular CLI setup).
4. Open the browser at the URL reported by the dev server (commonly `http://localhost:4200`).

> Note: The `Commitments.App` project is also wired into the solution as a .NET project shell so that building the solution can drive TypeScript compilation via MSBuild.

## Testing

- **Backend tests** – If present, run from the test project directories with `dotnet test`.
- **Front‑end tests** – From `src/Commitments.App`:
  - Unit tests: `npm test`.
  - End‑to‑end / Playwright tests: `npm run e2e` (or the configured script in `package.json`).

## Notes on Architecture

- The **Core** project is the primary dependency hub: other services depend on it for shared aggregates and cross‑cutting concerns.
- Building‑block libraries under `src/BuildingBlocks/*` are being gradually folded into `Commitments.Core.Services.*`; new code should preferentially target the Core services.
- EF Core is used with soft‑delete filters on most aggregates (using an `IsDeleted` flag) and shadow properties for audit information.

This repository is primarily intended as a learning and experimentation space for modular .NET backends and Angular front‑ends, rather than a production‑ready product.
