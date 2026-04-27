---
id: 013
title: Weekly Focus tile labels render invisible (#172033) on the dark surface
status: open
discovered: 2026-04-26
flow: dashboard-layout
severity: high
---

# Weekly Focus tile labels render invisible (#172033) on the dark surface

## Summary

After dropping the dashboard into the dark design surface (bugs
009 + 010), every per-tile component still hard-codes the
light-theme text colour. The first symptom is the **Weekly Focus**
tile: its primary labels (`Move`, `Read`, `Reflect`) use
`color: #172033` — the same near-black the shell used to use —
which is essentially invisible on the now-dark `--cui-surface-2`
tile background. The secondary span (`5 sessions planned`, etc.)
is `#64748b` slate which is just barely readable, and the row
divider is `#edf1f6` (almost white) — far too bright for a dark
card.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the Weekly Focus tile.
3. Inspect any `.focus-list strong` and read its computed
   `color`.

**Expected:** `rgb(255, 255, 255)` (`--cui-text-primary`) so the
label is legible.
**Actual:** `rgb(23, 32, 51)` — near-black on the dark surface.

Screenshot: `screenshots/dashboard-actual-1280.png` (the Move /
Read / Reflect rows fade into the tile).

## Fix outline (radically simple)

Three swaps in
`projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss`:

  - `.focus-list strong { color: var(--cui-text-primary); }`
  - `.focus-list span   { color: var(--cui-text-secondary); }`
  - `.focus-list li     { border-bottom: 1px solid var(--cui-divider); }`

No HTML or TS change.

The other dashboard tiles (`outstanding-todos`, `relations`,
`daily-results`) have the same class of bug and will be tracked
as their own bugs.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`weeklyFocusLabelColor()` and add a `dashboard.spec.ts` test
asserting `rgb(255, 255, 255)`. Currently fails with
`rgb(23, 32, 51)`.
