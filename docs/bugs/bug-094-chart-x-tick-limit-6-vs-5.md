---
id: bug-094
title: Consistency Trend chart x-axis maxTicksLimit is 6 but design has exactly 5 dates
status: Fixed
---

# Bug 094 — chart x-axis tick count

**Status**: Fixed

## Fix

Reduced X-axis `maxTicksLimit` from `6` to `5`, matching the
design's `xLabRow` of five dates (Apr 13/16/19/22/25). The
chart's X- and Y-axis now both render 5 ticks, aligning with
the design's overall visual rhythm. 291/291 workspace tests
green.

## Description

`docs/tiles/consistency-trend/ui-design.pen`'s `xLabRow` shows
exactly five date labels: Apr 13, Apr 16, Apr 19, Apr 22,
Apr 25. The `consistency-trend-tile.component.ts` X-axis ticks
block has:

```ts
ticks: { …, autoSkip: true, maxTicksLimit: 6, … }
```

`maxTicksLimit: 6` lets Chart.js auto-skip up to six labels —
matching live data could end up showing 6 dates when the
design's visual rhythm is 5.

After bug-091 the Y-axis is pinned to 5 grid lines; the X-axis
should match the same design count.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n "maxTicksLimit\s*:\s*6" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

Returns one match on the X-axis ticks block.

## Expected

`maxTicksLimit: 5` on the X axis.

## Verification

- Unit (TS source): assert the X-axis ticks block sets
  `maxTicksLimit: 5`.
- Existing consistency-trend specs continue to pass.
