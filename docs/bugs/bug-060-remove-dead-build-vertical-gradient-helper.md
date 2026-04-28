---
id: bug-060
title: `buildVerticalGradient` helper in chart-js-line.adapter.ts is unused after bug-044's inlined gradient
status: Open
---

# Bug 060 — Remove dead `buildVerticalGradient` helper

**Status**: Open

## Description

`frontend/projects/commitments-dashboard-plugin/src/lib/data/chart-js-line.adapter.ts`
exports a `buildVerticalGradient(ctx, fromColor, height)` helper:

```ts
export function buildVerticalGradient(
  ctx: CanvasRenderingContext2D,
  fromColor: string,
  height: number = 240
): CanvasGradient {
  const gradient = ctx.createLinearGradient(0, 0, 0, height);
  gradient.addColorStop(0, fromColor);
  gradient.addColorStop(1, 'rgba(0,0,0,0)');
  return gradient;
}
```

Bug-044 inlined the gradient construction directly in the
`ConsistencyTrendController.chartDataset` Scriptable callback so it
could anchor the gradient to the live `chartArea` bounds — the
helper's fixed `height: 240` parameter wouldn't have produced the
right gradient stops at the chart's actual draw-time dimensions.
With that change, no consumer references `buildVerticalGradient`
anywhere in the workspace.

The bug-044 fix doc explicitly noted the helper was being left for
"future tiles that may want a one-call gradient utility", but no
such tile has materialized and the inline pattern stayed cleaner.
Leaving the helper invites either accidental re-introduction of
the older `height: 240` bug or junior-dev confusion ("which one do
I use?").

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/data/chart-js-line.adapter.ts`

## Reproduction

```bash
grep -r 'buildVerticalGradient' frontend/
# Only one match — the export itself in the adapter.
```

## Expected

Function (and its `Filler` import dependency, if no longer needed)
removed from `chart-js-line.adapter.ts`. The `Filler` plugin is
still required by the `fill: true` dataset option in
`consistency-trend.controller.ts`, so it stays in the
`Chart.register(...)` call.

## Verification

- Unit: source-level check — `chart-js-line.adapter.ts` no
  longer contains a `buildVerticalGradient` declaration.
- All 21 affected suites continue to pass.
