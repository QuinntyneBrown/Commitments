# Application shell & navigation

## Summary

Every authenticated screen renders inside `DashboardLayoutComponent`: a fixed top toolbar (hamburger + brand text + spacer + profile name + circular avatar) and a left sidenav of the primary destinations, both above a `<router-outlet>`. The hamburger collapses/expands the sidenav (the sidenav is open by default on desktop). Each sidenav item is an anchor with `routerLink` + `routerLinkActive` and routes through Angular Router into the content area.

> **Implementation status:** the shell wraps `/` (the dashboard) and the catalog/CRUD routes (`/activities`, `/behaviours`, `/behaviour-types`, `/commitments`, `/cards`, `/card-layouts`, `/frequencies`, `/notes`, `/profiles`, `/to-dos`, plus `/my-profile` and `/settings`). Today the catalog routes resolve to `PlaceholderPageComponent` ("Coming soon") rather than the legacy ag-grid/Material catalog pages — see the individual catalog flow docs for the intended end-state. The legacy `MasterPageComponent` and `AnonymousMasterPageComponent` still exist in the source tree but are **not** routed by `app.routes.ts`.

## Surface area

- Active shell: `frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.{ts,html,scss}` (replaces the old `MasterPageComponent`).
- Routes: `frontend/projects/commitments-app/src/app/app.routes.ts` — declares `/login` (component) and `/` (DashboardLayoutComponent) with children `''` (DashboardShell) plus the placeholder paths listed above.
- Placeholder content: `components/placeholder-page/placeholder-page.component.ts` — derives a title from the route URL.
- Translation: `@ngx-translate/core` is no longer applied to the sidenav labels; the labels are hard-coded English strings in `DashboardLayoutComponent.navItems`. (The catalog pages continue to use translations once they exist.)

## Preconditions

- User is authenticated.

## Steps

1. **Toolbar renders on every authenticated route.**
   - Visit `/`, `/commitments`, `/notes`, `/to-dos` in turn.
   - **Assert:** on each route, the toolbar (`dashboard-layout-toolbar`) is visible with: hamburger button on the left, `Commitments` brand text, spacer, profile name `Quinn Brown`, and a 40×40 circular avatar slot on the right.

2. **Sidenav is open by default; hamburger toggles it.**
   - Land on `/`.
   - **Assert:** the sidenav (`dashboard-sidenav`) is visible (250px wide, `--cui-sidenav` background) without any user interaction.
   - Click `dashboard-layout-hamburger`.
   - **Assert:** the sidenav collapses (gets the `--collapsed` modifier — width 0).
   - Click the hamburger again.
   - **Assert:** the sidenav expands again.

3. **Sidenav items in design order.**
   - With the sidenav open, observe the items.
   - **Assert:** items render in this order, each with a Material Symbols Rounded icon and the label below:
     | Label | Icon | Route |
     | --- | --- | --- |
     | Dashboard | `dashboard` | `/` |
     | Activities | `directions_run` | `/activities` |
     | Behaviours | `psychology` | `/behaviours` |
     | Behaviour Types | `category` | `/behaviour-types` |
     | Commitments | `task_alt` | `/commitments` |
     | Cards | `style` | `/cards` |
     | Card Layouts | `dashboard_customize` | `/card-layouts` |
     | Frequencies | `schedule` | `/frequencies` |
     | Notes | `sticky_note_2` | `/notes` |
     | Profiles | `group` | `/profiles` |
     | To Do's | `checklist` | `/to-dos` |
     | Logout | `logout` | `/login` |

4. **Each link navigates and applies active state.**
   - Click each sidenav item.
   - **Assert:** the URL updates to the corresponding route; the clicked item gains the `sidenav-item--active` class (driven by `routerLinkActive`); the active item shows the indigo left border and primary-coloured icon/label per the design tokens. The `Dashboard` item is matched with `[routerLinkActiveOptions]="{ exact: true }"` so it is only active on `/`.

5. **Router outlet swaps content.**
   - Navigate from `/` to `/commitments`.
   - **Assert:** the dashboard shell unmounts from the content area; the placeholder-page renders with a `Commitments` PrimaryHeader and a "Coming soon." message. Reverse the navigation to confirm the dashboard shell remounts.

6. **Sidenav stays open across navigation.**
   - With the sidenav open, click any item.
   - **Assert:** the sidenav remains open (the new shell does **not** auto-close on item click; this differs from the legacy `MasterPageComponent` which had `(click)="drawer.close()"`).

## Selectors

| Need | Selector |
| --- | --- |
| App-shell root | `getByTestId('dashboard-layout')` |
| Toolbar | `getByTestId('dashboard-layout-toolbar')` |
| Hamburger button | `getByTestId('dashboard-layout-hamburger')` |
| Profile name | `getByTestId('dashboard-layout-profile-name')` |
| Profile avatar | `getByTestId('dashboard-layout-avatar')` |
| Sidenav | `getByTestId('dashboard-sidenav')` |
| Sidenav item (each) | `getByTestId('sidenav-item-<label>')` (e.g. `sidenav-item-Dashboard`, `sidenav-item-To Do's`) |
| Active sidenav item (style hook) | `.sidenav-item--active` |
| Page heading on placeholder routes | `app-primary-header h1` |

## Edge cases

- Narrow viewport (<576px) — the sidenav still toggles via the hamburger but its 250px width pushes the content area; if the design later specifies an overlay/off-canvas behaviour at small widths, update this section.
- Browser back/forward — should not change the sidenav's open/closed state (the signal is component-local).
- The legacy `MasterPageComponent` still ships in the bundle but is unrouted; flows that reference `mat-sidenav` selectors should be updated to use the new testids above.
