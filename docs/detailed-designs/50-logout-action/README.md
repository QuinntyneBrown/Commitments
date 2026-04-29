# Design 50 — Logout Action

Status: Accepted

## Context

The dashboard sidenav "Logout" item uses `[routerLink]="/login"` but never clears the
access token from localStorage. After clicking Logout, the user lands on the login page
but still has a valid token. Navigating back to `/` passes the authGuard and skips login.

## Design

Remove "Logout" from the `navItems` array (navigation links) and add a dedicated
`logout()` method + button in the template. The button is semantically an action,
not a route link.

### `DashboardLayoutComponent`

```ts
private readonly _router = inject(Router);

protected logout(): void {
  localStorage.removeItem('accessTokenKey');
  this._router.navigate(['/login']);
}
```

`navItems` — remove the `{ label: 'Logout', icon: 'logout', routerLink: '/login' }` entry.

### Template — after the `@for` nav items loop

```html
<button
  class="sidenav-item"
  type="button"
  data-testid="sidenav-item-Logout"
  (click)="logout()"
>
  <span class="material-symbols-rounded sidenav-item__icon">logout</span>
  <span class="sidenav-item__label">Logout</span>
</button>
```

## Acceptance tests

| File | Test |
|------|------|
| `dashboard-layout.component.spec.ts` | `logout()` removes accessTokenKey from localStorage |
| `dashboard-layout.component.spec.ts` | `logout()` navigates to `/login` |
| `dashboard-layout.component.spec.ts` | navItems does not contain a Logout route entry |

## Implementation steps

1. Add failing tests
2. Commit + push
3. Update component and template
4. Verify → commit + push
