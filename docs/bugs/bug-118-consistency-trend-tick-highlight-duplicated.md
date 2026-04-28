---
id: bug-118
title: consistency-trend tick color/font Scriptables duplicate the highlightedIndex check
status: Open
---

# Bug 118 — DRY tick highlight check

**Status**: Open

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
