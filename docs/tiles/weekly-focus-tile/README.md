# Weekly Focus Tile

**Tile ID:** `commitments.weekly-focus`
**Component:** `WeeklyFocusTileComponent`
**Status:** Static placeholder — hard-coded list, no service wiring

## Overview

The Weekly Focus tile lists the user's chosen **focus areas for the current calendar week**, each paired with a small supporting metric. It is intended to answer *"what am I trying to concentrate on this week?"* at a glance — not a progress chart, not a streak counter, but a curated list of intentions plus their planned engagement counts.

Currently the tile shows three hard-coded items (e.g. `Move — 5 sessions planned`, `Read — 3 sessions planned`, `Reflect — 2 notes pending`). It is a visual stub waiting on a data layer.

## Visual Description

- **Shell title:** `Weekly Focus` with a `date_range` Material icon
- **Eyebrow:** `This week`
- **Body:** an unordered list (`ul.focus-list`) of focus items; each `li` displays a focus name (primary text, bold) above a supporting metric (secondary text, smaller)
- **Layout:** CSS grid with 10px gaps, items separated by a divider border

## Dashboard Integration

Registered via `provideCommitmentsDashboardPlugin()` in
[`provide-commitments-dashboard-plugin.ts`](../../../frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts).

Static `tileMetadata`:

| Field              | Value                                  |
| ------------------ | -------------------------------------- |
| `tileId`           | `commitments.weekly-focus`             |
| `displayName`      | `Weekly Focus`                         |
| `description`      | `Current weekly focus areas.`          |
| `icon`             | `date_range`                           |
| `defaultSize`      | `{ cols: 3, rows: 2 }`                 |
| `defaultPosition`  | `{ x: 3, y: 0 }`                       |
| `includeByDefault` | `true`                                 |

Standard plugin-tile lifecycle: `NgComponentOutlet` instantiation, `TileContext` injection, `tile-chrome` wrapping in edit mode (drag handle + remove button), gridster-driven drag and resize.

The tile does not currently expose any edit affordances of its own — adding/removing focus areas is expected to happen elsewhere in the app, with this tile reading the result.

## Real-Time vs. Historical Events

**Currently:** static — no fetch, no subscriptions, no `TileContext` injection.

**Backend infrastructure that exists:**

- `DashboardTileSnapshotEvent.WeeklyFocusUpdated` — defined in `backend/src/Commitments.Api/Realtime/DashboardTileSnapshotEvent.cs`
- `DashboardTileDataset.WeeklyFocus = "weeklyFocus"` — defined in `DashboardTileDataInvalidatedPayload.cs`

The `DashboardTileInvalidationNotifier` already publishes invalidations on activity, commitment, and frequency changes; the `weeklyFocus` dataset is part of that fan-out.

**Wiring path** (mirrors `ConsistencyTrendTileComponent`):

1. Inject `TILE_CONTEXT`.
2. On mount, fetch focus areas via a `WeeklyFocusService` (to be created).
3. Subscribe to `TileContext.refresh$` (or `TileInvalidationService.invalidations$('weeklyFocus')`) and re-fetch on each emission.
4. In `review` mode, send `TileContext.selectedReviewDate` to fetch the focus areas as they stood for that historical week.

## Service Dependencies

### Frontend (current)

| Dependency           | Role        | Location                                                    |
| -------------------- | ----------- | ----------------------------------------------------------- |
| `TileShellComponent` | Card chrome | `commitments-ui/src/lib/tile-shell/tile-shell.component.ts` |

No services injected. No constructor.

### Backend

**No endpoint exists yet.** Only the realtime event name and dataset constant are reserved.

The natural backing model would link a `WeeklyFocus` row (a user-curated focus per week) to its target metric (sessions planned, notes pending, etc.). The tile's hard-coded copy hints at heterogeneous metric kinds across rows.

## Data Contract

Currently none. Once wired:

```ts
interface WeeklyFocusViewModel {
  weekStart: string;               // YYYY-MM-DD; locale-dependent week start
  weekEnd: string;                 // YYYY-MM-DD
  focusAreas: Array<{
    name: string;                  // e.g. "Move", "Read", "Reflect"
    supportingMetric: string;      // pre-formatted, e.g. "5 sessions planned"
    // optionally: progress, type, link
  }>;
}
```

In `live` mode the week is the calendar week containing today. In `review` mode it is the calendar week containing `selectedReviewDate`.

## Key File Reference

| Concern              | Path |
| -------------------- | ---- |
| Component            | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.ts` |
| Template             | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.html` |
| Styles               | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss` |
| Plugin registration  | `frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts` |
| Realtime event reservation | `backend/src/Commitments.Api/Realtime/DashboardTileSnapshotEvent.cs` |
| Invalidation dataset reservation | `backend/src/Commitments.Api/Realtime/DashboardTileDataInvalidatedPayload.cs` |
| Realtime notifier    | `backend/src/Commitments.Api/Realtime/DashboardTileInvalidationNotifier.cs` |
| Tile shell           | `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.ts` |
| Reference controller pattern | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts` |
