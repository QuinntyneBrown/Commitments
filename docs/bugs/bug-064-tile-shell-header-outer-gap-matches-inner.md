---
id: bug-064
title: Tile-shell `.tile-shell__header` hard-codes gap 12px while `.tile-shell__title-row` uses the per-tile `--cui-tile-header-gap`
status: Fixed
---

# Bug 064 — Tile-shell outer header gap should track the inner gap

**Status**: Fixed

## Fix

`.tile-shell__header { gap: 12px }` →
`gap: var(--cui-tile-header-gap, 8px)`.

Both header gap sources (outer between heading and pill, inner
between icon and title) now share one custom property:
- 5 tiles default to 8px (was 12px) → 4px tighter heading-to-pill.
- consistency-trend `--cui-tile-header-gap: 10px` override applies
  uniformly → 2px tighter heading-to-pill.

Aligns each tile with its .pen header `gap` value (drHd/wfHd/etc
gap: 8, ctHd gap: 10).

Coverage:
- New spec asserts `.tile-shell__header` consumes
  `var(--cui-tile-header-gap)` and no longer matches `gap: 12px`.
- All 22 affected suites pass (150/150 — was 149/149 before).

## Description

After bug-050 introduced the `--cui-tile-header-gap` custom
property (default 8px, consistency-trend overrides to 10px), the
inner gap between icon and title text uses the per-tile value:

```scss
.tile-shell__title-row {
  …
  gap: var(--cui-tile-header-gap, 8px);
}
```

The OUTER header gap, however, still hard-codes 12px:

```scss
.tile-shell__header {
  …
  gap: 12px;
}
```

Each .pen design declares a single `gap` on its header frame
(e.g. `drHd gap: 8`, `ctHd gap: 10`) and applies it uniformly
to all header children — icon, title block, spacer, pill. The
implementation's split between inner 8/10 and outer 12 means the
heading-to-pill spacing is 4px (or 2px for chart) wider than the
icon-to-title spacing.

The fix has the outer header consume the same custom property
so both gaps share one source of truth — and the .pen designs'
single `gap` value applies to both.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`

## Reproduction

```bash
grep -nE 'gap\s*:' frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss
```

Two different gap values for header concerns.

## Expected

```scss
.tile-shell__header {
  …
  gap: var(--cui-tile-header-gap, 8px);
}
```

After the change, daily-results / weekly-focus / monthly-progress
/ outstanding-todos / relations all render an 8px gap between the
heading block and the pill (matching their .pen header `gap: 8`).
Consistency-trend renders 10px (matching `ctHd gap: 10`) via its
`--cui-tile-header-gap: 10px` override on `:host`.

## Verification

- Unit: source-level SCSS spec — `.tile-shell__header` declares
  `gap: var(--cui-tile-header-gap)` (no longer literal `12px`).
- All 22 affected suites continue to pass — no behaviour change
  beyond the 2–4px gap reduction.
