# Outstanding Todos Tile

**Tile ID:** `commitments.outstanding-todos`
**Component:** `OutstandingTodosTileComponent`
**Status:** UI shell implemented; data is a hard-coded count, no service injection yet

## Overview

The Outstanding Todos tile shows the count of **to-do items currently waiting on the user**. To-dos are ad-hoc tasks tracked separately from recurring commitments — they live until they are completed (or removed). The tile's job is to surface a single, glanceable number on the dashboard so the user can see how much is outstanding before the next review.

Today the tile renders the literal value `4`. The backend already emits the events that *should* drive it; only the frontend wiring is missing.

## Visual Description

- **Shell title:** `Outstanding Todos` with a `checklist` Material icon
- **Category label:** `Tasks` (in metadata; not necessarily rendered)
- **Metric value:** very large warning-coloured number (~58px, weight 800)
- **Supporting copy:** `items need attention before the next review`

The metric value uses the warning colour rather than success because the metric is intentionally inverted — high values are bad.

## Dashboard Integration

Registered via `provideCommitmentsDashboardPlugin()` in
[`provide-commitments-dashboard-plugin.ts`](../../../frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts).

Static `tileMetadata`:

| Field              | Value                              |
| ------------------ | ---------------------------------- |
| `tileId`           | `commitments.outstanding-todos`    |
| `displayName`      | `Outstanding Todos`                |
| `icon`             | `checklist`                        |
| `category`         | `Tasks`                            |
| `defaultSize`      | `{ cols: 3, rows: 2 }`             |
| `defaultPosition`  | `{ x: 9, y: 0 }`                   |
| `includeByDefault` | `true`                             |

Standard plugin-tile lifecycle: framework instantiates via `NgComponentOutlet`, injects `TileContext`, wraps with `tile-chrome` in edit mode.

The tile itself is **read-only** — there is no per-item check-off UI here; users complete or remove to-dos elsewhere in the app. This tile is a counter, not a manager.

## Real-Time vs. Historical Events

**Currently:** static — no fetch, no subscriptions.

**Designed flow** (backend-ready, frontend-pending):

1. The backend tracks `ToDoChangedEvent { ToDoId, ProfileId, Kind: Created | Updated | Removed | Completed }` on the in-process event bus.
2. `DashboardTileInvalidationNotifier` listens for these events and publishes a `dashboardTileDataInvalidated` SignalR message tagged with the `outstandingTodos` dataset and the affected to-do IDs.
3. The frontend `TileInvalidationService` exposes `invalidations$('outstandingTodos')`.
4. A future `OutstandingTodosController` should subscribe to that stream (or to `TileContext.refresh$`), re-query the count endpoint, and update a signal bound by the template.

Because the count is the *only* thing the tile renders, the simplest implementation can ignore the IDs in the payload and just re-query unconditionally on any invalidation.

## Service Dependencies

### Frontend (current)

| Dependency           | Role        | Location                                                    |
| -------------------- | ----------- | ----------------------------------------------------------- |
| `TileShellComponent` | Card chrome | `commitments-ui/src/lib/tile-shell/tile-shell.component.ts` |

No data services injected today.

### Backend

**Event source:**

- `ToDoChangedEvent` — `backend/src/Commitments.Shared/IntegrationEvents.cs`
- `DashboardTileInvalidationNotifier` — `backend/src/Commitments.Api/Realtime/DashboardTileInvalidationNotifier.cs` (publishes for the `outstandingTodos` dataset)
- `DashboardTileDataset.OutstandingTodos = "outstandingTodos"` — defined in `DashboardTileDataInvalidatedPayload.cs`

**Snapshot endpoint:** none has been verified to exist yet. The expected pattern is `GET /api/v1.0/todos?status=outstanding` (or similar) returning a list — or simply a count endpoint. This needs to be confirmed or created when the tile is wired up.

## Data Contract

Currently none. Once wired:

```ts
interface OutstandingTodosViewModel {
  asOf: string;        // YYYY-MM-DD; today in live mode, selectedReviewDate in review
  count: number;       // number of to-dos in non-completed, non-removed state
}
```

A definition of "outstanding": created + not yet `Completed` + not `Removed`, scoped to the current `ProfileId` carried in the SignalR group.

## Key File Reference

| Concern              | Path |
| -------------------- | ---- |
| Component            | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.ts` |
| Template             | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.html` |
| Styles               | `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss` |
| Plugin registration  | `frontend/projects/commitments-dashboard-plugin/src/lib/provide-commitments-dashboard-plugin.ts` |
| Backend integration event | `backend/src/Commitments.Shared/IntegrationEvents.cs` (`ToDoChangedEvent`) |
| Realtime notifier    | `backend/src/Commitments.Api/Realtime/DashboardTileInvalidationNotifier.cs` |
| Realtime payload     | `backend/src/Commitments.Api/Realtime/DashboardTileDataInvalidatedPayload.cs` |
| Frontend invalidation bridge | `frontend/projects/commitments-app/src/app/dashboard-tiles/tile-invalidation.service.ts` |
| Tile shell           | `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.ts` |
