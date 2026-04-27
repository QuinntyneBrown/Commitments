---
id: 021
title: Login card "Login" subtitle is white - design specifies $text-secondary (#B0B0B0)
status: open
discovered: 2026-04-27
flow: authentication
severity: low
---

# Login card "Login" subtitle is white — design specifies $text-secondary (#B0B0B0)

## Summary

`docs/ui-design.pen` → `loginLGCard` (id `fs8TX`) places a "Login"
text node with `fill: $text-secondary` (`#B0B0B0`),
`fontSize: $fs-xl` (20 px), `fontWeight: $fw-medium` (500).

The running mat-card-title computes:

  color  : rgb(255, 255, 255)   ← should be rgb(176, 176, 176)
  weight : 500                  ← matches design
  size   : 20px                 ← matches design

So the font scale + weight already line up; only the colour
mismatches. The "Login" subtitle should sit on the dark card as
secondary text below the white "Commitments" h1, matching the
design hierarchy.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/login` at lg-desktop.
2. Inspect the `mat-card-title` and read its computed `color`.

**Expected:** `rgb(176, 176, 176)` (`--cui-text-secondary`).
**Actual:** `rgb(255, 255, 255)`.

Screenshot: `screenshots/login-actual-1280.png`.

## Fix outline (radically simple)

One new rule (3 lines) in
`projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss`:

```scss
mat-card-title {
  color: var(--cui-text-secondary);
}
```

The empty `mat-card-title { }` block was deleted in bug 004's
audit follow-up; this re-introduces it with one declaration.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/login.page.ts` with
`cardTitleColor()` and add a `login.spec.ts` test asserting
`rgb(176, 176, 176)`. Currently fails with `rgb(255, 255, 255)`.
