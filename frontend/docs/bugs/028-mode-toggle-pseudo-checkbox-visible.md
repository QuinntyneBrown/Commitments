---
id: 028
title: Mode-toggle segments still render Material's mat-pseudo-checkbox alongside the colour pill
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 1d73584
flow: dashboard-modes
severity: low
---

# Mode-toggle segments still render Material's mat-pseudo-checkbox alongside the colour pill

## Summary

`docs/ui-design.pen` `Mode-Toggle` (id `A1yim`) is a pure
colour-pill toggle: only the active segment is filled, everything
else is just the label. Angular Material's `mat-button-toggle`
ships with a `mat-pseudo-checkbox` indicator inside every segment
(intended for multi-select toggle groups). Our toggle is
single-select, but the checkbox still renders — visible as a small
checkmark glyph next to the segment labels in review mode (and
inside the Live pill in live mode).

It's a UX leak from Material's defaults. The colour-pill itself
already conveys which segment is active; the pseudo-checkbox just
adds noise.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Toggle to **Review**.
3. Inspect either segment — note the small checkbox glyph
   inside the segment chrome.

**Expected:** segments contain only the dot/icon + label
specified in the design.
**Actual:** every segment additionally renders a
`<mat-pseudo-checkbox>` glyph.

Screenshot: `screenshots/dashboard-review-actual-1280.png`
(checkmark visible to the left of "Live").

## Fix outline (radically simple)

One CSS rule in
`projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.scss`:

```scss
.mode-toggle ::ng-deep .mat-pseudo-checkbox {
  display: none;
}
```

The `::ng-deep` shadow-piercing selector is required because
`mat-pseudo-checkbox` is rendered inside Material's component
DOM, beyond the cui-mode-toggle view encapsulation boundary.
One rule, no HTML / TS change.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`modeToggleCheckboxVisible()` and add a `dashboard.spec.ts` test
asserting no `.mat-pseudo-checkbox` inside `.mode-toggle` is
visible. Currently fails — Material renders one per segment.
