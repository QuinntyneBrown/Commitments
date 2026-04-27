# Relations Tile

**Tile ID:** `commitments.relations`
**Component:** `RelationsTileComponent`
**Status:** Static visual placeholder — hard-coded percentages, no service wiring

## Overview

Despite the name, the Relations tile does **not** display interpersonal relationships. It shows the **distribution of a user's commitments across life domains** — for example Health (42%), Work (33%), Personal (25%) — as a balance metric. "Relations" here means the relation between commitments and the categories they belong to.

The intent is to give the user a quick read on whether their commitments are skewed toward one area of life. It is a single-snapshot metric, not a trend.

## Visual Description

- **Shell title:** `Relations` with a `diversity_3` Material icon
- **Eyebrow:** `Balance`
- **Layout:** a CSS grid with two columns (label, percentage) and three rows (one per domain)
- **Type:** secondary-coloured labels, primary-coloured values; no chart, no progress bar, just numbers

Today the rendered values are literal: `Health 42%`, `Work 33%`, `Personal 25%`.

## Dashboard Integration

Registered via `provideCommitmentsDashboardPlugin()` in
[`provide-commitments-dashboard-plugin.ts`](../../../frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts).

Static `tileMetadata`:

| Field              | Value                                  |
| ------------------ | -------------------------------------- |
| `tileId`           | `commitments.relations`                |
| `displayName`      | `Relations`                            |
| `description`      | `Commitment distribution by relation.` |
| `icon`             | `diversity_3`                          |
| `defaultSize`      | `{ cols: 4, rows: 2 }`                 |
| `includeByDefault` | `true`                                 |

The tile uses the standard dashboard-grid lifecycle: `NgComponentOutlet` instantiation, `TileContext` injection, `tile-chrome` wrapping in edit mode (drag handle + remove button), gridster-driven drag and resize.

The tile is **purely informational** — there is no click target to drill into a domain, no per-row interaction.

## Real-Time vs. Historical Events

**Currently:** neither — the percentages are inline `style` attributes in the template.

**Backend infrastructure that exists:**

- `DashboardTileSnapshotEvent.RelationsSummaryUpdated` — defined in `backend/src/Commitments.Api/Realtime/DashboardTileSnapshotEvent.cs`
- `DashboardTileDataset.Relations = "relations"` — defined in `DashboardTileDataInvalidatedPayload.cs`

**Frontend infrastructure that exists:**

- `TileInvalidationService.invalidations$('relations')` would surface invalidations as soon as the backend starts publishing them.
- `TileContext.refresh$` is already plumbed through the dashboard grid for every tile.

What is missing for the tile to become live:

1. A `RelationsSummaryService` (frontend) that calls a `GET /api/v1.0/relations/summary` endpoint (or equivalent).
2. A backend handler that aggregates the user's commitments by category and returns the percentages.
3. A controller (signal-based, following `ConsistencyTrendController`) that owns the data, observes `TileContext.refresh$`, and re-fetches on invalidation.

## Service Dependencies

### Frontend (current)

| Dependency           | Role        | Location                                                    |
| -------------------- | ----------- | ----------------------------------------------------------- |
| `TileShellComponent` | Card chrome | `commitments-ui/src/lib/tile-shell/tile-shell.component.ts` |

No services injected. No `@Input()` properties. No constructor.

### Backend

**No endpoint exists yet.** Only the realtime event name and dataset constant are reserved.

## Data Contract

Currently none. Once wired:

```ts
interface RelationsSummaryViewModel {
  asOf: string;                    // YYYY-MM-DD
  relations: Array<{
    name: string;                  // e.g. "Health", "Work", "Personal"
    percentage: number;            // 0–100
    // optionally: count, color, sortOrder
  }>;                              // sums to ~100
}
```

The list is open-ended in concept (user-defined categories), but the current visual layout assumes 3 rows. The future implementation should either constrain to top-N or make the grid scroll/wrap.

## Key File Reference

| Concern              | Path |
| -------------------- | ---- |
| Component            | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.ts` |
| Template             | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.html` |
| Styles               | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.scss` |
| Plugin registration  | `frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts` |
| Realtime event reservation | `backend/src/Commitments.Api/Realtime/DashboardTileSnapshotEvent.cs` |
| Invalidation dataset reservation | `backend/src/Commitments.Api/Realtime/DashboardTileDataInvalidatedPayload.cs` |
| Tile shell           | `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.ts` |
| Reference controller pattern | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts` |
