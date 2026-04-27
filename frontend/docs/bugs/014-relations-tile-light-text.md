---
id: 014
title: Relations tile percentages render invisible (#172033) on the dark surface
status: fixed
discovered: 2026-04-26
fixed: 2026-04-26
fixed_in: cb76365
flow: dashboard-layout
severity: high
---

# Relations tile percentages render invisible (#172033) on the dark surface

## Summary

Same class of bug as 013 (Weekly Focus) — the Relations tile
hard-codes light-theme text colours. The container's labels
(`Health`, `Work`, `Personal`) use `color: #5b6b84` (a mid-slate
that's only barely readable on the dark `--cui-surface-2` tile),
and the bold percentages (`42%`, `33%`, `25%`) use
`color: #172033` (near-black, effectively invisible).

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the Relations tile.
3. Inspect any `.relations strong` and read its computed
   `color`.

**Expected:** `rgb(255, 255, 255)` (`--cui-text-primary`).
**Actual:** `rgb(23, 32, 51)` — near-black on the dark surface.

Screenshot: `screenshots/dashboard-actual-1280.png` (the
percentages are invisible; only the labels show as faint slate).

## Fix outline (radically simple)

Two swaps in
`projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.scss`:

  - `.relations         { color: var(--cui-text-secondary); }`
  - `.relations strong  { color: var(--cui-text-primary); }`

No HTML or TS change.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`relationsValueColor()` and add a `dashboard.spec.ts` test
asserting `rgb(255, 255, 255)`. Currently fails with
`rgb(23, 32, 51)`.
