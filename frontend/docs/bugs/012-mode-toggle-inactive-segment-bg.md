---
id: 012
title: Mode-toggle inactive segments still wear Material's default #424242 fill instead of being transparent
status: open
discovered: 2026-04-26
flow: dashboard-modes
severity: medium
---

# Mode-toggle inactive segments still wear Material's default #424242 fill instead of being transparent

## Summary

`docs/ui-design.pen` → `Mode-Toggle` (id `A1yim`) is a pill that
shows the active option painted with the accent / primary token
and the inactive option as just the label sitting on the toggle's
own surface — i.e. inactive segments have **no fill**.

The `cui-mode-toggle` SCSS already paints the container correctly
(`background: var(--cui-surface-2)`, `border: 1px solid
var(--cui-divider)`) and explicitly fills the active segments,
but it never resets the default `mat-button-toggle` background.
Material's dark palette renders inactive `mat-button-toggle`
elements at `#424242`, which leaks through and shows as a lighter
"chip" beside the active segment.

Computed today: container = `rgb(36, 36, 36)`, inactive segment =
`rgb(66, 66, 66)`. Two visibly different greys side-by-side.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Inspect the inactive mode-toggle segment
   (`.mode-toggle__segment:not(.mode-toggle__segment--active)`).

**Expected:** `rgba(0, 0, 0, 0)` (transparent — the segment
inherits the toggle container's surface).
**Actual:** `rgb(66, 66, 66)`.

Screenshot: `screenshots/dashboard-actual-1280.png` (look at the
"Review" pill next to the pink "Live" pill).

## Fix outline (radically simple)

Add a single declaration to the existing
`.mode-toggle__segment` rule in
`projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.scss`:

```scss
.mode-toggle__segment {
  …
  background: transparent;
}
```

Because the active variants already specify their own backgrounds
(`--cui-accent` for live, `--cui-primary` for review) they keep
working unchanged. View encapsulation gives our selector higher
specificity than Material's default, so no `!important` is needed.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`inactiveModeSegmentBackground()` and a `dashboard.spec.ts` test
asserting `rgba(0, 0, 0, 0)`. Currently fails — `rgb(66, 66, 66)`.
