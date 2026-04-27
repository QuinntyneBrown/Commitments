---
id: 011
title: Dashboard topbar action buttons + tile-select still hard-code light theme colors
status: open
discovered: 2026-04-26
flow: dashboard-layout
severity: medium
---

# Dashboard topbar action buttons + tile-select still hard-code light theme colors

## Summary

After bug 009 + 010 the body, topbar, and grid all read as the
design's dark surface, but the topbar's right-hand actions (the
tile-select `<select>`, `Add Tile`, `Edit Layout`, `Reset` buttons)
still hard-code white backgrounds and slate borders inside
`dashboard-shell.component.scss`:

```scss
.dashboard-shell__select,
.dashboard-shell__button {
  border: 1px solid #c9d4e2;
  background: #ffffff;
  color: #172033;
}
```

So the dark topbar gets four bright white buttons floated to the
right that visually break the theme.

The design system equivalent is `Btn/Stroked` (id `45CHX`) —
transparent fill, `--cui-primary` (`#9FA8DA`) stroke and label.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Inspect `[data-testid="add-tile"]`'s background-color.

**Expected:** `rgba(0, 0, 0, 0)` (transparent — the stroked
button design pattern lets the dark toolbar show through).
**Actual:** `rgb(255, 255, 255)`.

Screenshot: `screenshots/dashboard-actual-1280.png`.

## Fix outline (radically simple)

Three rules in `dashboard-shell.component.scss`:

  - `.dashboard-shell__select, .dashboard-shell__button` —
    `background: transparent;`
    `border-color: var(--cui-primary);`
    `color: var(--cui-primary);`
  - `.dashboard-shell__button:hover` —
    `border-color: var(--cui-primary-strong);`
    `color: var(--cui-text-primary);`
  - `.dashboard-shell__button--active` —
    `background: var(--cui-primary);`
    `color: var(--cui-on-primary);`
    `border-color: var(--cui-primary);`

`--button--muted` keeps its own muted text color via
`--cui-text-secondary`.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`addTileButtonBackground()` and add a `dashboard.spec.ts` test
asserting it computes `rgba(0, 0, 0, 0)`. Currently fails with
`rgb(255, 255, 255)`.
