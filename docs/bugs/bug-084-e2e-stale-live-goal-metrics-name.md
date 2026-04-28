---
id: bug-084
title: e2e dashboard.spec.ts expects "Live Goal Metrics" — design-07 merged this tile with Review Goal History into one "Goal Metrics" tile, leaving the e2e stale
status: Fixed
---

# Bug 084 — e2e expects "Live Goal Metrics" after design-07 merge

**Status**: Fixed

## Fix

Replaced the stale `'Live Goal Metrics'` literal in
`dashboard.spec.ts:126` with `'Goal Metrics'` (matching the
component's `tileMetadata.displayName`), and refreshed the
comment block above the assertion: the standalone "Review Goal
History" tile no longer exists since design-07.

A new bug-084 cross-file regression spec on the goal-metrics
tile reads the e2e source and asserts no `Live Goal Metrics`
form remains — the same shape as bug-074 / bug-081 guards.

273/273 workspace tests green.

## Description

`60ad8f1 feat(goal-metrics): merge legacy live + review tiles
into single dual-mode tile (design 07)` collapsed two pre-merge
tiles ("Live Goal Metrics" + "Review Goal History") into a
single dual-mode `goal-metrics-tile` whose
`tileMetadata.displayName` is `'Goal Metrics'`.

`dashboard.spec.ts:126` still asserts the pre-merge label in the
add-tile catalog:

```ts
await expect(dashboard.addTileDialogLabels).toHaveText([
  'Daily Results',
  'Weekly Focus',
  'Monthly Progress',
  'Outstanding Todos',
  'Relations',
  'Consistency Trend',
  'Live Goal Metrics',     // ← stale; current tile renders 'Goal Metrics'
]);
```

When the e2e suite runs, the dialog will render `'Goal Metrics'`
and this assertion will fail.

The same comment block above the assertion also says
"Review Goal History has supportedModes: ['review'] and is
hidden here" — that stand-alone tile no longer exists either, so
the comment is misleading.

## Affected files

- `frontend/projects/commitments-app/e2e/dashboard.spec.ts` —
  replace `'Live Goal Metrics'` with `'Goal Metrics'`; refresh
  the comment to reflect the merged tile.
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.spec.ts` —
  add a cross-file regression spec (same shape as bug-074 / bug-081)
  that reads the e2e and asserts no `Live Goal Metrics` form remains.

## Reproduction

```bash
grep -n "Live Goal Metrics" frontend/projects/commitments-app/e2e/dashboard.spec.ts
```

Returns one stale match.

## Expected

- `dashboard.spec.ts` reads `'Goal Metrics'` (matching the
  component's metadata `displayName`).
- A cross-file unit guard prevents future drift.
- All existing goal-metrics specs continue to pass.

## Verification

- Unit: source-level grep on `dashboard.spec.ts` content asserts
  no `Live Goal Metrics` occurrence.
- All existing tests stay green.
