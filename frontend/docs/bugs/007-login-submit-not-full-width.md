---
id: 007
title: Login Submit button is right-aligned and content-width instead of design's full-width raised-primary
status: open
discovered: 2026-04-26
flow: authentication
severity: medium
---

# Login Submit button is right-aligned and content-width instead of design's full-width raised-primary

## Summary

`docs/ui-design.pen` → frame `Login — LG (1280)` (id `8xz6c`)
shows the Submit action as the `Btn/Raised/Primary` reusable
component (id `AgzGC`) with `width: fill_container` — i.e. the
button spans the full width of the card's content area
(card 480 px – padding 2 × 40 = 400 px).

The current stylesheet shrinks the button to its label
("Submit", ~75 px) and pushes it to the right via
`form { text-align: right }`:

```scss
form {
  margin: 20px 0px 0px 0px;
  text-align: right;
}
button {
  margin-top: 40px;
}
```

So the call-to-action is small and floated, instead of the design's
prominent full-bleed raised-primary block.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/login` at the lg-desktop
   viewport.
2. Inspect the Submit button's bounding-rect width.

**Expected:** ~400 (card content width).
**Actual:** ~75 (just enough for the label).

Screenshot: `screenshots/login-actual-1280.png`.

## Fix outline (radically simple)

Two-line CSS change in
`projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss`:

  1. Drop `text-align: right` from the `form` rule (it stops doing
     anything useful once the button fills the row).
  2. Add `width: 100%` to the `button` rule.

The mat-card's 40 px padding already gives the button the same
left/right insets the design has.

## Tests to add (failing first)

E2E (Playwright, POM): extend `login.spec.ts` to assert the
Submit button's bounding-rect width is ≥ 380 (card content width
minus a small tolerance) at the lg-desktop viewport.
