---
id: bug-146
title: Legacy DashboardPageComponent (cardId-based) is unused — delete
status: Open
---

# Bug 146 — Remove unused legacy `DashboardPageComponent`

## Description

`commitments-app/src/app/pages/dashboard/dashboard-page/`
contains a legacy `DashboardPageComponent` that uses the
old cardId integer system (1-4, 6) and a `ViewContainerRef`-
based component creation pattern. It was superseded by the
modern `DashboardShellComponent` from `@commitments/dashboard-framework`,
which is what the root `''` route actually renders:

```ts
// app.routes.ts
{ path: '', component: DashboardShellComponent },
```

Cross-repo grep confirms zero references to the legacy
component anywhere except inside its own three files
(component, template, stylesheet):

```
$ grep -rn 'DashboardPageComponent\|app-dashboard-page' frontend/projects
projects/commitments-app/src/app/pages/dashboard/dashboard-page/dashboard-page.component.ts:28:  selector: 'app-dashboard-page',
…(only the file's own definition lines)
```

No route binds to it, no other component imports it, no test
imports it. It is genuinely dead code carrying along a long
list of imports (5 specific dashboard card components, 4
services, 2 dialog services, the legacy `Dashboard` model,
etc.) that other bug iterations can address as their cascade
becomes clear.

This iteration removes JUST the page directory itself. The
orphaned imports become candidates for follow-up cleanup once
their now-removed-from-here references are visible.

## Affected files

- `frontend/projects/commitments-app/src/app/pages/dashboard/dashboard-page/dashboard-page.component.ts` (deleted)
- `frontend/projects/commitments-app/src/app/pages/dashboard/dashboard-page/dashboard-page.component.html` (deleted)
- `frontend/projects/commitments-app/src/app/pages/dashboard/dashboard-page/dashboard-page.component.scss` (deleted)

## Reproduction

```bash
grep -rn 'DashboardPageComponent\|app-dashboard-page' frontend/projects --include='*.ts' --include='*.html'
```

Returns matches only inside the file's own definition.

## Expected

The three legacy files are removed. The `pages/dashboard/`
parent directory may still hold other dashboard-related
content (or be empty after this) — leave any such cleanup for
a follow-up bug.

## Verification

- A new spec asserts the `dashboard-page.component.ts` file
  does not exist (filesystem check).
- All existing workspace tests continue to pass.
- The legacy `app-dashboard-page` selector returns zero hits
  in a grep across `frontend/projects`.
