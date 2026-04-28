---
id: bug-118
title: consistency-trend tick color/font Scriptables duplicate the highlightedIndex check
status: Fixed
---

# Bug 118 — DRY tick highlight check

**Status**: Fixed

## Fix

Extracted a local `isHighlighted(ctx)` helper next to the
TODAY_GLOW_* constants and the `todayPointGlow` plugin:

```ts
const isHighlighted = (ctx: { index?: number }) =>
  ctx.index === controller.highlightedIndex();
```

The two tick Scriptables now read `isHighlighted(ctx)` instead
of inlining the predicate. Refactor only — visual output is
identical.

The bug-092 spec was loosened so it accepts either the inline
`highlightedIndex()` form or the new helper form.

315/315 workspace tests green.

## Description

After bug-092 replaced `isLastTick` with the
`highlightedIndex`-based check, the tick `color` and `font`
Scriptables both contain the identical predicate:

```ts
color: (ctx) => ctx.index === controller.highlightedIndex() ? ACCENT_CHART : TEXT_MUTED,
font:  (ctx) => ({ weight: ctx.index === controller.highlightedIndex() ? 700 : 400 }),
```

A junior reader has to scan both expressions to confirm they
agree on which tick is highlighted. Extracting a tiny local
helper makes the intent explicit and removes the duplication —
same shape as the bug-116 refactor for the glow plugin.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

```bash
grep -c "ctx.index === controller.highlightedIndex" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts
```

Returns 2.

## Expected

A local helper, e.g.

```ts
const isHighlighted = (ctx: { index?: number }) =>
  ctx.index === controller.highlightedIndex();
```

declared once next to the plugin/constants, with both
Scriptables reading `isHighlighted(ctx)`. Refactor only — no
behaviour change.

## Verification

- Unit (TS source): the inline predicate appears at most once
  (in the helper definition); the two Scriptables both call
  `isHighlighted(ctx)`.
- All existing consistency-trend specs continue to pass.
