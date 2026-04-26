# Detailed Designs — Index

These detailed designs apply a Live / Review tile pattern to goal tracking in Commitments. Each feature is **vertically sliced**, **radically simple**, and small enough for a single PR + one screenshot of working functionality.

| #  | Feature                                                       | Status | Description |
|----|---------------------------------------------------------------|--------|-------------|
| 01 | [Live Goal Metrics](01-live-goal-metrics/README.md)           | Draft  | Real-time tile that pushes achievement counts against a goal target via SignalR. |
| 02 | [Review Goal History](02-review-goal-history/README.md)       | Draft  | Scrub-through-history tile: pick a window, drag the slider, see the goal's count at any past instant. |

## How they relate

- Both tiles target a **single `goalId`** chosen from tile config — no multi-goal aggregation in either one.
- Both tiles consume the same `GoalProgressDto` shape and live next to each other in the same `GoalProgressController` on the API side.
- Live = **push** (SignalR `goalProgressUpdated`). Review = **pull** (`GET /api/goal-progress/at?asOf=…`).
- They are deliberately split into two tiles so each PR is small and visually demonstrable on its own.

## Implementation order

1. Feature 01 first — it introduces `GoalProgressDto`, the `GoalProgressController`, and the `GoalProgressService` that Feature 02 then extends.
2. Feature 02 reuses both, adding only the `at?asOf=` action, the scrub helpers, and the review tile.
