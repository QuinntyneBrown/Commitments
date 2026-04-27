# 064 — Live Goal Metrics tile renders too narrow at default size; bar chart is cropped

## Status

FIXED — bars container went from 159px → 411px wide; full lg-desktop
e2e 57/57.

## Symptom

When the user adds Live Goal Metrics from the Add Tile dialog at lg-desktop:

```
tile bounding box: 193 × 203
gridster cell:     150 (rows=3 in a 12×8 fit grid)
.bars container:   120 tall
```

The tile content (203px) overflows the cell (150px) and the bar chart
falls past the cell's `overflow: hidden`. The design intends a wide
chart-style tile (`o0BgI` ratio ≈ 470 × 240).

## Root cause

`live-goal-metrics-tile.component.ts:30`:

```ts
defaultSize: { cols: 3, rows: 3 },
```

`cols: 3` puts the tile in 1/4 of the grid width, leaving no room for
the 14-day bars to be readable. The design's `Real-Time-Metric-Tile`
is approximately 6 cols wide (half the dashboard).

## Fix

Bump `defaultSize` to `{ cols: 6, rows: 3 }` so the chart bars get the
horizontal space they need and the tile aspect matches the design.
This only affects newly-added tile instances; persisted layouts in
`localStorage` keep their existing sizes.

## Resolution

- [x] Visual screenshot captured pre-fix (203px-tall content in 150px cell;
      bars 159px wide).
- [x] Two changes:
      1. `.tile-shell { width: 100% }` — surfaced while auditing; the mat-card
         was content-sized, so even a wide cell didn't widen the body.
      2. live-goal-metrics-tile `defaultSize: { cols: 6, rows: 3 }` — matches
         the chart-tile aspect of design `o0BgI`.
- [x] Bars container now 411px wide (was 159); 57/57 e2e on lg-desktop.
