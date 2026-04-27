# 01 — App Shell: replace `dashboard-shell` topbar with two-row Toolbar + PrimaryHeader

**Status: ACCEPTED**

**Design**: `App-Shell` (`8028h`) is composed of:
- Row 1: `Mat-Toolbar` (`xGF9T`) — `$toolbar` (#1F2233), height **64**, padding `[0, 16, 0, 8]`, gap 8, drop-shadow blur 6 y2 #00000099. Contents: hamburger `menu` icon (24×24, 40×40 button frame), brand text "Commitments" (Inter, `$fs-xl`/20, `$fw-medium`), spacer, profile name "Quinn Brown" (Inter, `$fs-body`/14, normal), avatar ellipse 40×40 fill `$primary-strong`.
- Row 2: `Mat-PrimaryHeader` (`ROIa0`) — `$primary-dim` (#3F51B5), height **80**, horizontal padding 20, single h2 page title "Page Title" (Inter, `$fs-h2`/28, `$fw-medium`, `$text-primary`).
- Row 3 (body): `Mat-Sidenav` (`1WUcZ`) on the left + content area on the right.

**Implementation**: `dashboard-shell.component.html/scss` flattens all of this into one wrap-flex topbar with eyebrow + h1 + mode toggle + tile picker + buttons. There is no separate primary-header row, no hamburger, no profile name, no avatar, no drop shadow.

## Tasks
- [ ] **1.1** Split `DashboardShellComponent` topbar into two stacked rows: a `Mat-Toolbar` row (height 64) and a `Mat-PrimaryHeader` row (height 80), with the body below.
- [ ] **1.2** Toolbar row background must be `var(--cui-toolbar)` (#1F2233); set `height: 64px`, `padding: 0 16px 0 8px`, `gap: 8px`, and apply `box-shadow: 0 2px 6px #00000099`.
- [ ] **1.3** Add a hamburger menu icon-button (Material Symbols Rounded `menu`, 24×24 in a 40×40 button) on the far left of the toolbar. It will toggle the sidenav (see [04-sidenav.md](04-sidenav.md)).
- [ ] **1.4** Brand text "Commitments" in the toolbar: Inter / 20px / 500 / `var(--cui-text-primary)`. Replace the current `dashboard-shell__eyebrow` (12 / 700 / uppercase).
- [ ] **1.5** Add a flex spacer, then a profile-name label (Inter 14 normal, `var(--cui-text-primary)`) and a 40×40 circular avatar (`var(--cui-primary-strong)` fill, `border-radius: 50%`, optional background-image of profile avatar) on the far right of the toolbar.
- [ ] **1.6** Remove the `dashboard-shell__eyebrow` + `<h1>Dashboard</h1>` from the toolbar — the page title belongs in the new PrimaryHeader row.
- [ ] **1.7** Move the page title ("Dashboard") into a second row using the existing `<app-primary-header>` (`PrimaryHeaderComponent`) — `$primary-dim` background, height 80, content "Dashboard". Keep the component contract: title comes from input or content projection.
- [ ] **1.8** `PrimaryHeaderComponent` font-family must be `Inter` (currently `Roboto, Arial`). Update `frontend/projects/commitments-ui/src/lib/primary-header/primary-header.component.scss` line 32 (`font: 500 28px/1.2 Roboto, Arial, sans-serif;` → `Inter, ...`).

See [_reference.md](_reference.md) for tokens and node IDs.
