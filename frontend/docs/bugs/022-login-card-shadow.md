---
id: 022
title: Login card uses Material's faint elevation shadow - design specifies a prominent floating drop
status: open
discovered: 2026-04-27
flow: authentication
severity: low
---

# Login card uses Material's faint elevation shadow — design specifies a prominent floating drop

## Summary

`docs/ui-design.pen` → `loginLGCard` (id `fs8TX`) declares an
outer drop shadow:

```
effect: { type: shadow, shadowType: outer,
          blur: 24, offset: { x: 0, y: 8 }, color: #000000B3 }
```

…matching the design system's `--cui-shadow-floating` token
(`0 6px 12px rgba(0, 0, 0, 0.6)`) — a single, soft, fairly
strong drop that lifts the card off the dark page.

The running mat-card uses Material's default elevation:

```
rgba(0,0,0,0.2)  0  2px 1px -1px,
rgba(0,0,0,0.14) 0  1px 1px  0px,
rgba(0,0,0,0.12) 0  1px 3px  0px
```

…three faint layers that barely register on the dark
surface, so the card reads as flat against `--cui-bg`.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/login` at lg-desktop.
2. Inspect `mat-card`'s computed `box-shadow`.

**Expected:** contains `rgb`/`rgba(0, 0, 0, 0.6) 0px 6px 12px`
(the `--cui-shadow-floating` token).
**Actual:** Material's three-layer default at 12-20 % opacity.

Screenshot: `screenshots/login-actual-1280.png` (the card has no
visible drop on the dark page).

## Fix outline (radically simple)

One declaration added to the existing `mat-card` rule in
`projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss`:

```scss
mat-card {
  …
  box-shadow: var(--cui-shadow-floating);
}
```

The token is already declared in
`projects/commitments-app/src/styles/theme.scss`.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/login.page.ts` with
`cardBoxShadow()` and add a `login.spec.ts` test asserting the
computed shadow contains `rgba(0, 0, 0, 0.6) 0px 6px 12px`.
Currently fails with the Material three-layer default.
