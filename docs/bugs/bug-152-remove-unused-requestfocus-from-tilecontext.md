---
id: bug-152
title: TileContext.requestFocus() is unused — drop from interface and stubs
status: Fixed
---

# Bug 152 — Drop unused `requestFocus()` from `TileContext`

**Status**: Fixed

## Fix

Three small edits — interface member dropped, dashboard-grid
stub removed, bind-tile-mode spec fixture updated. 355/355
workspace tests green. The next iteration's regression-guard
asserts the symbol does not regrow in `tile.model.ts`.

## Description

`TileContext` declares a `requestFocus(): void` method that
is **never called**. Cross-repo grep:

```
grep -rn '\.requestFocus(' frontend/projects --include='*.ts'
```

Returns no matches. The only references are:

1. The interface declaration in `tile.model.ts`
2. A no-op stub `requestFocus: () => undefined` in
   `dashboard-grid.component.ts` (the canonical TileContext
   provider)
3. A no-op stub `requestFocus: () => {}` in
   `bind-tile-mode.spec.ts` test fixture

No tile component, framework consumer, or test ever invokes
`requestFocus()`. Same YAGNI shape as bugs 144/145/151 — a
forward-looking surface scaffolded but never wired. Drop it.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/tile-registration/tile.model.ts` (drop interface member)
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.ts` (drop stub)
- `frontend/projects/dashboard-framework/src/lib/tile-registration/bind-tile-mode.spec.ts` (drop fixture stub)

## Reproduction

```bash
grep -rn 'requestFocus' frontend/projects --include='*.ts'
```

Returns three matches — all definitions/stubs, never an
invocation.

## Expected

- `TileContext` no longer declares `requestFocus`.
- The two stubs are removed.
- A regression-guard spec asserts the interface no longer
  contains `requestFocus`.

## Verification

- New regression spec confirms `tile.model.ts` does not
  declare `requestFocus`.
- All existing workspace tests continue to pass.
