# Consistency Trend Tile

**Tile ID:** `commitments.consistency-trend`
**Component:** `ConsistencyTrendTileComponent`
**Status:** Fully implemented (live + review modes, real-time invalidation)

## Overview

The Consistency Trend tile visualises the **percentage of a goal's daily activity target completed each day** over a configurable rolling window (default 30 days). It is the dashboard's primary signal for *how consistently* a user is hitting their commitments — not whether they hit a single day, but the shape of the curve over time.

A "consistency" data point on a given day is computed as `completedActivities / dailyTarget`, where the daily target is conventionally **30 activities** for a behaviour. The tile renders the resulting series as a Chart.js line chart with a translucent fill underneath the curve, plus a current value, peak/low captions, and a delta badge comparing the current window to the prior 14-day average.

## Visual Description

- **Eyebrow:** "Goal completion rate"
- **Status pill:** `LIVE` (with pulse animation) or `REVIEW`
- **Headline metric:** the latest day's completion percentage (or the selected review date's percentage)
- **Caption:** peak and low percentages across the window
- **Delta badge:** signed change vs. the prior 14-day average, e.g. `+12% vs prior 14d`
- **Chart:** Chart.js line with accent-colored stroke and 33% opacity fill; Y-axis fixed at 0–100, X-axis = category labels per day; legend hidden, Y-grid only
- **Highlight:** in `live` mode the most recent point is enlarged (radius 7px vs. 3px). In `review` mode the highlight follows the dashboard's `selectedReviewDate`.

## Dashboard Integration

Registered via `provideCommitmentsDashboardPlugin()` in
[`frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts`](../../../frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts).

Static `tileMetadata` declared on the component class:

| Field              | Value                                  |
| ------------------ | -------------------------------------- |
| `tileId`           | `commitments.consistency-trend`        |
| `displayName`      | `Consistency Trend`                    |
| `defaultSize`      | `{ cols: 6, rows: 4 }`                 |
| `supportedModes`   | `['live', 'review']`                   |
| `includeByDefault` | `false` (user must add it explicitly)  |

The `dashboard-framework` discovers the tile through the `PLUGIN_TILES` multi-provider, instantiates it inside the gridster grid via `NgComponentOutlet`, and injects a `TileContext` providing:

- `mode: Signal<'live' | 'review'>`
- `selectedReviewDate: Signal<string | null>` (ISO `YYYY-MM-DD`)
- `refresh$: Observable<void>` (fires on framework-level invalidation)
- `isEditMode`, `isMaximized` signals
- `requestRefresh()`, `remove()`, `maximize()`, `restore()` methods

In edit mode the tile is wrapped by `tile-chrome` (drag handle + remove button) and is freely draggable / resizable through gridster.

## Inputs

| Input         | Type     | Default       | Purpose                           |
| ------------- | -------- | ------------- | --------------------------------- |
| `goalId`      | `string` | `'demo-goal'` | Which goal's trend to display     |
| `windowDays`  | `number` | `30`          | Rolling window size (clamped 1–365 server-side) |

When `goalId === 'demo-goal'` the controller synthesises data locally instead of calling the API, so the tile renders out of the box on a fresh install.

## Real-Time vs. Historical Events

This is one of the few tiles that fully implements the dashboard's hybrid pattern:

1. **Snapshot load on mount** — `ngOnInit` invokes `controller.load(goalId, windowDays)`, which calls `GoalTrendService.getTrend(...)` over HTTP. The response includes every day in the window, the per-day percentage, and pre-computed peak/low/current/delta.
2. **Reactive refresh on context change** — an `effect()` in the component watches `mode` and `selectedReviewDate`. Any change triggers `controller.refresh()`, which re-issues the trend query with the new `mode` and `asOf` parameters.
3. **Real-time invalidation via SignalR** — when an `ActivityRecordedEvent` fires server-side, `DashboardTileInvalidationNotifier` publishes a `dashboardTileDataInvalidated` payload tagged with the `goalTrend` dataset to the user's `profile:{profileId}` SignalR group. On the client, `TileInvalidationService` filters that stream and drives the framework's `refresh$` Observable for matching tiles, which in turn re-fetches the trend.

This is a **pull-on-invalidation** model: the server tells the client *something changed*, the client re-queries the snapshot endpoint. No raw activity data is pushed over the socket.

## Service Dependencies

### Frontend

| Dependency              | Role                                                                 | Location                                                          |
| ----------------------- | -------------------------------------------------------------------- | ----------------------------------------------------------------- |
| `GoalTrendService`      | HTTP fetch of trend snapshots                                        | `commitments-dashboard-plugin/src/lib/data/goal-trend.service.ts` |
| `ChartJsLineAdapter`    | Chart.js wrapper (attach / updateDataset / destroy)                  | `commitments-dashboard-plugin/src/lib/data/chart-js-line.adapter.ts` |
| `ConsistencyTrendController` | Owns signals for trend, percentages, chart labels, highlight idx | `…/tiles/consistency-trend/consistency-trend.controller.ts`        |
| `TILE_CONTEXT` (token)  | Optional — read mode, selectedReviewDate, refresh$                   | `dashboard-framework/src/lib/tile-registration/tile.model.ts`     |
| `TileShellComponent`    | Card chrome (title, eyebrow, icon, status)                           | `commitments-ui/src/lib/tile-shell/`                              |

State is held in **Angular signals** inside the controller; no NgRx is involved.

### Backend

| Endpoint                             | Method | Handler                                                                        |
| ------------------------------------ | ------ | ------------------------------------------------------------------------------ |
| `/api/v1.0/goal-progress/trend`      | GET    | `GetGoalTrendHandler` (MediatR) at `Modules/Commitments/Features/GoalProgress/GetGoalTrend.cs` |

The handler queries the `Activities` table filtered by `BehaviourId`, groups by date, computes per-day completion percentages, and derives `currentPercentage`, `peakPercentage`, `lowPercentage`, and the 14-day delta. Cache-control headers vary based on whether `asOf` is in the past (cacheable) or live (short TTL).

Real-time plumbing:

- `DashboardTileInvalidationNotifier` (`backend/src/Commitments.Api/Realtime/`) — hosted service that listens on the in-process event bus and publishes invalidations.
- `GoalProgressUpdatedRealtimeNotifier` — companion notifier for finer-grained goal updates.
- `CommitmentsHub` — SignalR hub; clients are joined to a `profile:{profileId}` group on connect.

## Data Contract

`GoalTrendDto` (returned by `GoalTrendService.getTrend`):

```ts
{
  goalId: string;            // UUID
  mode: 'live' | 'review';
  asOf: string;              // YYYY-MM-DD snapshot date
  windowDays: number;        // 1–365
  points: Array<{
    date: string;            // YYYY-MM-DD
    completed: number;
    target: number;          // 30 by convention
    percentage: number;      // 0–100
  }>;
  currentPercentage: number;
  peakPercentage: number;
  lowPercentage: number;
  deltaLabel: string;        // e.g. "+12% vs prior 14d"
}
```

## Key File Reference

| Concern              | Path |
| -------------------- | ---- |
| Component            | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts` |
| Controller           | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts` |
| Controller spec      | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.spec.ts` |
| Data service         | `frontend/projects/commitments-dashboard-plugin/src/lib/data/goal-trend.service.ts` |
| Chart adapter        | `frontend/projects/commitments-dashboard-plugin/src/lib/data/chart-js-line.adapter.ts` |
| Backend handler      | `backend/src/Modules/Commitments/Features/GoalProgress/GetGoalTrend.cs` |
| Invalidation notifier | `backend/src/Commitments.Api/Realtime/DashboardTileInvalidationNotifier.cs` |
| Frontend invalidation bridge | `frontend/projects/commitments-app/src/app/dashboard-tiles/tile-invalidation.service.ts` |
