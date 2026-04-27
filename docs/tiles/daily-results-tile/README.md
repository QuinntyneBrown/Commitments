# Daily Results Tile

**Tile ID:** `commitments.daily-results`
**Component:** `DailyResultsTileComponent`
**Status:** UI shell implemented; data binding is currently a static placeholder

## Overview

The Daily Results tile is the dashboard's at-a-glance answer to *"how am I doing today?"*. It shows a count of completed daily commitments versus the user's total daily commitments — e.g. **`7 / 9 commitments completed`** — alongside a progress bar visualising the same ratio.

A "result" in this app means a logged completion of a commitment whose frequency is *per day*. The tile is intentionally a snapshot of **today only** — it is not a trend, not a chart, just the headline number.

## Visual Description

- **Shell title:** `Daily Results` with a `today` Material icon
- **Eyebrow:** `Today`
- **Status pill:** `Live`
- **Metric value:** large success-coloured numerator/denominator (e.g. `7 / 9`), ~42px / weight 800
- **Metric label:** `commitments completed` in secondary text
- **Progress bar:** 8px, divider-coloured track, success-coloured fill at the corresponding percentage; carries `role="progressbar"` with `aria-valuenow` / `aria-valuemax` set from the actual numerator and denominator

## Dashboard Integration

Registered alongside the other plugin tiles in
[`provide-commitments-dashboard-plugin.ts`](../../../frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts).

Static `tileMetadata`:

| Field              | Value                       |
| ------------------ | --------------------------- |
| `tileId`           | `commitments.daily-results` |
| `displayName`      | `Daily Results`             |
| `icon`             | `today`                     |
| `category`         | `Commitments`               |
| `defaultSize`      | `{ cols: 3, rows: 2 }`      |
| `defaultPosition`  | `{ x: 0, y: 0 }`            |
| `includeByDefault` | `true`                      |

The dashboard-framework instantiates the component via `NgComponentOutlet` and provides a `TileContext`. In edit mode it gains the standard `tile-chrome` (drag handle, remove button) and is draggable / resizable through gridster.

## Real-Time vs. Historical Events

> **Implementation gap.** The component as currently written has **no constructor**, no service injections, and no template bindings — the `7 / 9` and `78%` values in the template are hard-coded. The tile renders the `Live` pill but does not yet listen to anything.

The infrastructure is, however, in place to wire it up:

- **Backend** — `DashboardTileInvalidationNotifier` publishes a `dashboardTileDataInvalidated` payload tagged with the `dailyResults` dataset whenever an `ActivityRecordedEvent` (or related commitment change) is observed on the in-process event bus. The payload carries `reason`, `from`/`to` date range, and affected commitment IDs.
- **Frontend bridge** — `TileInvalidationService.invalidations$('dailyResults')` exposes a filtered Observable on top of the SignalR `dashboardTileDataInvalidated` stream.
- **Snapshot endpoint** — `GET /api/v1.0/commitment/daily` already returns the user's per-day commitments (see `Modules/Commitments/Features/Commitment/Queries/GetDailyCommitments.cs`), so a controller can be wired without backend changes.

Once a controller is added, the expected shape is:

1. On mount, fetch via `commitment/daily` and project to `{ completed, total }`.
2. Subscribe to `TileContext.refresh$` (or the `dailyResults` invalidation stream directly) and re-fetch.
3. In `review` mode, send `selectedReviewDate` as the `asOf` parameter so the tile reflects the historical day instead of today.

Until that work lands, **the tile reflects neither real-time nor historical state — it is a visual stub.**

## Service Dependencies

### Frontend (current)

| Dependency           | Role                  | Location                                                      |
| -------------------- | --------------------- | ------------------------------------------------------------- |
| `TileShellComponent` | Card chrome           | `commitments-ui/src/lib/tile-shell/tile-shell.component.ts`   |

No data services are currently injected.

### Frontend (expected once data-bound)

| Dependency               | Role                                                            |
| ------------------------ | --------------------------------------------------------------- |
| `HttpClient`             | Fetch `commitment/daily`                                        |
| `TILE_CONTEXT` (token)   | Read `mode`, `selectedReviewDate`, observe `refresh$`           |
| `TileInvalidationService`| Subscribe to `invalidations$('dailyResults')`                   |

The `ConsistencyTrendController` is a good template for a `DailyResultsController` — same signal-based controller pattern.

### Backend

| Endpoint                       | Method | Handler                                                                            |
| ------------------------------ | ------ | ---------------------------------------------------------------------------------- |
| `/api/v1.0/commitment/daily`   | GET    | `GetDailyCommitmentsHandler` at `Modules/Commitments/Features/Commitment/Queries/GetDailyCommitments.cs` |

Handler returns `GetDailyCommitmentsResponse { Commitments: IEnumerable<CommitmentDto> }`, eagerly loading each commitment's `Behaviour` and `Frequency` for "per day" frequency entries. The numerator (completed-today count) is computed by counting matching `Activity` rows for the current date.

Realtime payloads:
- `DashboardTileSnapshotEvent.DailyResultsUpdated`
- `DashboardTileDataset.DailyResults` (`"dailyResults"`)

## Data Contract

Expected once wired:

```ts
interface DailyResultsViewModel {
  date: string;              // YYYY-MM-DD; today in live mode, selectedReviewDate in review mode
  completed: number;         // count of commitments with at least one matching activity logged for `date`
  total: number;             // count of "per day" commitments
  // derived: percentage = total > 0 ? completed / total * 100 : 0
}
```

## Key File Reference

| Concern              | Path |
| -------------------- | ---- |
| Component            | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.ts` |
| Template             | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.html` |
| Styles               | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss` |
| Plugin registration  | `frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts` |
| Backend query        | `backend/src/Modules/Commitments/Features/Commitment/Queries/GetDailyCommitments.cs` |
| Backend controller   | `backend/src/Modules/Commitments/Controllers/CommitmentController.cs` |
| Realtime payload     | `backend/src/Commitments.Api/Realtime/DashboardTileDataInvalidatedPayload.cs` |
| Frontend invalidation bridge | `frontend/projects/commitments-app/src/app/dashboard-tiles/tile-invalidation.service.ts` |
| Tile shell           | `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.ts` |
| Reference controller pattern | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts` |
