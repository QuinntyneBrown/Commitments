---
id: 004
title: Login page renders on a transparent / light background instead of the dark design surface
status: open
discovered: 2026-04-26
flow: authentication
severity: high
---

# Login page renders on a transparent / light background instead of the dark design surface

## Summary

The design (`docs/ui-design.pen` → frame `Login — LG (1280)` (id `8xz6c`))
specifies a fully dark surface: page background `$bg` = `#121212`, card
fill `$surface` = `#1E1E1E`, white headings.

The running app shows the login page on a transparent `<body>`
(`rgba(0, 0, 0, 0)`) inside Angular Material's **light** theme — white
card, dark-grey "Commitments" heading on white, default Material
indigo button. The dark-theme palette declared in
`projects/commitments-app/src/styles/theme.scss` is gated by a
`.dark-theme` class that is never applied to any element, and `body`
never references `--cui-bg`.

## Reproduction

1. Start frontend (`npm run start`) and navigate to
   `http://127.0.0.1:4200/login`.
2. Inspect the `<body>` background.

**Expected:** `rgb(18, 18, 18)` (= `#121212`, design `$bg`).
**Actual:** `rgba(0, 0, 0, 0)` (transparent — falls through to white
HTML default).

Screenshot: `screenshots/login-actual-1280.png` (light theme).
Design reference: `docs/ui-design.pen` → node `8xz6c`.

## Evidence

- `projects/commitments-app/src/styles.scss` styles `body, html` with
  margin/font/size — no `background-color`.
- `projects/commitments-app/src/styles/theme.scss` defines the dark
  Material palette under `.dark-theme { @include
  mat.all-component-colors($commitments-dark-theme); }` but no element
  wears that class.
- `projects/commitments-app/src/index.html` `<body>` has no `class`
  attribute.

## Fix outline (radically simple)

1. `index.html` — add `class="dark-theme"` to `<body>`.
2. `styles.scss` — give `body, html` a `background-color: var(--cui-bg)`
   and `color: var(--cui-text-primary)` so the design's `$bg` token is
   applied at the page level.

Two tiny edits, no new files, no new dependencies.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/login.spec.ts` to assert that the
page body's computed `background-color` is `rgb(18, 18, 18)` after the
`/login` navigation.
