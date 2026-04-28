---
id: bug-046
title: Consistency Trend chart x-axis renders raw ISO dates; design shows abbreviated "Apr 25" format
status: Fixed
---

# Bug 046 — Consistency Trend x-axis label format

**Status**: Fixed

## Fix

`consistency-trend-tile.component.ts` adds a Chart.js
`scales.x.ticks.callback` that renders each ISO date label
(`2026-04-25`) as `Apr 25` via
`toLocaleDateString('en-US', { month: 'short', day: 'numeric' })`,
matching the .pen xLabRow.

Two subtleties:
- `'T00:00:00'` suffix on the parsed date avoids the UTC
  interpretation `new Date('YYYY-MM-DD')` would otherwise produce.
- `Number.isNaN(date.getTime())` returns the raw label as a
  defensive fallback for malformed inputs.

Function-style declaration (`callback(value)`) is needed so that
`this.getLabelForValue` resolves against the Chart.js scale
binding; an arrow function would lose `this`.

`chartLabels()` continues to expose raw ISO strings for the rest
of the controller (tooltips, accessibility, future review-mode
queries) — formatting is purely a presentation concern in the
chart config.

Coverage:
- New TS-source spec asserts the x-axis tick callback contains
  `toLocaleDateString` with `'short'` and `'numeric'` options.
  Source-level regex avoids locale-dependent runtime tests.
- All 20 affected suites pass (114/114 — was 113/113 before).

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `Etp94` →
`xLabRow`) shows the x-axis labels as abbreviated month + day:
`Apr 13`, `Apr 16`, `Apr 19`, `Apr 22`, `Apr 25`.

The implementation in `consistency-trend.controller.ts` passes raw
ISO date strings:

```ts
readonly chartLabels = computed(() => this.trend()?.points.map(p => p.date) ?? []);
```

Each `p.date` is a `YYYY-MM-DD` string. Chart.js then renders those
as the x-axis tick labels — `2026-04-13`, `2026-04-16`, etc. — much
longer and less readable than the .pen.

The fix lives in the chart config: Chart.js exposes
`scales.x.ticks.callback` to format tick labels at draw time.
Formatting in the chart config (rather than upstream in the
controller) keeps `chartLabels()` as raw data the rest of the
controller can reason about (for tooltips, accessibility, etc.).

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect the x-axis labels — `2026-04-13` style ISO strings.
3. Compare to the .pen — short `Apr 13` style.

## Expected

```ts
x: {
  type: 'category',
  ticks: {
    color: '#666666',
    autoSkip: true,
    maxTicksLimit: 6,
    callback(value) {
      const label = this.getLabelForValue(value as number);
      const date = new Date(label + 'T00:00:00');
      if (Number.isNaN(date.getTime())) return label;
      return date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
    }
  },
  grid: { display: false }
}
```

The `T00:00:00` suffix avoids treating the YYYY-MM-DD as UTC
(JavaScript's `new Date('2026-04-25')` is interpreted as UTC; the
suffix forces local-time parsing so the displayed day matches the
ISO string).

## Verification

- Unit: source-level TS check on the component file — the x-axis
  config block contains a `callback` function that produces
  `toLocaleDateString` output with `'short'` and `'numeric'` opts.
- Visual: x-axis labels render as `Apr 13`, `Apr 16`, etc.
