---
id: 017
title: Daily Results tile metric label is invisible (#5b6b84) on the dark surface
status: open
discovered: 2026-04-26
flow: dashboard-layout
severity: high
---

# Daily Results tile metric label is invisible (#5b6b84) on the dark surface

## Summary

Same class of bug as 013 (Weekly Focus), 014 (Relations), 016
(Outstanding To Dos) — the Daily Results tile's caption
("commitments completed") hard-codes `color: #5b6b84` (the
light-theme slate), which renders barely visible on the dark
`--cui-surface-2` tile.

The other Daily Results colours (`.metric__value` deep green
`#1d5f3f`, the progress track `#e6edf4`, the progress fill
`#2e9d68`) are off-palette but readable; rationalising those
across the design palette is a separate accent / progress-bar
bug.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the Daily Results tile under "7 / 9".
3. Inspect `.metric__label` and read its computed `color`.

**Expected:** `rgb(176, 176, 176)` (`--cui-text-secondary`).
**Actual:** `rgb(91, 107, 132)` — slate, washes out.

Screenshot: `screenshots/dashboard-actual-1280.png` (the
"commitments completed" caption fades into the surface).

## Fix outline (radically simple)

One swap in
`projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`:

  - `.metric__label { color: var(--cui-text-secondary); }`

No HTML / TS change.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`dailyResultsLabelColor()` and add a `dashboard.spec.ts` test
asserting `rgb(176, 176, 176)`. Currently fails with
`rgb(91, 107, 132)`.
