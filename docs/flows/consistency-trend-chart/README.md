# Consistency Trend chart

## Summary

The `Consistency Trend` tile is the canonical line-graph view of goal completion percentage over time. It's a Chart.js `line` chart driven by `GET /api/v1.0/goal-progress/trend?goalId=...&windowDays=30`. The chart renders 30 daily points, fills under the line, and highlights the current day in Live mode or the scrubber-selected day in Review mode. It is mode-aware: the status pill switches between `LIVE` and `REVIEW`, and the highlighted point follows the selected date.

This flow has a smoke test (`chart-tile.spec.ts`) that asserts canvas presence + non-zero dimensions. The full historical-line behaviour is what this doc captures.

## Surface area

- Tile: `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.{ts,html}`
- Controller: `consistency-trend.controller.ts` — owns the `trend` signal and the synthesized demo path (active when `goalId === 'demo-goal'`).
- Chart adapter: `lib/data/chart-js-line.adapter.ts` (Chart.js wrapper)
- Service: `lib/data/goal-trend.service.ts` — calls `GET /api/v1.0/goal-progress/trend`
- Backend: `Modules/Commitments/Features/GoalProgress/GetGoalTrend.cs` — groups activities by `PerformedOn.Date`, emits 30-day point series.
- Mode integration: `TILE_CONTEXT.mode()`, `TILE_CONTEXT.selectedReviewDate()` — see [`dashboard-modes`](../dashboard-modes/README.md).

## Preconditions

- Authenticated, dashboard at `/`.
- For real data: a commitment exists for the active profile, and at least 1 activity exists in the last 30 days for that commitment's behaviour. For the dev path, `goalId='demo-goal'` synthesizes 30 days of believable data so no backend is needed.

## Steps

1. **Add a Consistency Trend tile.**
   - Default to Live mode → click the bottom-right FAB (`add-tile-fab`) to open the Add Tile dialog → click the `Consistency Trend` cell → click `ADD TILE` (`add-tile-dialog-confirm`).
   - **Assert:** the dialog closes; a `tile-shell` containing `Consistency Trend` appears; a `consistency-trend-canvas` element is visible inside it; the canvas's bounding box has non-zero width and height (already asserted by `ChartTilePage.expectCanvasHasDimensions()`).

2. **Status pill matches mode.**
   - In Live mode.
   - **Assert:** the tile's status pill text is `LIVE`.

3. **Line renders 30 points.**
   - Wait for the trend payload (or for the synthesized demo path).
   - **Assert:** the controller's `chartLabels()` length is 30; `chartDataset().data` has 30 numeric entries; each entry is between 0 and 100 inclusive.

4. **Last point is highlighted in Live mode.**
   - **Assert:** the `pointRadius` array has its largest value at the last index (live highlight); all other indices have the default radius (3 vs 7).

5. **Hover shows tooltip.**
   - Hover the canvas at increasing x-positions across the chart.
   - **Assert:** Chart.js tooltip becomes visible (via canvas content) — best asserted via Playwright `screenshot` diff or the tile's accessible "Last 30 days" supporting text updating, since the tooltip itself draws on the canvas. As a structural assertion, hovering should not throw and should not unmount the canvas.

6. **Switch to Review mode.**
   - Click the `Review` segment of `dashboard-mode-toggle`.
   - **Assert:** the tile's status pill switches from `LIVE` to `REVIEW`; `selectedReviewDate` becomes non-null; the highlighted index in the dataset moves to match the selected date.

7. **Scrub through history.**
   - Move the review scrubber to several positions.
   - **Assert:** each move triggers `controller.refresh()` (a re-fetch with new `asOf`); the chart's labels and dataset update; the highlighted index tracks the selected date.

8. **Real-time push (Live mode, real backend).**
   - With the chart open in Live mode, `POST` an activity for today via API for a commitment whose goal id is bound to the tile.
   - **Assert:** the chart's last data point's percentage increases without page reload (driven by SignalR `goalProgressUpdated`).

## Selectors

| Need | Selector |
| --- | --- |
| Tile shell | `getByTestId('tile-shell').filter({ hasText: 'Consistency Trend' })` |
| Canvas | `getByTestId('consistency-trend-canvas')` |
| Status pill | `getByTestId('tile-shell').filter({ hasText: 'Consistency Trend' }) .locator('cui-status-pill')` (verify final selector) |
| Mode toggle | `getByTestId('dashboard-mode-toggle')` |
| Scrubber | `getByTestId('dashboard-scrubber-slot')` |

## Edge cases

- Empty activity history → all 30 points have `percentage = 0`; chart still renders (flat line at 0).
- More than 30 days of activity → handler clamps `windowDays` to a max of 365; tile passes 30 by default.
- Future-dated `asOf` from scrubber → backend clamps to `UtcNow`.
- `goalId === 'demo-goal'` → controller short-circuits to `synthesizeDemoTrend()` (deterministic curve). This is the path exercised by the recorded demo video.
- Two `Consistency Trend` tiles open simultaneously → each has its own controller instance; both should render independently.
