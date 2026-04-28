---
id: bug-156
title: TileContext.requestRefresh / refresh$ are unused — remove the dead refresh pipeline
status: Open
---

# Bug 156 — Drop dead `refresh$` + `requestRefresh()` from `TileContext`

## Description

`TileContext` declares two coupled members:

```ts
readonly refresh$: Observable<void>;
requestRefresh(): void;
```

`requestRefresh` is the producer; `refresh$` the consumer.
Cross-repo grep confirms zero production callers of
`requestRefresh()`:

```
$ grep -rn '\.requestRefresh(' frontend/projects --include='*.ts'
(no matches)
```

`bindTileMode` does subscribe to `context.refresh$.pipe(...)`,
but since nothing ever calls `requestRefresh`, the underlying
`Subject<void>` in `dashboard-grid.component.ts` never emits.
The subscription is a perpetually-quiet listener.

Same coupled-pair YAGNI shape as bug-151's `invalidations$` (a
hook for an ingestion path that was never wired). The
`refresh$` and `requestRefresh()` exist only to support each
other — drop them as a unit.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/tile-registration/tile.model.ts` (drop interface members + `Observable` import if unused)
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.ts` (drop the `Subject<void>` and the two associated context fields)
- `frontend/projects/dashboard-framework/src/lib/tile-registration/bind-tile-mode.ts` (drop the `context.refresh$` subscribe line)
- `frontend/projects/dashboard-framework/src/lib/tile-registration/bind-tile-mode.spec.ts` (drop the test that exercises `refresh$` emission, drop fixture stubs)

## Reproduction

```bash
grep -rn 'requestRefresh\|refresh\$' frontend/projects --include='*.ts'
```

Returns matches only inside the four files above — never an
external `requestRefresh()` invocation.

## Expected

- `TileContext` no longer declares `refresh$` or `requestRefresh`.
- `dashboard-grid` no longer creates the unused `Subject<void>`.
- `bindTileMode` no longer subscribes to `context.refresh$`.
- The bind-tile-mode spec drops the test for refresh$ emission.
- Regression-guard spec asserts the symbols are absent from
  `tile.model.ts`.

## Verification

- New regression spec confirms `tile.model.ts` does not contain
  `refresh$` or `requestRefresh`.
- All other tests continue to pass.
