# UI Design Alignment

Audit date: 2026-04-27 (revised same day after `ui-design.pen` added Edit Mode + Add Tile Dialog frames)
Authoritative source: `C:\projects\Commitments\docs\ui-design.pen`

The implementation is **structurally different** from the design at the app-shell, header, sidenav, and add-tile-flow levels. The dashboard tile chrome and design tokens are mostly correct; the framing around the dashboard is not.

The design now also defines an explicit **Edit Mode** state and a redesigned **Add Tile Dialog** (tile-grid, not search/chips). Tasks 03, 06, and the new 11 cover that surface area.

Each task lives in its own file. They can mostly be done independently, but there is a natural ordering:

| # | Task | File | Depends on |
|---|---|---|---|
| 1 | App Shell — two-row Toolbar + PrimaryHeader | [01-app-shell-toolbar-primary-header.md](01-app-shell-toolbar-primary-header.md) | 7 (tokens) |
| 2 | Remove extraneous header buttons | [02-remove-extraneous-header-buttons.md](02-remove-extraneous-header-buttons.md) | 1 |
| 3 | Add Tile flow — FAB + Dialog (tile-grid) | [03-add-tile-fab-and-dialog.md](03-add-tile-fab-and-dialog.md) | 2 |
| 4 | Sidenav with active/hover states | [04-sidenav.md](04-sidenav.md) | 1, 7 |
| 5 | Dashboard page layout | [05-dashboard-page-layout.md](05-dashboard-page-layout.md) | 1, 4 |
| 6 | Tile chrome (default + editable) and tile styling | [06-tile-chrome-and-styling.md](06-tile-chrome-and-styling.md) | 7, 11 (for editable variant) |
| 7 | Design tokens — CSS custom properties | [07-design-tokens-css-vars.md](07-design-tokens-css-vars.md) | — |
| 8 | Typography consistency | [08-typography-consistency.md](08-typography-consistency.md) | 7 |
| 9 | Login page audit | [09-login-page-audit.md](09-login-page-audit.md) | — |
| 10 | Tests to update / add | [10-tests.md](10-tests.md) | 1–6, 11 |
| 11 | Edit Mode (pink header, EDIT MODE pill, DONE button, editable tile chrome) | [11-edit-mode.md](11-edit-mode.md) | 1, 5, 6 |

New design frames added 2026-04-27:
- `fJpM0` — Dashboard — LG (1280) — Edit Mode
- `a2Cjz` — Dashboard — LG (1280) — Add Tile Dialog
- `KB9Mx` — `Dashboard-Tile/Editable` reusable component
- `AhkGr` — the dialog frame inside `a2Cjz`

Shared design reference: [_reference.md](_reference.md) — design tokens, key node IDs, screenshots.
