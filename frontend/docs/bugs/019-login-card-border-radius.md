---
id: 019
title: Login card border-radius is 4px - design specifies 8px ($r-md)
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 86c9d5f
flow: authentication
severity: low
---

# Login card border-radius is 4px — design specifies 8px ($r-md)

## Summary

`docs/ui-design.pen` → frame `Login — LG (1280)` (id `8xz6c`)
declares the login card with `cornerRadius: $r-md`, which the
design system maps to `--cui-radius-md` = 8 px.

Material's default `mat-card` ships at 4 px, and
`login-page.component.scss` does not override it, so the running
card has noticeably sharper corners than the mockup.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/login` at lg-desktop.
2. Inspect the `mat-card` border-radius.

**Expected:** `8px` (`--cui-radius-md`).
**Actual:** `4px`.

Screenshot: `screenshots/login-actual-1280.png`.

## Fix outline (radically simple)

One declaration added to the existing `mat-card` rule in
`projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss`:

```scss
mat-card {
  …
  border-radius: var(--cui-radius-md);
}
```

The token is already declared in
`projects/commitments-app/src/styles/theme.scss` and used by other
shells.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/login.page.ts` with
`cardBorderRadius()` and add a `login.spec.ts` test asserting
`8px`. Currently fails — Material default `4px`.
