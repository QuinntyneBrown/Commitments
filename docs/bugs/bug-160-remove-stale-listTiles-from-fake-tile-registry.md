---
id: bug-160
title: FakeTileRegistry in dashboard-layout.store.spec carries stale listTiles() — remove
status: Open
---

# Bug 160 — Drop stale `listTiles()` from `FakeTileRegistry`

## Description

`dashboard-layout.store.spec.ts` defines a `FakeTileRegistry`
test double that mirrors the real `TileRegistryService` API.
It still declares a `listTiles(): TileDescriptor[]` method —
which the real service lost in bug-154 (the method shadowed
the `tiles` signal and was removed). The fake was never updated
to match.

The fake's `listTiles()` is not called by any test in the file
(I grepped). It is dead surface on the test fake, leftover
from before bug-154's cleanup.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-layout.store.spec.ts`

## Reproduction

```bash
grep -n 'listTiles' frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-layout.store.spec.ts
```

Returns the declaration line. No call site.

## Expected

The 3-line method declaration is removed from `FakeTileRegistry`.
A regression-guard spec asserts the symbol does not reappear.

## Verification

- New regression spec confirms the fake no longer declares
  `listTiles`.
- All other tests continue to pass.
