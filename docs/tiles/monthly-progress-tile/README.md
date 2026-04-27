# Monthly Progress Tile

**Tile ID:** `commitments.monthly-progress`
**Component:** `MonthlyProgressTileComponent`
**Status:** Static placeholder — no services, no API calls, hard-coded bar heights

## Overview

The Monthly Progress tile is intended to show a **30-day completion trend broken down by week**, giving the user a coarser-grained view than Daily Results but a more recent view than Consistency Trend's full window. It renders as four vertical bars (one per week of the month), each scaled to that week's completion rate.

In its current form the tile is a **visual stub**: the four bars are inline-styled at 35%, 72%, 58%, and 86%. There is no controller, no data service, and no API endpoint. The backend reserves event names and dataset constants for it (`MonthlyProgressUpdated`, `DashboardTileDataset.MonthlyProgress`), but there is currently no query handler or controller route to populate it.

## Visual Description

- **Shell title:** `Monthly Progress` with a `calendar_month` Material icon
- **Eyebrow:** `30 days`
- **Bars:** four equal-width columns, bottom-aligned (`align-items: end`), filled with a blue gradient (`#2f6db3` → `#8bb8e8`)
- **No metric headline, no delta badge, no status pill** — purely the bar chart inside the standard `TileShellComponent`

## Dashboard Integration

Registered via `provideCommitmentsDashboardPlugin()` in
[`provide-commitments-dashboard-plugin.ts`](../../../frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts).

Static `tileMetadata`:

| Field              | Value                          |
| ------------------ | ------------------------------ |
| `tileId`           | `commitments.monthly-progress` |
| `displayName`      | `Monthly Progress`             |
| `icon`             | `calendar_month`               |
| `category`         | `Commitments`                  |
| `defaultSize`      | `{ cols: 3, rows: 2 }`         |
| `defaultPosition`  | `{ x: 6, y: 0 }`               |
| `includeByDefault` | `true`                         |

Behaviour inside the dashboard grid is identical to every other plugin tile: `NgComponentOutlet` instantiation, `TileContext` injection, drag/resize/remove via the `tile-chrome` wrapper in edit mode.

## Real-Time vs. Historical Events

**Neither — currently.** The component does not subscribe to anything and does not fetch anything. The bars are literal style attributes in the template.

Backend infrastructure that *could* drive it once implemented:

- `DashboardTileSnapshotEvent.MonthlyProgressUpdated` — defined in `backend/src/Commitments.Api/Realtime/DashboardTileSnapshotEvent.cs` for snapshot push events.
- `DashboardTileDataset.MonthlyProgress` — defined in `DashboardTileDataInvalidatedPayload.cs` for the existing `dashboardTileDataInvalidated` SignalR message.

Wiring the tile to live data would follow the same pattern as Consistency Trend:

1. Add a `MonthlyProgressService` that calls a (yet to be created) `GET /api/v1.0/…/monthly-progress` endpoint.
2. Add a `MonthlyProgressController` that holds signals for the four bucket percentages.
3. Inject `TILE_CONTEXT`, observe `mode` / `selectedReviewDate`, subscribe to `refresh$` for invalidation-driven re-fetch.

## Service Dependencies

### Frontend (current)

| Dependency           | Role        | Location                                                    |
| -------------------- | ----------- | ----------------------------------------------------------- |
| `TileShellComponent` | Card chrome | `commitments-ui/src/lib/tile-shell/tile-shell.component.ts` |

No services injected. No `@Input()` properties. No constructor.

### Backend

**No endpoint exists today.** Only the realtime constants are reserved.

The future endpoint would presumably aggregate `Activity` rows over the last 30 days, bucket them into 7-day weeks, and compute the per-week completion percentage against the user's "per day" commitment targets.

## Data Contract

Currently none. Once wired, the natural shape is:

```ts
interface MonthlyProgressViewModel {
  asOf: string;                    // YYYY-MM-DD anchor (today in live, selectedReviewDate in review)
  windowDays: 30;
  buckets: Array<{
    weekStart: string;             // YYYY-MM-DD
    weekEnd: string;               // YYYY-MM-DD
    completed: number;             // sum of activities in the week
    target: number;                // sum of daily targets across the week
    percentage: number;            // 0–100
  }>;                              // length === 4 to match the four bars
}
```

## Key File Reference

| Concern              | Path |
| -------------------- | ---- |
| Component            | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.ts` |
| Template             | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.html` |
| Styles               | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss` |
| Plugin registration  | `frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts` |
| Realtime event reservation | `backend/src/Commitments.Api/Realtime/DashboardTileSnapshotEvent.cs` |
| Invalidation dataset reservation | `backend/src/Commitments.Api/Realtime/DashboardTileDataInvalidatedPayload.cs` |
| Tile shell           | `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.ts` |
| Reference controller pattern | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts` |
