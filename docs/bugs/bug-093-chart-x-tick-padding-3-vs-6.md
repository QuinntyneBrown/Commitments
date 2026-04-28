---
id: bug-093
title: Chart x-axis tick padding uses Chart.js default (3px), design specifies a 6px gap between plot and labels
status: Open
---

# Bug 093 — chart x-axis tick padding

**Status**: Open

## Description

`docs/tiles/consistency-trend/ui-design.pen`'s chart frame
`e5bRb` (parent of `ltPlot` and `xLabRow`) has `gap: 6`,
meaning a 6px gap between the bottom of the plot and the top
of the date labels.

`consistency-trend-tile.component.ts`'s X-axis ticks block has
no `padding` setting:

```ts
ticks: {
  color: …,
  font: …,
  autoSkip: true,
  maxTicksLimit: 6,
  callback(value) { … }
}
```

Chart.js v4 defaults `scales.x.ticks.padding` to 3px. The
labels therefore sit 3px below the plot, half what the design
specifies.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n "ticks\.padding\|padding\s*:" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

No match in the X-axis ticks block.

## Expected

`ticks.padding: 6` on the X axis, matching the design's
chart-frame gap.

## Verification

- Unit (TS source): assert the X-axis ticks block contains
  `padding: 6`.
- Existing consistency-trend specs continue to pass.
