---
id: bug-151
title: Remove unused `invalidations$` plumbing — bindTileMode option + TileInvalidationService
status: Fixed
---

# Bug 151 — Drop unused tile-invalidation pipeline

**Status**: Fixed

## Fix

153 lines deleted across 4 files:

- `bind-tile-mode.ts`: drop `invalidations$` field from
  `BindTileModeOptions`, drop the corresponding subscribe
  line, drop the now-unused `Observable` import.
- `bind-tile-mode.spec.ts`: drop the 1 test exercising the
  unused option.
- `tile-invalidation.service.ts`: deleted.
- `tile-invalidation.service.spec.ts`: deleted.

Test count drops from 359 → 354 (−1 from bind-tile-mode, −5
from tile-invalidation suite, +1 regression guard). 354/354
workspace tests green.

## Description

`BindTileModeOptions` exposes an optional `invalidations$:
Observable<unknown>` that, when provided, retriggers
`load(mode, asOf)` on each emit. The complement is
`TileInvalidationService` — a SignalR-fed stream of
`DashboardTileDataInvalidatedPayload` events keyed by
dataset ('dailyResults' | 'weeklyFocus' | …).

Cross-repo grep confirms **no tile** ever calls
`bindTileMode({ invalidations$ })`, and **no consumer** ever
calls `TileInvalidationService.invalidations$('…')`. The only
references are:

- The interface declaration in `bind-tile-mode.ts`
- One test in `bind-tile-mode.spec.ts` exercising the unused option
- The service definition in `tile-invalidation.service.ts`
- Its own spec file

Same YAGNI shape as bug-145 (`applyHubUpdate`) — a SignalR
hook that was scaffolded but never wired to any production
caller. The service's hub subscription, the dataset enum, and
the payload interface all exist purely to serve a dead path.
Drop them.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/tile-registration/bind-tile-mode.ts` (drop `invalidations$` field + `subscribe` line)
- `frontend/projects/dashboard-framework/src/lib/tile-registration/bind-tile-mode.spec.ts` (drop the 1 test)
- `frontend/projects/commitments-app/src/app/dashboard-tiles/tile-invalidation.service.ts` (delete)
- `frontend/projects/commitments-app/src/app/dashboard-tiles/tile-invalidation.service.spec.ts` (delete)

After deleting the service files, the `dashboard-tiles/`
directory becomes empty — remove it too.

## Reproduction

```bash
grep -rn 'invalidations\$\|TileInvalidationService' frontend/projects --include='*.ts'
```

Returns matches only inside the four files listed above. No
production caller anywhere.

## Expected

- `BindTileModeOptions` has no `invalidations$` field.
- `bindTileMode` body does not subscribe to invalidations.
- `tile-invalidation.service.ts` and its spec are gone.
- A regression-guard spec asserts the service file no longer
  exists and that `BindTileModeOptions` does not declare the
  property.

## Verification

- New regression spec confirms each removal.
- All other tests continue to pass.
- The bind-tile-mode spec drops from 7 tests to 6.
