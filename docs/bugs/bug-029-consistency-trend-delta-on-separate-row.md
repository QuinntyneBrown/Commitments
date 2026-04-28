---
id: bug-029
title: Consistency Trend — delta badge sits on its own row beneath the metric; design has them inline on the same row
status: Open
---

# Bug 029 — Consistency Trend metric and delta on separate rows

**Status**: Open

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
