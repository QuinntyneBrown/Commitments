---
id: 025
title: Daily Results progress fill is off-palette #2e9d68 - should match the metric's --cui-success
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 4479306
flow: dashboard-layout
severity: low
---

# Daily Results progress fill is off-palette #2e9d68 — should match the metric's --cui-success

## Summary

The Daily Results tile's progress bar fill (`.progress span`)
hard-codes `background: #2e9d68` — yet another off-palette green.
After bug 024 the metric value just above it now reads as
`--cui-success` (`#66BB6A`); the fill should read in the same
green so the two visually link as one indicator. The track
(`.progress`) was already moved to `--cui-divider` in bug 018.

This is the **last** off-palette literal anywhere in
`projects/commitments-dashboard-plugin/src`.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the green strip at the bottom of the Daily Results
   tile.
3. Inspect `.progress span` and read its computed
   `background-color`.

**Expected:** `rgb(102, 187, 106)` (`--cui-success`).
**Actual:** `rgb(46, 157, 104)` — off-palette green that doesn't
match the metric just above.

Screenshot: `screenshots/dashboard-actual-1280.png`.

## Fix outline (radically simple)

One swap in
`projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`:

  - `.progress span { background: var(--cui-success); }`

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`progressFillBackground()` and add a `dashboard.spec.ts` test
asserting `rgb(102, 187, 106)`. Currently fails with
`rgb(46, 157, 104)`.
