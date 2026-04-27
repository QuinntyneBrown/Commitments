---
id: 006
title: Login card sits at the top of the viewport instead of being vertically centered
status: fixed
discovered: 2026-04-26
fixed: 2026-04-26
fixed_in: a34d642
flow: authentication
severity: medium
---

# Login card sits at the top of the viewport instead of being vertically centered

## Summary

`docs/ui-design.pen` → frame `Login — LG (1280)` (id `8xz6c`) is a
1280 × 900 column with `justifyContent: center` and `alignItems:
center`, which centers the login card both horizontally and vertically
inside the viewport.

The current layout only centers horizontally
(`mat-card { margin: 0 auto }`). The card sits flush with the top of
the viewport and the entire bottom half of the screen is empty.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/login` at the lg-desktop
   viewport (1280 × 800).
2. Inspect the card's vertical position.

**Expected:** the card's vertical center is at approximately
`viewport.height / 2` (= 400 px).
**Actual:** the card top is roughly at the viewport top — the
midpoint sits well above 400 px.

Screenshot: `screenshots/login-actual-1280.png` (card hugs top edge,
empty space below).

## Fix outline (radically simple)

Make `:host` a flex column that fills the viewport and centers its
single child (the card) on both axes. CSS already centers
horizontally via `mat-card { margin: 0 auto }` — adding flex on the
host adds vertical centering with no other layout changes.

```scss
:host {
  display: flex;
  width: 100%;
  min-height: 100vh;
  align-items: center;
  justify-content: center;
}
```

That replaces the existing single-line `:host { width: 100%; }`.

The fixed `mat-card { height: 416px }` rule is a related but
separate issue (the design has no fixed card height — it sizes from
content) and will be tracked as its own bug.

## Tests to add (failing first)

E2E (Playwright, POM): extend `login.spec.ts` to assert the card's
bounding-rect vertical midpoint lands near `viewport.height / 2`
(within 20 px tolerance to allow for body padding / margin).
