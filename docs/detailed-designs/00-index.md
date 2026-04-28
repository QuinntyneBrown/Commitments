# Detailed Designs — Index

Each design covers the changes needed to satisfy **L1-012a / L2-031a — Mode-Invariant Tile Set**: every dashboard tile must support both Live and Review modes, must remain mounted across mode toggles, and must render the same catalog regardless of mode.

| #  | Feature                                                                                    | Status | Description                                                                                              |
| -- | ------------------------------------------------------------------------------------------ | ------ | -------------------------------------------------------------------------------------------------------- |
| 01 | [Mode-Invariant Tile Foundation](01-mode-invariant-tile-foundation/README.md)              | Implemented | Cross-cutting framework changes: TileContext, registry validation, snapshot-with-asOf endpoint contract. |
| 02 | [Daily Results — Dual-Mode](02-daily-results-dual-mode/README.md)                          | Implemented | Wire `DailyResultsTileComponent` to live & historical data; add `GET /commitment/daily-results?asOf`. |
| 03 | [Weekly Focus — Dual-Mode](03-weekly-focus-dual-mode/README.md)                            | Implemented | Build snapshot endpoint, controller, and live/historical bindings for `WeeklyFocusTileComponent`.   |
| 04 | [Monthly Progress — Dual-Mode](04-monthly-progress-dual-mode/README.md)                    | Implemented | Build snapshot endpoint, controller, and 4-bucket weekly aggregation with `asOf`.                   |
| 05 | [Outstanding Todos — Dual-Mode](05-outstanding-todos-dual-mode/README.md)                  | Accepted | Wire count endpoint with `asOf`, add delta-vs-today indicator in Review mode.                          |
| 06 | [Relations — Dual-Mode](06-relations-dual-mode/README.md)                                  | Draft  | Build category-distribution snapshot endpoint and historical projection.                                 |
| 07 | [Goal Metrics — Dual-Mode Merge](07-goal-metrics-dual-mode-merge/README.md)                | Draft  | Merge `LiveGoalMetricsTileComponent` + `ReviewGoalHistoryTileComponent` into a single dual-mode tile.    |

## Reading Order

1. **Start with 01.** It defines the framework contract every tile design depends on.
2. **02–07 are siblings.** Each follows the same shape: controller signal model, snapshot/historical endpoint, indicator swap, optimistic refresh on mode/`selectedReviewDate` change.

## Key Requirements Traced

- **L1-012a** — Mode-invariant tile set (tiles identical across Live/Review, only data differs).
- **L2-031a** — Mode-invariant tile registry (no per-mode filtering; `supportedModes` must be `['live','review']` or omitted).
- **L2-030** — Review mode keeps tile set stable; chrome-swap pattern is disallowed.
- **L2-027 / L2-028 / L2-029** — Review window selection, scrub-debounced fetch, server-side `asOf` clamping.
- **L2-045** — Historical responses cacheable when `asOf` older than 1 minute.
