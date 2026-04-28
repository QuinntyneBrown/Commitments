---
id: bug-153
title: DashboardLayoutStore.resetLayout/toggleEditMode + POM resetLayoutButton are dead — remove
status: Open
---

# Bug 153 — Drop dead `resetLayout()`, `toggleEditMode()`, POM `resetLayoutButton`

## Description

Three related dead surfaces in the dashboard-layout system:

1. `DashboardLayoutStore.resetLayout(): void` — declared in the
   store but never called. There is no UI button that triggers
   it.

2. `DashboardLayoutStore.toggleEditMode(): void` — also declared
   but never called. The dashboard-shell uses
   `setEditMode(true)` / `setEditMode(false)` directly via two
   separate methods.

3. `e2e/pages/dashboard.page.ts` declares `resetLayoutButton:
   Locator` and assigns `page.getByTestId('reset-layout')`, but
   no e2e test ever interacts with it — and the dashboard-shell
   template doesn't render a `reset-layout` testid anywhere.

Same YAGNI shape as bugs 144/145/151/152: forward-looking
surfaces never wired. Drop them.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-layout.store.ts` (drop two methods)
- `frontend/projects/commitments-app/e2e/pages/dashboard.page.ts` (drop POM locator)

## Reproduction

```bash
grep -rn 'resetLayout\b\|toggleEditMode\b' frontend/projects --include='*.ts'
```

Returns matches only at the declaration sites, plus the POM
locator that is never used by any test.

## Expected

- `DashboardLayoutStore` no longer declares `resetLayout` or
  `toggleEditMode`.
- `DashboardPage` POM no longer declares `resetLayoutButton`.
- Regression-guard spec asserts the symbols don't reappear.

## Verification

- New regression spec confirms the source no longer contains
  `resetLayout(` or `toggleEditMode(` in the store, nor
  `resetLayoutButton` in the POM.
- All existing workspace tests continue to pass.
