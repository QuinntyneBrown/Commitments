---
id: bug-159
title: TileContext.tileId / instanceId are unused — drop both
status: Fixed
---

# Bug 159 — Drop dead `TileContext.tileId` / `TileContext.instanceId`

**Status**: Fixed

## Fix

6 lines removed across 3 files. TileContext is now a clean
two-signal interface:

```ts
export interface TileContext {
  readonly mode: Signal<DashboardMode>;
  readonly selectedReviewDate: Signal<string | null>;
}
```

361/361 workspace tests green.

## Description

After the bug-152/156/157/158 cleanup arc, `TileContext` is:

```ts
export interface TileContext {
  readonly tileId: string;
  readonly instanceId: string;
  readonly mode: Signal<DashboardMode>;
  readonly selectedReviewDate: Signal<string | null>;
}
```

Cross-repo grep confirms the two identification fields are
never read from a TileContext anywhere — neither tile
components, framework helpers, tests, nor the dashboard host
access `_tileContext.tileId` or `_tileContext.instanceId`.

The `tileId`/`instanceId` ARE used widely on `DashboardItem`
(layout records) and `TileMetadata` (registry lookups), but
those are different surfaces — not the per-tile context.

Drop the two unused context fields. After this removal,
TileContext is just a `mode` + `selectedReviewDate` pair —
which is exactly what the framework's `bindTileMode` reads.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/tile-registration/tile.model.ts` (drop two fields)
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.ts` (drop two stub fields)
- `frontend/projects/dashboard-framework/src/lib/tile-registration/bind-tile-mode.spec.ts` (drop two fixture stubs)

## Reproduction

```bash
grep -rn '_tileContext\.tileId\|_tileContext\.instanceId\|context\.tileId\|context\.instanceId' frontend/projects --include='*.ts'
```

Returns no matches.

## Expected

- TileContext narrows to just `mode` + `selectedReviewDate`.
- Regression-guard spec asserts neither field is declared in
  `tile.model.ts`'s `TileContext`.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
