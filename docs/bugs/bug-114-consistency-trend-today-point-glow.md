---
id: bug-114
title: Consistency Trend today-point lacks the glow shadow the design specifies
status: Open
---

# Bug 114 — today-point glow

**Status**: Open

## Description

`docs/tiles/consistency-trend/ui-design.pen`'s `todayDot`
specifies a glow:

```
{
  effect: { blur: 8, color: "#42A5F5CC", shadowType: "outer", type: "shadow" },
  fill: "#42A5F5",
  stroke: { fill: "#121212", thickness: 3 },
  width: 14, height: 14
}
```

The glow (80%-opacity blue, blur 8) gives the highlighted point
a soft halo. The implementation renders just the 14px dot with
3px BG_APP stroke (bug-045 added the stroke); no shadow.

Chart.js doesn't support point shadows natively. The cleanest
fix is a tiny `afterDatasetDraw` plugin that uses
`ctx.shadowBlur` / `shadowColor` to draw a glow circle on top of
the highlighted point.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

1. Open the dashboard, add the Consistency Trend tile.
2. Compare the most-recent data point to the
   `ui-design.pen` screenshot.
3. The design shows a soft blue halo around the dot; the impl
   shows a plain dot.

## Expected

A small Chart.js plugin draws a glow behind the highlighted
data point using `ACCENT_CHART + 'CC'` (the design's
`#42A5F5CC`) at blur 8.

## Verification

- Unit (TS source): assert the chart `plugins` array includes
  a custom plugin that uses `ctx.shadowBlur` /
  `ctx.shadowColor` to draw the glow at the
  `controller.highlightedIndex()` point.
- All existing consistency-trend specs continue to pass.
