# Detailed Designs — Index

These detailed designs apply a Live / Review tile pattern to goal tracking in Commitments and align the dashboard with `docs/ui-design.pen`. Each feature is **vertically sliced**, **radically simple**, and small enough for a single PR + one screenshot of working functionality.

| #  | Feature                                                                                            | Status   | Description |
|----|----------------------------------------------------------------------------------------------------|----------|-------------|
| 01 | [Live Goal Metrics](01-live-goal-metrics/README.md)                                                | Complete | Real-time tile that pushes achievement counts against a goal target via SignalR. |
| 02 | [Review Goal History](02-review-goal-history/README.md)                                            | Complete | Scrub-through-history tile: pick a window, drag the slider, see the goal's count at any past instant. |
| 03 | [UI Tokens And Shared Components](03-ui-tokens-and-shared-components/README.md)                    | Complete | Dark-theme tokens + reusable primitives in `commitments-ui` (mode toggle, status pill, icon button, tile shell, metric header, delta badge). |
| 04 | [Dashboard Mode Shell](04-dashboard-mode-shell/README.md)                                          | Complete | First-class `live`/`review` mode state in `dashboard-framework`, persisted, exposed through tile context. |
| 05 | [Gridster Layouts By Mode](05-gridster-layouts-by-mode/README.md)                                  | Complete | Separate live / review Gridster layouts with isolated persistence and mode-aware FAB visibility. |
| 06 | [Review Scrubber Timeline](06-review-scrubber-timeline/README.md)                                  | Complete | Dark scrubber bar with prev / play / next / today, publishing the selected review date into mode context. |
| 07 | [Live Real-Time Metric Tile](07-live-real-time-metric-tile/README.md)                              | Complete | Polished live tile matching `o0BgI`: large metric, delta vs yesterday, 14-day bar strip, SignalR updates. |
| 08 | [Chart.js Line Chart Tile](08-chartjs-line-chart-tile/README.md)                                   | Complete | Real-canvas Chart.js line chart (`9IpBQ`) with mode-aware highlighting and a reusable `ChartJsLineAdapter`. |
| 09 | [Review Goal History Tile Polish](09-review-goal-history-tile-polish/README.md)                    | Complete | Polished review tile (`nAfUX`) consuming the global scrubber date with selected-date badge and delta vs today. |
| 10 | [Mode-Aware Plugin Contracts](10-mode-aware-plugin-contracts/README.md)                            | Complete | Tile registration metadata for supported modes, per-mode component maps, and tile context with refresh hooks. |
| 11 | [Backend Trend And Snapshot Endpoints](11-backend-trend-and-snapshot-endpoints/README.md)          | Complete | `/current`, `/at`, `/trend` endpoints with bounded queries, indexes, and cache headers. |
| 12 | [E2E And Visual Acceptance](12-e2e-and-visual-acceptance/README.md)                                | Complete | Playwright POM coverage for mode switching, scrubber, FAB visibility, Chart.js canvas, and three viewports. |

## How they relate

- Features 01 and 02 are the original vertical slices that introduced `GoalProgressDto`, `GoalProgressController`, and `GoalProgressService`. Live = **push** (SignalR `goalProgressUpdated`). Review = **pull** (`GET /api/goal-progress/at?asOf=…`).
- Feature 03 (tokens + primitives) is the visual foundation every other feature consumes.
- Feature 04 introduces the mode signal; Features 05, 06, and 10 build the live/review shell, layouts, scrubber, and tile contracts on top of it.
- Features 07–09 are tile-level polish slices that produce the `o0BgI`, `9IpBQ`, and `nAfUX` designs.
- Feature 11 hardens the backend behind these tiles.
- Feature 12 closes the loop with end-to-end behavioral assertions.

## Implementation order

1. **03** — UI tokens and shared primitives (no tiles depend on un-tokenized styles after this).
2. **04** — Dashboard mode shell (introduces the mode signal everything else reads).
3. **05** — Mode-specific Gridster layouts (per-mode persistence isolation).
4. **06** — Review scrubber timeline (mode context now publishes a selected date).
5. **07** — Live metric tile visual polish (consumes 03 primitives; still uses Feature 01 endpoint).
6. **08** — Chart.js line chart tile (introduces the chart adapter; consumes Feature 11 trend endpoint).
7. **09** — Review tile visual polish (consumes the scrubber date and 03 primitives).
8. **10** — Mode-aware plugin contracts (formalises the manifest the prior tiles already reach for).
9. **11** — Backend trend endpoint (prerequisite for 08 to ship; can land in parallel after 08 starts).
10. **12** — E2E and visual acceptance coverage.

This order keeps each slice vertical enough to validate in the running app while avoiding a large dashboard rewrite before the shared mode and UI contracts are stable.
