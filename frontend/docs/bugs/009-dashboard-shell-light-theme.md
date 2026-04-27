---
id: 009
title: Dashboard shell still hard-codes light-theme colors instead of using the dark design tokens
status: fixed
discovered: 2026-04-26
fixed: 2026-04-26
fixed_in: a4af0e0
flow: dashboard-layout
severity: high
---

# Dashboard shell still hard-codes light-theme colors instead of using the dark design tokens

## Summary

`docs/ui-design.pen` → frame `Dashboard — LG (1280)` (id `OxYKj`) shows
the dashboard on a fully dark surface: shell background `--cui-bg`
(`#121212`), toolbar `--cui-toolbar` (`#1F2233`), white headings, muted
secondary text, and a `#3A3A3A` divider beneath the toolbar.

The running shell ignores those tokens and hard-codes a light theme
inside `dashboard-shell.component.scss`:

```scss
:host {
  color: #172033;
  background: #f4f7fb;          /* light bg */
}
.dashboard-shell__topbar {
  background: #ffffff;          /* white toolbar */
  border-bottom: 1px solid #dce5ef;
}
h1 {
  color: #172033;               /* near-black title */
}
.dashboard-shell__eyebrow {
  color: #65758d;               /* muted slate */
}
```

So the body now correctly fills with `--cui-bg` (after bug 004), but
the dashboard renders a bright white toolbar and pale gutter on top of
it — the opposite of the design.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Inspect `getComputedStyle(.dashboard-shell__topbar).backgroundColor`.

**Expected:** `rgb(31, 34, 51)` (= `#1F2233`, the `--cui-toolbar` token
bound to the design's `Mat-Toolbar` fill).
**Actual:** `rgb(255, 255, 255)`.

Screenshot: `screenshots/dashboard-actual-1280.png`.

## Fix outline (radically simple)

Edit `projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.scss`:

  - `:host`            — `background: var(--cui-bg);`
                         `color: var(--cui-text-primary);`
  - `.dashboard-shell__topbar` — `background: var(--cui-toolbar);`
                                 `border-bottom: 1px solid var(--cui-divider);`
  - `h1`               — `color: var(--cui-text-primary);`
  - `.dashboard-shell__eyebrow` — `color: var(--cui-text-secondary);`

Five token swaps in one SCSS file, no JS or HTML changes. The dark
tokens already exist (registered in `styles/theme.scss` and consumed
elsewhere) — this just routes the shell through them.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with a
`topbarBackgroundColor()` helper and add a test in
`dashboard.spec.ts` asserting the topbar background is
`rgb(31, 34, 51)`. Currently fails — it computes
`rgb(255, 255, 255)`.
