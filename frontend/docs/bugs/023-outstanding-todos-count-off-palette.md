---
id: 023
title: Outstanding To Dos count metric is dim off-palette #8b3f10 - design palette has --cui-warning (#FFA726)
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: d09faf2
flow: dashboard-layout
severity: low
---

# Outstanding To Dos count metric is dim off-palette #8b3f10 — design palette has --cui-warning (#FFA726)

## Summary

The Outstanding To Dos tile renders its primary metric ("4") in
`#8b3f10` — a deep burnt orange that is technically readable on
the `--cui-surface-2` tile but reads dim next to the brighter
metrics in the other tiles (white, green, etc.). Crucially the
colour is **off-palette**: it isn't declared in
`projects/commitments-app/src/styles/theme.scss` or used
anywhere else in the design system.

The design system already provides `--cui-warning: #FFA726`
(amber) — a bright, on-palette orange that fits the "queue
needs attention" semantic of this metric.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the "4" inside the Outstanding To Dos tile.
3. Inspect `.todo-count` and read its computed `color`.

**Expected:** `rgb(255, 167, 38)` (`--cui-warning` = `#FFA726`).
**Actual:** `rgb(139, 63, 16)` — dim burnt orange.

Screenshot: `screenshots/dashboard-actual-1280.png`.

## Fix outline (radically simple)

One swap in
`projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss`:

  - `.todo-count { color: var(--cui-warning); }`

The semantic stays "warning / attention" but the colour becomes
on-palette and bright enough to read at a glance.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`outstandingTodosCountColor()` and add a `dashboard.spec.ts`
test asserting `rgb(255, 167, 38)`. Currently fails with
`rgb(139, 63, 16)`.
