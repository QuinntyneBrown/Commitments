---
id: bug-196
title: Login card is full-width (1280px) not centered — form fields are horizontal not stacked, button says "Sign in" not "Submit", button never disabled
status: Fixed
---

## Symptoms

- `mat-card` stretches to 1280px (full viewport width), top: 0, left: 0 — not centered
- Form fields (Username, Password) render on one horizontal row instead of stacked vertically
- Button text is "Sign in" — flow spec and Playwright POM expect "Submit"
- Submit button is never disabled even with an empty/invalid form
- Card background is `rgb(66,66,66)` not `rgb(30,30,30)` (`--cui-surface`)
- Card shadow is generic Material shadow, not the design's `rgba(0,0,0,0.7) 0px 8px 24px`
- No "Commitments" brand title or "Login" subtitle in the card

## Expected (from design + login.spec.ts)

- Card: 480px wide, vertically centered, background `rgb(30,30,30)`, border-radius 8px, box-shadow `rgba(0,0,0,0.7) 0px 8px 24px`
- Inside card: `mat-card-title` "Commitments", `mat-card-subtitle` or similar "Login"
- Form fields stacked vertically, full-width
- Button: full-width, text "Submit", disabled while form is invalid

## Fix

Add SCSS to `login-page.component.scss` centering the host; update the template to add title, subtitle, correct button label, and `[disabled]` binding; add `mat-card-header` content.
