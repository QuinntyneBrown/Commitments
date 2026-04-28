---
id: bug-158
title: TileContext.remove() and isEditMode are unused — drop both
status: Fixed
---

# Bug 158 — Drop dead `TileContext.remove()` and `TileContext.isEditMode`

**Status**: Fixed

## Fix

7 lines removed across 3 files. After this cleanup, TileContext
is just `tileId`, `instanceId`, `mode`, `selectedReviewDate` —
exactly what the framework's reactive bindings actually
consume. 360/360 workspace tests green.

## Description

Following bugs 152/156/157 narrowing the `TileContext` surface,
two more members are dead:

- **`remove(): void`** — zero callers. The dashboard-grid
  template's "remove tile" button calls `removeTile(item.instanceId)`
  on the component, which delegates to `layoutStore.removeTile`
  directly. Tile components never invoke
  `context.remove()` themselves.

- **`isEditMode: Signal<boolean>`** — exposed in TileContext but
  no consumer reads it. Both the dashboard-shell template and
  dashboard-grid template read `layoutStore.isEditMode()`
  directly via injection, not through the per-tile context.

Same shape as the earlier dead-context cleanups (152, 156, 157).
Drop both as a unit.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/tile-registration/tile.model.ts` (drop two interface members)
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.ts` (drop the two stub fields)
- `frontend/projects/dashboard-framework/src/lib/tile-registration/bind-tile-mode.spec.ts` (drop the two fixture stubs)

## Reproduction

```bash
grep -rn '\.remove()\|\.isEditMode\b' frontend/projects --include='*.ts'
```

For `.remove()`: returns matches only on store/registry calls
unrelated to TileContext. For `.isEditMode`: returns matches
only on `layoutStore.isEditMode` (not via TileContext).

## Expected

- TileContext narrows to: tileId, instanceId, mode,
  selectedReviewDate.
- Regression-guard spec asserts `remove(` and `isEditMode` are
  absent from `tile.model.ts`.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
