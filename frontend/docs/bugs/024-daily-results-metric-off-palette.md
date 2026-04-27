---
id: 024
title: Daily Results metric value is off-palette #1d5f3f - design palette has --cui-success (#66BB6A)
status: open
discovered: 2026-04-27
flow: dashboard-layout
severity: low
---

# Daily Results metric value is off-palette #1d5f3f — design palette has --cui-success (#66BB6A)

## Summary

The Daily Results tile renders its primary metric ("7 / 9") in
`#1d5f3f` — a deep, unsaturated forest green that is technically
readable on the dark `--cui-surface-2` tile but reads dimmer
than every other metric on the dashboard. Crucially, the colour
is **off-palette**: it isn't declared in
`projects/commitments-app/src/styles/theme.scss` or used
anywhere else.

The design system already provides `--cui-success: #66BB6A` —
the same green Material's `m2-define-dark-theme` uses for
positive states, and the value the design's
`Mat-Snackbar/Success` pattern (id `8LaDm`) draws from.

Same class of bug as 023 (off-palette burnt-orange Outstanding
To Dos count). This is the last off-palette accent literal in
the dashboard tiles.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the "7 / 9" inside the Daily Results tile.
3. Inspect `.metric__value` and read its computed `color`.

**Expected:** `rgb(102, 187, 106)` (`--cui-success` = `#66BB6A`).
**Actual:** `rgb(29, 95, 63)` — dim off-palette green.

Screenshot: `screenshots/dashboard-actual-1280.png`.

## Fix outline (radically simple)

One swap in
`projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`:

  - `.metric__value { color: var(--cui-success); }`

The "completed / success" semantic stays intact and the colour
becomes on-palette and bright enough to read at a glance.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`dailyResultsMetricColor()` and add a `dashboard.spec.ts` test
asserting `rgb(102, 187, 106)`. Currently fails with
`rgb(29, 95, 63)`.
