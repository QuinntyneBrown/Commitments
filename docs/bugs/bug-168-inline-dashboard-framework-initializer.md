---
id: bug-168
title: dashboard-framework.initializer.ts is single-consumer indirection — inline into provideDashboardFramework
status: Fixed
---

# Bug 168 — Inline `dashboard-framework.initializer` into `provideDashboardFramework`

**Status**: Fixed

## Fix

The 21-line `dashboard-framework.initializer.ts` file deleted.
Its factory + Provider inlined directly into
`provideDashboardFramework`. The dashboard barrel's stale
re-export removed. 368/368 workspace tests green.

## Description

`dashboard-framework/src/lib/dashboard/dashboard-framework.initializer.ts`
exports two symbols:

- `dashboardFrameworkInitializerFactory(bootstrapper, store): () => void`
- `DASHBOARD_FRAMEWORK_INITIALIZER: Provider`

The factory is referenced only by the Provider in the same
file. The Provider is referenced only by
`provide-dashboard-framework.ts`:

```ts
export function provideDashboardFramework(): Provider[] {
  return [DASHBOARD_FRAMEWORK_INITIALIZER];
}
```

This is a one-step indirection that doesn't carry its
weight: the entire factory + Provider can be inlined into
`provideDashboardFramework`, deleting the initializer file.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-framework.initializer.ts` (delete)
- `frontend/projects/dashboard-framework/src/lib/provide-dashboard-framework.ts` (inline the Provider)
- `frontend/projects/dashboard-framework/src/lib/dashboard/index.ts` (drop the initializer re-export)

## Reproduction

```bash
grep -rn 'DASHBOARD_FRAMEWORK_INITIALIZER\|dashboardFrameworkInitializerFactory' frontend/projects --include='*.ts'
```

Returns matches only inside the initializer file and its one
consumer.

## Expected

- `dashboard-framework.initializer.ts` removed.
- `provideDashboardFramework` declares the APP_INITIALIZER
  Provider inline.
- The dashboard barrel `index.ts` no longer re-exports the
  initializer file.
- Regression-guard spec asserts the initializer file no
  longer exists.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
