---
id: bug-164
title: Remove final 6 dead shell-related UI components from @commitments/ui
status: Fixed
---

# Bug 164 — Drop final dead shell UI components (app-shell, dashboard-tile, sidenav, sidenav-item, toolbar, table-row)

**Status**: Fixed

## Fix

18 component files deleted (6 dirs × .ts/.html/.scss). 6 export
lines removed from `public-api.ts`. 410 lines net. 366/366
workspace tests green.

After this bug closes the bug-162 → 163 → 164 arc, the
`@commitments/ui` library exports only the 7 actively-consumed
components plus the design tokens module:

- TileShellComponent
- PrimaryHeaderComponent
- ModeToggleComponent
- StatusPillComponent
- IconButtonComponent
- MetricHeaderComponent
- DeltaBadgeComponent

The library is now ~60% smaller than before bug-162.

## Description

Final batch in the bug-162 → 163 → 164 cleanup arc. These six
components are exported from `@commitments/ui` but no consumer
in the workspace imports them:

| Component | Selector |
|-----------|----------|
| `AppShellComponent` | `cui-app-shell` |
| `DashboardTileComponent` | `cui-dashboard-tile` |
| `SidenavComponent` | `cui-sidenav` |
| `SidenavItemComponent` | `cui-sidenav-item` |
| `ToolbarComponent` | `cui-toolbar` |
| `TableRowComponent` | `cui-table-row` |

The `cui-toolbar` and `cui-sidenav` matches inside
`commitments-app/dashboard-layout.component.scss` are
references to the **CSS custom properties** `--cui-toolbar`
and `--cui-sidenav` (color tokens), not Angular selectors —
unrelated to the components.

## Affected files (18 + 6 export lines)

For each component, three files in
`frontend/projects/commitments-ui/src/lib/<dir>/`:
- `<dir>.component.ts`
- `<dir>.component.html`
- `<dir>.component.scss`

Plus 6 `export *` lines removed from `public-api.ts`.

## Reproduction

```bash
grep -rn 'cui-app-shell\|cui-dashboard-tile\|cui-sidenav-item\|cui-table-row' frontend/projects/commitments-app
```

Returns no matches. The bare `cui-sidenav` and `cui-toolbar`
matches are CSS custom property references, not selectors.

## Expected

- 18 component files removed.
- 6 lines removed from `public-api.ts`.
- After this bug, only the actively-used UI components remain
  (tile-shell, status-pill, icon-button, metric-header,
  delta-badge, mode-toggle, primary-header, plus the tokens
  module).
- Regression-guard spec asserts each directory's
  `.component.ts` no longer exists.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
