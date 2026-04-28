---
id: bug-067
title: Tile-shell title doesn't pin font-family; relies on Material's typography cascade
status: Open
---

# Bug 067 — Tile-shell title font-family relies on cascade

**Status**: Open

## Description

Every tile .pen design explicitly declares `fontFamily: Inter`
on the title text node (e.g. daily-results' `Mmu4C`,
consistency-trend's `ZVlcb`, etc.). The implementation in
`tile-shell.component.scss`:

```scss
.tile-shell__title {
  margin: 0;
  color: var(--cui-text-primary, #FFFFFF);
  font-size: 16px;
  font-weight: 500;
  line-height: 1.2;
}
```

— has no `font-family` declaration. The `<mat-card-title>` element
inherits font-family from Material's typography theme, which
`commitments-app/src/styles/theme.scss` configures as
`'Inter, Roboto, "Helvetica Neue", sans-serif'`. Should the theme
ever change, the title would silently drift to Material's
fallback chain.

`_tokens.scss` already exposes the design-system font as a CSS
custom property:

```scss
--cui-font-display: 'Inter';
```

Pinning `font-family: var(--cui-font-display, 'Inter')` on
`.tile-shell__title` makes the title's typography choice explicit
and independent of Material's typography theme.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`

## Reproduction

```bash
grep -n 'font-family' frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss
# (no match)
```

## Expected

```scss
.tile-shell__title {
  margin: 0;
  color: var(--cui-text-primary, #FFFFFF);
  font-family: var(--cui-font-display, 'Inter');
  font-size: 16px;
  font-weight: 500;
  line-height: 1.2;
}
```

## Verification

- Unit: source-level SCSS spec — `.tile-shell__title` rule
  references `--cui-font-display` (or pins `Inter`).
- Visual: no change in the typical theme-loaded environment;
  resilience to Material typography drift.
- All 22 affected suites continue to pass.
