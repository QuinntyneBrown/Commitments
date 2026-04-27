---
id: 020
title: Login card paints Material's default #424242 instead of the design's $surface (#1E1E1E)
status: open
discovered: 2026-04-27
flow: authentication
severity: low
---

# Login card paints Material's default #424242 instead of the design's $surface (#1E1E1E)

## Summary

`docs/ui-design.pen` → frame `Login — LG (1280)` (id `8xz6c`)
fills the `loginLGCard` with `$surface` = `#1E1E1E`
(`--cui-surface`). The running card uses Material's dark-theme
`mat-card` background which resolves to `#424242` — visibly
lighter than the surrounding `--cui-surface-2` palette.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/login` at lg-desktop.
2. Inspect `mat-card`'s computed `background-color`.

**Expected:** `rgb(30, 30, 30)` (`--cui-surface`).
**Actual:** `rgb(66, 66, 66)` — Material's default dark surface.

Screenshot: `screenshots/login-actual-1280.png` (the card is
visibly lighter than the rest of the dark surface).

## Fix outline (radically simple)

One declaration added to the existing `mat-card` rule in
`projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss`:

```scss
mat-card {
  …
  background-color: var(--cui-surface);
}
```

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/login.page.ts` with
`cardBackgroundColor()` and add a `login.spec.ts` test asserting
`rgb(30, 30, 30)`. Currently fails — `rgb(66, 66, 66)`.
