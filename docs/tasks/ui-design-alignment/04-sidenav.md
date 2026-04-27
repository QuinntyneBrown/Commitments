# 04 — Sidenav: add left sidebar with active/hover states

**Status: COMPLETE**

> Already implemented in `frontend/projects/commitments-app/src/app/components/dashboard-layout/`. Width 250, `--cui-sidenav` background, padding-top 20, right border `--cui-divider`, hamburger toggle (4.1); 12 nav items in design order (4.2); height 48 / padding 16 / gap 16 / Material Symbols 20px / Inter 14 (4.3); active state `#9FA8DA22` + 3px left `--cui-primary` (4.4); hover `--cui-hover-overlay` (4.5); icons match the specified set (4.6); every nav route wired in `app.routes.ts` (4.7).

**Design**: `Mat-Sidenav` (`1WUcZ`), width **250**, background `$sidenav` (#181A24), padding `[20, 0, 0, 0]`, right border 1px `$divider`. Items use:
- `Mat-SidenavItem` (`ZOXJ0`): height 48, padding `[0, 16]`, gap 16, transparent background. Icon (Material Symbols Rounded, 20×20, `$text-secondary`) + label (Inter, `$fs-body`/14, normal, `$text-primary`).
- `Mat-SidenavItem/Active` (`XFQ3C`): same dimensions, fill `#9FA8DA22` (translucent primary), 3-px left border `$primary`, icon + label both in `$primary`, label weight `$fw-medium`.
- `Mat-SidenavItem/Hover` (`DtboV`): fill `$hover-overlay` (#FFFFFF14).

The expected items (from `master-page.component.html` and design appearance in `OxYKj`): Dashboard, Activities, Behaviours, Behaviour Types, Commitments, Cards, Card Layouts, Frequencies, Notes, Profiles, To Do's, Logout.

**Implementation**: `DashboardShellComponent` has no sidenav. The legacy `MasterPageComponent` has a `mat-sidenav` but it is not used by `app.routes.ts`.

## Tasks
- [ ] **4.1** Add a left-anchored sidebar to `DashboardShellComponent` (or extract a `DashboardLayoutComponent` that wraps the toolbar + sidebar + main content). Width 250, background `var(--cui-sidenav)`, top padding 20, right border `1px solid var(--cui-divider)`. Toggle visibility via the toolbar hamburger ([01.3](01-app-shell-toolbar-primary-header.md)); on desktop default to expanded.
- [ ] **4.2** Render nav items in this order: **Dashboard**, **Activities**, **Behaviours**, **Behaviour Types**, **Commitments**, **Cards**, **Card Layouts**, **Frequencies**, **Notes**, **Profiles**, **To Do's**, **Logout**. Each item is a `routerLink` to its corresponding route.
- [ ] **4.3** Each item: height 48, horizontal padding 16, gap 16 between icon and label, Material Symbols Rounded icon 20×20, label Inter / 14 / normal, `var(--cui-text-primary)`.
- [ ] **4.4** Active state (driven by `routerLinkActive`): background `#9FA8DA22`, 3-px left border `var(--cui-primary)`, icon + label color `var(--cui-primary)`, label weight 500.
- [ ] **4.5** Hover state: background `var(--cui-hover-overlay, #FFFFFF14)`. (Add a `--cui-hover-overlay` token to `_tokens.scss` to back this — see [07.6](07-design-tokens-css-vars.md).)
- [ ] **4.6** Pick icons that match the design intent (e.g. Dashboard → `dashboard`, Activities → `event_available`, Behaviours → `repeat`, Behaviour Types → `category`, Commitments → `task_alt`, Cards → `style`, Card Layouts → `dashboard_customize`, Frequencies → `schedule`, Notes → `description`, Profiles → `person`, To Do's → `format_list_bulleted`, Logout → `logout`). Verify against `OxYKj` and `JX39X` frames; the design's `Mat-SidenavItem` examples use `dashboard` (default), `task_alt` (active Commitments), `format_list_bulleted` (To Do's).
- [ ] **4.7** Add the missing routes to `app.routes.ts` so each sidenav item resolves to a real component (Activities, Behaviours, Behaviour Types, Cards, Card Layouts, Frequencies, Notes, Profiles, To Do's, My Profile, Settings). Components for most of these already exist under `frontend/projects/commitments-app/src/app/pages/`. Use lazy `loadComponent` to keep bundle size in check.

See [_reference.md](_reference.md) for tokens and node IDs.
