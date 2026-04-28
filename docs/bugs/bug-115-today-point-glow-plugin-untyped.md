---
id: bug-115
title: bug-114's todayPointGlow plugin object is untyped
status: Open
---

# Bug 115 — todayPointGlow plugin typing

**Status**: Open

## Description

bug-114 added a `todayPointGlow` plugin inside
`ngAfterViewInit`. The plugin object is declared without a
type annotation:

```ts
const todayPointGlow = {
  id: 'todayPointGlow',
  afterDatasetDraw(chart: Chart<'line'>): void { … }
};
```

Chart.js exports a `Plugin<'line'>` type for plugin shape
checking. Without it, the plugin's hook signature isn't checked
against Chart.js's expected interface — a typo in `id` or a
mistaken hook name (e.g., `afterDataSetDraw`) would compile
silently.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -n "todayPointGlow" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

The const is untyped.

## Expected

The plugin is declared with the `Plugin<'line'>` type so the
`afterDatasetDraw` hook is checked against Chart.js's shape:

```ts
import { Chart, ChartConfiguration, Plugin } from 'chart.js';

const todayPointGlow: Plugin<'line'> = {
  id: 'todayPointGlow',
  afterDatasetDraw(chart) { … }
};
```

## Verification

- Unit (TS source): assert the plugin declaration includes
  `: Plugin<'line'>`.
- All existing consistency-trend specs continue to pass.
