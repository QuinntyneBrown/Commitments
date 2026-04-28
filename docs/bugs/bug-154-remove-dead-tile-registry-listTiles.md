---
id: bug-154
title: TileRegistryService.listTiles() is dead — replaced by the existing `tiles` signal
status: Fixed
---

# Bug 154 — Drop unused `TileRegistryService.listTiles()`

**Status**: Fixed

## Fix

Removed the 3-line method from the service. Updated the one
spec line that called `registry.listTiles()` to use
`registry.tiles()` (the existing readonly signal). 358/358
workspace tests green.

## Description

`TileRegistryService` exposes both:

```ts
readonly tiles: Signal<TileDescriptor[]> = this.descriptors.asReadonly();

listTiles(): TileDescriptor[] {
  return this.descriptors();
}
```

`tiles` is the canonical signal-based readonly accessor and is
used by `DashboardLayoutStore`, `AddTileDialogComponent`, etc.
`listTiles()` is a method that returns the same data via a
function call. Cross-repo grep confirms `listTiles()` is
called by exactly one location: a single line in
`tile-registry.service.spec.ts` that could trivially read
`registry.tiles()` instead.

Same YAGNI shape as the other dead surfaces removed in bugs
144/145/151/152/153 — when a method exists only because tests
exercise it, drop the method and update the test to read the
real production signal.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/tile-registration/tile-registry.service.ts` (drop `listTiles` method)
- `frontend/projects/dashboard-framework/src/lib/tile-registration/tile-registry.service.spec.ts` (swap one `.listTiles()` call to `.tiles()`)

## Reproduction

```bash
grep -rn '\.listTiles\(\)' frontend/projects --include='*.ts'
```

Returns one match — the spec line.

## Expected

- `listTiles()` is removed from the service.
- The spec uses `.tiles()` instead.
- Regression-guard spec asserts `listTiles` is absent from the
  service source.

## Verification

- New regression spec confirms the source no longer declares
  `listTiles(`.
- All existing tests continue to pass.
