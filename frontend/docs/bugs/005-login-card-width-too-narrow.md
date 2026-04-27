---
id: 005
title: Login card max-width is 370px - design specifies 480px at LG/XL
status: open
discovered: 2026-04-26
flow: authentication
severity: medium
---

# Login card max-width is 370px — design specifies 480px at LG/XL

## Summary

`docs/ui-design.pen` → frame `Login — LG (1280)` (id `8xz6c`) shows the
login card at width **480 px** with 40 px padding. Same width
(480) at the XL breakpoint, 440 px at M (768), fill-container at S (360).

The current stylesheet caps the card at **370 px**:

```scss
mat-card {
  margin: 0 auto;
  max-width: 370px;
  width: 100%;
  height: 416px;
  padding: 40px 40px 40px 40px;
}
```

So at every viewport ≥ 370 px the card is too narrow; the gap between
the card edge and the inputs is wrong, and the form looks cramped.

## Reproduction

1. Start frontend, navigate to `http://127.0.0.1:4200/login` at a
   viewport ≥ 480 px wide.
2. Inspect `mat-card`'s computed width.

**Expected:** 480 (= `Login — LG (1280)` design value).
**Actual:** 370.

## Fix outline (radically simple)

Change the single literal `max-width: 370px` to `max-width: 480px` in
`projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss`.

Tradeoff (out of scope): full responsive parity would also adjust to
440 px at the M breakpoint and remove the fixed `height: 416px`. Those
are separate visual bugs.

## Tests to add (failing first)

E2E (Playwright, POM): extend `login.spec.ts` to assert the
`mat-card` bounding-box width is `480` at the lg-desktop viewport.
