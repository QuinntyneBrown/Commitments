---
id: bug-055
title: Consistency Trend passes a wrong-derived `[delta]` plus the full API deltaLabel as `[caption]`, so the badge displays two different deltas back-to-back
status: Open
---

# Bug 055 — Consistency Trend delta-badge wiring duplicates the delta number

**Status**: Open

## Description

`consistency-trend-tile.component.html` currently wires the badge:

```html
<cui-delta-badge
  [delta]="controller.currentPercentage() - controller.lowPercentage()"
  format="percent"
  [caption]="controller.deltaLabel()">
</cui-delta-badge>
```

- `[delta]` is `current% - low%` — a *peak-to-low spread* measure
  unrelated to the trend's headline delta.
- `[caption]` is the API's `deltaLabel`, already a fully-formatted
  string like `"+12% vs prior 14d"` (current vs the prior 14-day
  average).

The badge renders both — value + caption — so the user sees e.g.
`"+28% +12% vs prior 14d"` — two contradicting delta numbers
side-by-side. The .pen `ctDelta` shows a single
`"+12% vs prior 14d"` line.

The fix derives both inputs from the single source of truth
(`deltaLabel`):

- `deltaPercentage = computed parse leading signed number`
- `deltaCaption    = computed parse trailing descriptive text`

Then bind the badge to the parsed pair so it renders one
consistent delta.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html`

## Reproduction

1. Render the Consistency Trend tile with a non-zero
   peak-to-low spread.
2. Inspect the delta-badge — value reads e.g. `+28%`, caption
   reads `+12% vs prior 14d`. Two different numbers.
3. Compare to the .pen — single `+12% vs prior 14d`.

## Expected

`consistency-trend.controller.ts` adds:

```ts
readonly deltaPercentage = computed(() => {
  const match = this.deltaLabel().match(/-?\d+/);
  return match ? Number(match[0]) : 0;
});

readonly deltaCaption = computed(() => {
  const idx = this.deltaLabel().indexOf('%');
  return idx >= 0 ? this.deltaLabel().slice(idx + 1).trim() : '';
});
```

`consistency-trend-tile.component.html`:

```html
<cui-delta-badge
  [delta]="controller.deltaPercentage()"
  format="percent"
  [caption]="controller.deltaCaption()">
</cui-delta-badge>
```

## Verification

- Unit: `consistency-trend.controller.spec.ts` adds three branch
  specs for `deltaPercentage()` (positive, negative, zero) and one
  spec for `deltaCaption()` returning `'vs prior 14d'`.
- Unit: source-level template check — `[delta]` binds to
  `controller.deltaPercentage()` (no longer to the
  `currentPercentage - lowPercentage` expression).
- Visual: the badge shows a single `+12% vs prior 14d` line.
