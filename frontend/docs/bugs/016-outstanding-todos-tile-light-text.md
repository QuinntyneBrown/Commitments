---
id: 016
title: Outstanding To Dos tile description text is invisible (#5b6b84) on the dark surface
status: fixed
discovered: 2026-04-26
fixed: 2026-04-26
fixed_in: b3589d3
flow: dashboard-layout
severity: high
---

# Outstanding To Dos tile description text is invisible (#5b6b84) on the dark surface

## Summary

Same class of bug as 013 (Weekly Focus) and 014 (Relations) — the
Outstanding To Dos tile hard-codes light-theme text colours.
The `.todo-copy` paragraph ("items need attention before the next
review.") uses `color: #5b6b84` which is barely readable on the
dark `--cui-surface-2` tile and effectively cuts the user off
from the body of the message.

The `.todo-count` ("4") uses `#8b3f10` (a deep burnt orange)
which technically reads on dark, but is well off the design
palette. Out of scope here — the priority is making the body
copy legible. A separate bug can rationalise the metric
accent later.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the Outstanding To Dos tile.
3. Inspect `.todo-copy` and read its computed `color`.

**Expected:** `rgb(176, 176, 176)` (`--cui-text-secondary`).
**Actual:** `rgb(91, 107, 132)` — slate, barely visible.

Screenshot: `screenshots/dashboard-actual-1280.png` (the "items
need attention" copy fades into the surface).

## Fix outline (radically simple)

One swap in
`projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss`:

  - `.todo-copy { color: var(--cui-text-secondary); }`

No HTML / TS change.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`outstandingTodosCopyColor()` and add a `dashboard.spec.ts` test
asserting `rgb(176, 176, 176)`. Currently fails with
`rgb(91, 107, 132)`.
