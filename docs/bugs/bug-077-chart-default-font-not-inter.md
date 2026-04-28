---
id: bug-077
title: Consistency Trend chart x-axis labels render in Chart.js default font, not Inter as the design specifies
status: Fixed
---

# Bug 077 — Chart.js default font instead of Inter on consistency-trend axes

**Status**: Fixed

## Fix

Two lines added to `chart-js-line.adapter.ts` at module load,
right after the existing `Chart.register(...)` call:

```ts
Chart.defaults.font.family = 'Inter, Roboto, "Helvetica Neue", sans-serif';
Chart.defaults.font.size = 11;
```

Pinning the global Chart.js defaults propagates to every chart
the adapter creates, with no per-chart config changes. Axis tick
labels now render in Inter at 11px, matching every text node
specified in the .pen designs and the rest of the app's
typography. All 261 jest specs (including the new bug-077
guard) pass.

## Description

`docs/tiles/consistency-trend/ui-design.pen`'s chart text — the
x-axis date labels and the today-point label — all set
`fontFamily: "Inter"` at `fontSize: 11`. The rest of the
application also pins Inter via the global `styles.scss` and the
Material typography config.

`consistency-trend-tile.component.ts`'s Chart.js config sets
`ticks.font` only as a scriptable returning `{ weight }`:

```ts
font: (ctx) => ({ weight: isLastTick(ctx) ? 700 : 400 })
```

It never sets `family`, so Chart.js falls back to its built-in
default font stack (`'Helvetica Neue', 'Helvetica', 'Arial',
sans-serif`). The "Apr 13 / Apr 16 / Apr 19 / Apr 22 / Apr 25"
labels therefore render in Helvetica/Arial inside the canvas,
visually inconsistent with every Inter text node around them.

`chart-js-line.adapter.ts` registers the controllers but does not
touch `Chart.defaults.font`, so no global override exists either.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/data/chart-js-line.adapter.ts`
  (or the per-chart `ticks.font` scriptable in
  `consistency-trend-tile.component.ts`)

## Reproduction

1. Open the dashboard at <http://localhost:4200> and add the
   Consistency Trend tile.
2. Inspect the canvas's x-axis tick labels.
3. They render in Helvetica/Arial; the design specifies Inter.

## Expected

The Chart.js x-axis tick labels render in Inter at 11px, matching
the rest of the tile typography and the design.

The simplest fix is a single global default:

```ts
Chart.defaults.font.family = 'Inter, Roboto, "Helvetica Neue", sans-serif';
Chart.defaults.font.size = 11;
```

set once in `chart-js-line.adapter.ts` at module load time
(alongside the existing `Chart.register(...)` call). This propagates
to every chart the adapter creates without touching individual
chart configs.

## Verification

- Unit (TS source): assert `chart-js-line.adapter.ts` calls
  `Chart.defaults.font.family = '…Inter…'`.
- All existing consistency-trend specs continue to pass.
