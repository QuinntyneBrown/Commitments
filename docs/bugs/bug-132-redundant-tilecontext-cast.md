---
id: bug-132
title: All 7 tile components cast `inject(TILE_CONTEXT, { optional: true })` to `TileContext | null` redundantly
status: Fixed
---

# Bug 132 — redundant TileContext cast

**Status**: Fixed

## Fix

Removed the `as TileContext | null` cast from each of the 7
tile components. Angular's `inject(TILE_CONTEXT, { optional: true })`
already returns `TileContext | null` via the standard overload,
so the cast was no-op clutter.

The now-unused `TileContext` import was also dropped from each
file's `@commitments/dashboard-framework` import list. 328/328
workspace tests green — refactor only, no behaviour change.

## Description

Every tile component injects `TILE_CONTEXT` like this:

```ts
const context = inject(TILE_CONTEXT, { optional: true }) as TileContext | null;
```

Angular's `inject()` overload with `{ optional: true }` already
returns `T | null`. The explicit `as TileContext | null` adds
nothing and clutters the call site.

The token is `export const TILE_CONTEXT = new InjectionToken<TileContext>('TILE_CONTEXT')`,
so `inject(TILE_CONTEXT, { optional: true })` is already typed
as `TileContext | null` by the standard overload.

Same pattern in 7 files:
- consistency-trend-tile, daily-results-tile, goal-metrics-tile,
  monthly-progress-tile, outstanding-todos-tile, relations-tile,
  weekly-focus-tile

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/*/...-tile.component.ts` (7 files)

## Reproduction

```bash
grep -rn "as TileContext" frontend/projects/commitments-dashboard-plugin/src/lib/tiles --include="*.ts" | grep -v spec
```

Returns 7 matches.

## Expected

The cast is removed; inject() returns the proper type
directly.

## Verification

- Source-level grep returns no matches after the fix.
- All existing tile specs continue to pass — refactor only.
