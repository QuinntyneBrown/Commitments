---
id: bug-117
title: Consistency Trend chart's default X/Y axis border lines are not explicitly hidden
status: Fixed
---

# Bug 117 — chart axis borders

**Status**: Fixed

## Fix

Added `border: { display: false }` to both X and Y scale blocks.
Chart.js no longer draws the default scale border lines at the
chart's left and bottom edges — matching the design's `ltPlot`
which has no visible axis lines. 314/314 workspace tests green.

## Description

`docs/tiles/consistency-trend/ui-design.pen`'s `ltPlot` shows
five horizontal grid rectangles + the line/area chart + data
points + the `todayDot`. There are **no** visible X-axis or
Y-axis border lines.

Chart.js v4 draws each scale's border by default at the edge
of the chart area (separate from the grid lines, which the
component already disables via `grid.display: false` on x and
the muted-white grid color on y). The default border color
(`#e5e5e5` light grey) is faint against the dark surface but
still painted — visually a thin grey line at the chart's left
and bottom that the design doesn't include.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n "border" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

No border config — defaults apply.

## Expected

Both scales explicitly hide their border:

```ts
scales: {
  x: { …, border: { display: false } },
  y: { …, border: { display: false } }
}
```

## Verification

- Unit (TS source): assert both `x` and `y` scale blocks
  contain `border: { display: false }`.
- All existing consistency-trend specs continue to pass.
