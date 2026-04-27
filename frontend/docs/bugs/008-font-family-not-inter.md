---
id: 008
title: App typography is Roboto - design system specifies Inter and the Inter font is not loaded
status: fixed
discovered: 2026-04-26
fixed: 2026-04-26
fixed_in: f148970
flow: authentication
severity: medium
---

# App typography is Roboto — design system specifies Inter and the Inter font is not loaded

## Summary

Every text node in `docs/ui-design.pen` (including the Login frame
`8xz6c`, the design-system frame `lPXSZ`, and every reusable
component) uses `fontFamily: "Inter"`. Inter is also one of the
named families Pencil registered for the project tokens.

The running app loads only the Material Icons stylesheet in
`projects/commitments-app/src/index.html` — Inter is **never
linked** — and `projects/commitments-app/src/styles.scss` declares
`font-family: Roboto, 'Helvetica Neue', sans-serif`. So every page
in the app renders in Roboto with the wrong letter shapes,
spacing, and weight grid.

This is the foundational bug behind every "the type doesn't match
the design" issue and should land before the per-component
typography tweaks.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/login`.
2. Inspect `getComputedStyle(document.body).fontFamily`.

**Expected:** `Inter` (or a stack starting with `Inter`).
**Actual:** `Roboto, "Helvetica Neue", sans-serif`.

## Fix outline (radically simple)

1. `projects/commitments-app/src/index.html` — add the Google Fonts
   `<link>` for Inter (400/500/600/700) next to the existing
   Material Icons link.
2. `projects/commitments-app/src/styles.scss` — prepend `Inter` to
   the existing font-family stack so it falls back to Roboto when
   Inter fails to load.

No new packages, no new tooling.

## Tests to add (failing first)

E2E (Playwright, POM): extend `login.spec.ts` to assert the body's
computed `font-family` starts with `Inter`. Currently fails because
the body uses `Roboto`.
