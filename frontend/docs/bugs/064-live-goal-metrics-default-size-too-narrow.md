# 064 — Live Goal Metrics tile renders too narrow at default size; bar chart is cropped

## Status

OPEN — surfaced via visual capture
(`frontend/docs/bugs/screenshots/live-goal-metrics-tile-1280.png`) compared
against design frame `o0BgI` (`Real-Time-Metric-Tile`).

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

- [ ] Visual screenshot captured pre-fix.
- [ ] defaultSize updated.
- [ ] Re-screenshot post-fix shows full bar chart.
