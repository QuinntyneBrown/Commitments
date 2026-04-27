# Application shell & navigation

## Summary

Every authenticated screen renders inside `MasterPageComponent`: a top toolbar (hamburger + app title + profile name + avatar) and a left sidenav with the primary destinations. The sidenav opens on toolbar click, closes on item click, and routes through Angular Router into a `<router-outlet>` in the main content area. Every primary destination is reachable from the sidenav.

## Surface area

- `components/master-page/master-page.component.{ts,html,scss}`
- `app.routes.ts` — declares the routes referenced from the sidenav (`/`, `/activities`, `/behaviours`, `/behaviour-types`, `/commitments`, `/cards`, `/card-layouts`, `/frequencies`, `/notes/create`, `/profiles`, `/to-dos`, `/login`).
- Translation: `@ngx-translate/core` — every visible label is translated.

## Preconditions

- User is authenticated.
- Translations for the active culture are loaded.

## Steps

1. **Toolbar renders on every page.**
   - Visit `/` (dashboard), `/commitments`, `/notes/create`, `/to-dos` in turn.
   - **Assert:** on each route, the toolbar is visible with `Commitments` title, a hamburger icon button on the left, and the profile name + avatar on the right.

2. **Sidenav opens.**
   - From any page, click the toolbar hamburger.
   - **Assert:** the `<mat-sidenav>` becomes visible (in DOM with `mode="side"`) and shows links: `Dashboard`, `Activities`, `Behaviours`, `Behaviour Types`, `Commitments`, `Cards`, `Card Layouts`, `Frequencies`, `Notes`, `Profiles`, `To Do's`, `Logout`.

3. **Sidenav closes after a click.**
   - Click any sidenav link (e.g. `Commitments`).
   - **Assert:** sidenav closes (its `(click)="drawer.close()"` handler), and the URL updates to the corresponding route.

4. **Each link navigates correctly.**
   - For each link, open the sidenav and click it.
   - **Assert:**
     | Link | URL |
     | --- | --- |
     | Dashboard | `/` |
     | Activities | `/activities` |
     | Behaviours | `/behaviours` |
     | Behaviour Types | `/behaviour-types` |
     | Commitments | `/commitments` |
     | Cards | `/cards` |
     | Card Layouts | `/card-layouts` |
     | Frequencies | `/frequencies` |
     | Notes | `/notes/create` |
     | Profiles | `/profiles` |
     | To Do's | `/to-dos` |
     | Logout | `/login` |

5. **Router outlet swaps content.**
   - Navigate from `/commitments` to `/to-dos`.
   - **Assert:** the page heading text changes from `Commitments` to `To Do's`; the previous route's grid is gone from the DOM.

## Selectors

| Need | Selector |
| --- | --- |
| Hamburger button | `mat-toolbar button[mat-icon-button]` containing `mat-icon` `menu` |
| Sidenav drawer | `mat-sidenav` (or `<mat-sidenav>`) |
| Sidenav link | `mat-sidenav button[mat-button]:has-text("<label>")` |
| Page heading | `app-primary-header h1` (catalog pages all use this) |

> **Add `data-testid="sidenav"` on the drawer and `data-testid="nav-<key>"` on each sidenav button** to make this flow stable.

## Edge cases

- Narrow viewport (XS, <576px) — sidenav should still open and overlay content (today it uses `mode="side"`; behaviour at small widths needs verification).
- RTL languages — labels are translated; ordering of name + avatar should still be readable.
- Browser back/forward — should not reopen the sidenav.
