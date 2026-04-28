---
id: bug-029
title: Consistency Trend — delta badge sits on its own row beneath the metric; design has them inline on the same row
status: Fixed
---

# Bug 029 — Consistency Trend metric and delta on separate rows

**Status**: Fixed

## Fix

`consistency-trend-tile.component.html` moves `<cui-metric-header>`
inside the existing `<div class="trend-row">` so it lives next to
`<cui-delta-badge>`. `consistency-trend-tile.component.scss` upgrades
`.trend-row` from a plain `margin-top: 6px;` block to:

```scss
.trend-row {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  gap: 16px;
  margin-top: 6px;
}
```

Metric headline now stays on the left while the delta badge floats
to the right edge with baselines aligned, matching the .pen
ctMetric frame.

Out of scope (tracked separately if filed):
- metric-header internal value font 42px vs design 48px;
- metric-header doesn't yet stack a `today` sublabel above the
  Peak/Low caption.

Coverage:
- Two new specs in
  `consistency-trend-tile.component.spec.ts` cover both the template
  co-location and the flex-row CSS commitments.
- All 20 affected suites pass (82/82 — was 80/80 before).

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `ctMetric`) is a
**single horizontal row** with `alignItems: end` containing:

- the metric headline (`78%`) and its right-side caption column
- a spacer (`fill_container`)
- the delta pill (`+12% vs prior 14d`) at the trailing edge

The implementation in
`consistency-trend-tile.component.html` instead projects three
separate vertical sections inside the tile shell body:

```html
<cui-metric-header …></cui-metric-header>
<div class="trend-row">
  <cui-delta-badge …></cui-delta-badge>
</div>
<div class="plot">
  <canvas …></canvas>
</div>
```

`.trend-row` is just `margin-top: 6px;` — it does not hold the
metric header. The metric and delta therefore stack on consecutive
rows, with the delta pinned to the left rather than the right edge
the design specifies.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss`

## Reproduction

1. Render the Consistency Trend tile.
2. Observe `78%` on its own line, then `+12% vs prior 14d` on the next
   line, then the chart.
3. Compare to the .pen — the metric and the delta share row 2 with
   the delta tucked against the right edge.

## Expected

The metric header and delta badge share a single flex row inside
`.trend-row`, with `align-items: flex-end; justify-content: space-between`
so that:

- the metric headline keeps its intrinsic size on the left,
- the delta badge floats to the right.

## Verification

- Unit: source-level template + SCSS spec — `<cui-metric-header>` and
  `<cui-delta-badge>` are both projected inside the same
  `class="trend-row"` container; `.trend-row` declares `display: flex`
  with `align-items: flex-end` and `justify-content: space-between`.
- Visual: screenshot the tile vs the .pen.
