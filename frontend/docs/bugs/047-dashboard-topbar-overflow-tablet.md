---
id: 047
title: Dashboard topbar overflows at the 768 px tablet viewport - Reset button is partially clipped
status: open
discovered: 2026-04-27
flow: dashboard-layout
severity: medium
---

# Dashboard topbar overflows at the 768 px tablet viewport — Reset button is partially clipped

## Summary

`dashboard-shell.component.scss` declares the responsive
breakpoint at **`max-width: 760px`** — the column-stack only
kicks in below 761 px. Playwright runs e2e at the
**`tablet (768 × 1024)`** viewport, which sits just above the
breakpoint, so the topbar stays in row layout and the
brand + mode-toggle + tile-select + Add Tile + Edit Layout +
Reset chain overflows the 768 px viewport. The **Reset**
button is partially clipped at the right edge.

Mobile (≤760) is fine; lg/xl-desktop (≥1280) is fine; the gap
between 761 px and the chain's natural width breaks the layout.

## Reproduction

1. Run the dashboard at the `tablet` Playwright project
   (768 × 1024) - or open
   `http://127.0.0.1:4200/` and resize the window to 768.
2. Inspect the right edge of the topbar.

**Expected:** all six topbar children visible.
**Actual:** the **Reset** button is half off-screen.

Screenshot: `screenshots/dashboard-actual-768.png`.

## Fix outline (radically simple)

One declaration added to the existing
`.dashboard-shell__topbar` rule:

```scss
.dashboard-shell__topbar {
  …
  flex-wrap: wrap;
}
```

`flex-wrap` lets the topbar children wrap to a second row
when they don't fit horizontally — a graceful fallback that
covers every viewport between the existing 760 px column
breakpoint and the natural row width, *without* introducing a
new media query.

The 760 px column-stack rule stays for true mobile.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts`
with `topbarBoundsExceedsViewport()` checking the topbar's
`scrollWidth` vs the viewport width, and add a
`viewport.spec.ts` test at the existing tablet project that
asserts no horizontal overflow. Currently fails — the topbar
overflows by ~70 px.
