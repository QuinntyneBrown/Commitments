---
id: bug-044
title: Consistency Trend chart area renders flat 20% blue; design specifies a vertical gradient `#42A5F566` → `#42A5F500`
status: Open
---

# Bug 044 — Consistency Trend chart area uses flat fill instead of vertical gradient

**Status**: Open

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `e5bRb` →
`ltPlot` → `areaFill`) draws the area beneath the trend line as a
**vertical gradient**:

```
gradient:
  rotation: 180
  colors:
    - #42A5F566 at position 0   (40% blue, top)
    - #42A5F500 at position 1   (0%, bottom — fully transparent)
```

The implementation in `consistency-trend.controller.ts` configures
the dataset with a flat colour:

```ts
backgroundColor: ACCENT_CHART + '33',   // 20% blue, flat
```

Visually that's the **average** of the design's gradient, so the
area feels heavier at the bottom than the design shows and the
chart loses its "fade-into-tile" reading.

Chart.js dataset properties accept a Scriptable function that
receives the chart context, so we can build a `CanvasGradient`
against the chart area at draw time without leaking canvas wiring
into the tile component.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect the area beneath the trend line — it's a uniform 20%
   blue from y=0 to y=100.
3. Compare to the .pen — the area is denser at the line's
   position and fades to fully transparent at the bottom.

## Expected

```ts
backgroundColor: (ctx) => {
  const chart = ctx.chart;
  const area = chart?.chartArea;
  if (!area) return ACCENT_CHART + '00';
  const gradient = chart.ctx.createLinearGradient(0, area.top, 0, area.bottom);
  gradient.addColorStop(0, ACCENT_CHART + '66'); // 40% top
  gradient.addColorStop(1, ACCENT_CHART + '00'); // 0% bottom
  return gradient;
}
```

## Verification

- Unit: source-level TS check on
  `consistency-trend.controller.ts` — `backgroundColor` is a
  function that calls `createLinearGradient` (i.e. no longer a
  static string).
- Visual: the area beneath the trend line fades from ~40% blue at
  the top to fully transparent at the bottom.
