---
id: bug-091
title: Consistency Trend Y-axis grid renders 6 lines, design has exactly 5
status: Fixed
---

# Bug 091 — chart Y-axis grid line count

**Status**: Fixed

## Fix

Added `maxTicksLimit: 5` to the Y-axis `ticks` block. Chart.js
now renders exactly 5 horizontal grid lines, matching the
design's g1–g5 rectangles. Y-axis labels stay hidden via
`display: false`; the tick count drives the grid line count.
288/288 workspace tests green.

## Description

`docs/tiles/consistency-trend/ui-design.pen`'s `ltPlot` defines
exactly five horizontal grid lines (rectangles `g1`–`g5` at
y=10, 50, 90, 130, 170 within a 180px-tall plot). The
implementation lets Chart.js auto-step the Y axis (0–100) with
no limit, which by default produces six grid lines at 0, 20, 40,
60, 80, 100.

The Y-axis tick *labels* are hidden (`ticks.display: false`) so
the user only sees grid lines, but the count itself is a
designed visual rhythm. Rendering one extra horizontal line
makes the chart feel busier than the design specifies.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n "maxTicksLimit\|ticks\.count" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

Returns one match — `maxTicksLimit: 6` on the X axis. The Y axis
has no equivalent.

## Expected

Y-axis ticks pinned to 5 via `maxTicksLimit: 5` (since labels are
hidden, the tick *count* is what controls the grid line count
through Chart.js).

## Verification

- Unit (TS source): assert the Y-axis `ticks` block sets
  `maxTicksLimit: 5`.
- Existing consistency-trend specs continue to pass.
