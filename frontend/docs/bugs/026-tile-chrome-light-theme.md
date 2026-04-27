---
id: 026
title: Tile chrome (edit-mode label + remove button) still hard-codes white-ish backgrounds and slate text
status: open
discovered: 2026-04-27
flow: dashboard-layout
severity: medium
---

# Tile chrome (edit-mode label + remove button) still hard-codes white-ish backgrounds and slate text

## Summary

Pressing "Edit Layout" reveals chrome on every tile: a small pill
label (`tile-chrome__label`) and a remove button
(`tile-chrome__button`) plus a destructive hover state. All three
hard-code light-theme colours in
`projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss`:

```scss
.tile-chrome__label  { background: rgba(255, 255, 255, 0.92); color: #4a5b73; border: 1px solid #d9e2ee; }
.tile-chrome__button { background: rgba(255, 255, 255, 0.95); color: #344258; border: 1px solid #d5dde8; }
.tile-chrome__button:hover { background: #bd2b2b; color: #ffffff; border-color: #bd2b2b; }
```

So once the user toggles edit mode, every tile gains a bright
white pill/button overlay against the dark `--cui-surface-2`
tile — the opposite of the design's dark surface story.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Click "Edit Layout".
3. Inspect any `[data-testid="remove-tile"]` button background.

**Expected:** `rgb(44, 44, 44)` (`--cui-surface-3`).
**Actual:** `rgba(255, 255, 255, 0.95)` — bright white pill.

## Fix outline (radically simple)

Three rules in
`projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss`:

  - `.tile-chrome__label`  : `background: var(--cui-surface-3)`,
                             `color: var(--cui-text-secondary)`,
                             `border: 1px solid var(--cui-divider)`.
  - `.tile-chrome__button` : `background: var(--cui-surface-3)`,
                             `color: var(--cui-text-primary)`,
                             `border: 1px solid var(--cui-divider)`.
  - `.tile-chrome__button:hover` : `background: var(--cui-warn)`,
                                   `border-color: var(--cui-warn)`,
                                   `color: var(--cui-text-primary)`.

Same hex-to-token pattern that landed every tile body update.
Eight literal swaps across three rules; no HTML or TS change.

The empty-state colours (`.dashboard-grid__empty`,
`__empty h2`) are also light-coded but not visible at the
default 5-tile layout — separate bug if/when they surface.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`removeTileButtonBackground()` (after entering edit mode) and add
a `dashboard.spec.ts` test asserting `rgb(44, 44, 44)`. Currently
fails with `rgba(255, 255, 255, 0.95)`.
