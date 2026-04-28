---
id: bug-167
title: LayoutPersistenceService leaks into dashboard-framework public API
status: Fixed
---

# Bug 167 — Hide internal `LayoutPersistenceService` from public-api

**Status**: Fixed

## Fix

One re-export line deleted from
`dashboard-framework/src/lib/dashboard/index.ts`. The service
file remains in place — just no longer published as part of
the framework's public API. 367/367 workspace tests green.

## Description

`dashboard-framework/src/lib/dashboard/index.ts` re-exports
`layout-persistence.service.ts`, but the service is consumed
only inside the framework's own folder — by
`DashboardLayoutStore` (via injection) and the two specs
(layout-persistence + dashboard-layout.store).

```bash
$ grep -rn 'LayoutPersistenceService' frontend/projects --include='*.ts'
```

…returns matches only inside the dashboard folder. No
external consumer.

Same shape as bug-166: drop the re-export to narrow the
public surface. The service file stays — it's just no longer
published.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/index.ts` (drop one re-export)

## Expected

- `index.ts` no longer re-exports `layout-persistence.service`.
- Regression-guard spec asserts the re-export is gone.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
