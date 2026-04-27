---
id: 018
title: Daily Results progress track is hard-coded #e6edf4 - shows as a bright bar on the dark surface
status: open
discovered: 2026-04-26
flow: dashboard-layout
severity: medium
---

# Daily Results progress track is hard-coded #e6edf4 — shows as a bright bar on the dark surface

## Summary

The Daily Results tile renders a small horizontal progress bar
beneath the metric. The fill (`.progress span`) uses
`background: #2e9d68` which reads as a healthy green on dark, but
the track (`.progress`) is `background: #e6edf4` — a near-white
that draws the eye away from the green and breaks the dark
surface.

Design (`Real-Time-Metric-Tile`, id `o0BgI` in
`docs/ui-design.pen`) keeps the track at the divider tone so the
fill carries the visual weight.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the very bottom of the Daily Results tile.
3. Inspect `.progress` and read its computed `background-color`.

**Expected:** `rgb(58, 58, 58)` (`--cui-divider`).
**Actual:** `rgb(230, 237, 244)` — almost white.

Screenshot: `screenshots/dashboard-actual-1280.png` (the bright
strip across the bottom of the leftmost tile).

## Fix outline (radically simple)

One swap in
`projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`:

  - `.progress { background: var(--cui-divider); }`

The fill colour `#2e9d68` is left unchanged — it already
contrasts well against the divider grey.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`progressTrackBackground()` and add a `dashboard.spec.ts` test
asserting `rgb(58, 58, 58)`. Currently fails with
`rgb(230, 237, 244)`.
