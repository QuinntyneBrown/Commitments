# Design 48 — Dashboard Layout Profile Name

Status: Accepted

## Context

The `DashboardLayoutComponent` toolbar shows the hardcoded string "Quinn Brown" as the
profile name. After design 47, the auth flow is complete — the user is logged in and the
`ProfileService` in `@commitments/identity-feature` can fetch the current profile.

## Goal

Replace the hardcoded name with a `profileName` signal populated on `ngOnInit` via
`ProfileService.current()`.

## Design

### `DashboardLayoutComponent`

```ts
export class DashboardLayoutComponent implements OnInit {
  private readonly _profileService = inject(ProfileService);

  protected readonly sidenavOpen = signal(true);
  protected readonly profileName = signal('');

  ngOnInit() {
    this._profileService.current().then(r => this.profileName.set(r.profile.name));
  }
  // toggleSidenav unchanged
}
```

### Template

```html
<span class="dashboard-layout__profile-name" data-testid="dashboard-layout-profile-name">
  {{ profileName() }}
</span>
```

## Acceptance tests

The existing spec uses `new DashboardLayoutComponent()` which breaks once `inject()` is
added. Convert all component-instantiation tests to use `runInInjectionContext` with a
mock `ProfileService`.

| Test | Expected |
|------|----------|
| starts with the sidenav open | `sidenavOpen()` is `true` |
| toggleSidenav flips the signal | toggles correctly |
| loads profile name on ngOnInit | `profileName()` equals fixture name after init |
| profileName starts empty | `profileName()` is `''` before init |

## Implementation steps

1. Write failing tests (update existing spec + new tests)
2. Commit + push
3. Update component and template
4. Verify → commit + push
