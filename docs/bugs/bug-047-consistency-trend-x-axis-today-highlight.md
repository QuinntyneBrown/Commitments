---
id: bug-047
title: Consistency Trend chart x-axis renders all ticks in `#666666` / weight 400; design highlights the last "today" tick in `#42A5F5` / weight 700
status: Fixed
---

# Bug 047 — Consistency Trend x-axis "today" tick highlight

**Status**: Fixed

## Fix

`consistency-trend-tile.component.ts`:
- Adds `ACCENT_CHART` to the existing `@commitments/ui` import.
- Replaces flat `color: '#666666'` and missing `font` with
  Scriptable callbacks:
  ```ts
  color: (ctx) => isLastTick(ctx) ? ACCENT_CHART : '#666666',
  font:  (ctx) => ({ weight: isLastTick(ctx) ? 700 : 400 }),
  ```
- New `isLastTick(ctx)` module-level helper returns `true` when
  `ctx.index === ctx.scale.ticks.length - 1`. Pure, no `this`
  binding, easy to read.

`autoSkip: true` keeps the first and last ticks, so the rightmost
rendered tick remains the today data point — the highlight lands
on the correct label.

The two Scriptable callbacks each call `isLastTick` independently;
caching is unnecessary at tick-render frequency.

Coverage:
- New TS-source spec asserts the x-axis tick `color` is a
  Scriptable function referencing `ACCENT_CHART` and the `font`
  is a Scriptable function with weight `700`.
- All 20 affected suites pass (115/115 — was 114/114 before).

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `Etp94` →
`xLabRow`) renders five x-axis labels:

| Label    | Colour    | Weight |
| -------- | --------- | ------ |
| `Apr 13` | `#666666` | normal |
| `Apr 16` | `#666666` | normal |
| `Apr 19` | `#666666` | normal |
| `Apr 22` | `#666666` | normal |
| `Apr 25` | `#42A5F5` | **700** |

The last label — "today" — visually anchors the chart by sharing
the trend line's accent colour and a heavier weight. The other four
labels stay quiet in the muted text-muted colour.

The implementation in
`consistency-trend-tile.component.ts` configures the x-axis with a
flat colour:

```ts
ticks: {
  color: '#666666',
  …
}
```

There's no font weight override, so all ticks render at Chart.js'
default weight (400). The today tick gets no highlight.

Chart.js exposes Scriptable functions for `ticks.color` and
`ticks.font`, both of which receive a context including the tick
index and the scale. Returning the accent colour / weight 700 for
the highest-index tick mirrors the .pen.

The `ACCENT_CHART` token (`#42A5F5`) is already used in the
controller; the component file imports `MetricHeaderAccent` and
others from `@commitments/ui` but doesn't currently import
`ACCENT_CHART`.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect the x-axis labels — every one renders at `#666666` /
   weight 400.
3. Compare to the .pen — the rightmost label is `#42A5F5` weight
   700.

## Expected

```ts
import { ACCENT_CHART } from '@commitments/ui';
…

x: {
  …
  ticks: {
    color: (ctx) => isLastTick(ctx) ? ACCENT_CHART : '#666666',
    font: (ctx) => ({ weight: isLastTick(ctx) ? 700 : 400 }),
    autoSkip: true,
    maxTicksLimit: 6,
    callback(value) { /* bug-046 short label */ }
  },
  …
}
```

Where `isLastTick(ctx)` returns true when `ctx.index ===
ctx.scale.ticks.length - 1`. Chart.js' `autoSkip` typically retains
the first and last ticks, so the rightmost rendered tick is the
"today" data point.

## Verification

- Unit: source-level TS check on the component file — the x-axis
  tick `color` is a function referencing `ACCENT_CHART`; the tick
  `font` is a function and weight 700 appears in the file
  alongside `'700'` or `: 700`.
- Visual: rightmost x-axis label renders blue and bold.
