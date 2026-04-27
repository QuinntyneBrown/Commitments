# 052 — DashboardPage POM topbar selector points to a class that no longer exists

## Status

FIXED — `dashboard.spec.ts:12` and `viewport.spec.ts` overflow check now pass.

## Symptom

`dashboard.spec.ts:12` — `topbar uses the dark toolbar token from the design system` — fails on every viewport:

```
Locator: locator('.dashboard-shell__topbar').first()
Error: locator.evaluate: Test timeout of 30000ms exceeded.
```

## Root cause

`projects/commitments-app/e2e/pages/dashboard.page.ts:114` reads:

```ts
topbarBackgroundColor(): Promise<string> {
  return this.page.locator('.dashboard-shell__topbar').first()
    .evaluate((el) => getComputedStyle(el).backgroundColor);
}
```

A repo-wide grep for `dashboard-shell__topbar` returns exactly one match — this POM file. The class is not produced by any component. The current shell renders the dark toolbar at `<header class="dashboard-layout__toolbar" data-testid="dashboard-layout-toolbar">` in `dashboard-layout.component.html`, with `background: var(--cui-toolbar)` (#1F2233 — what the test asserts).

## Fix

Point the POM at the real toolbar element. Smallest, junior-readable form:

```ts
this.page.locator('.dashboard-layout__toolbar').first()
```

The existing `dashboard.spec.ts:12` is the failing test; once the selector is corrected it goes green.

## Resolution

- [x] Failing test verified pre-fix.
- [x] POM updated (`fix(e2e): point DashboardPage topbar selectors at .dashboard-layout__toolbar`).
- [x] Tests verified passing post-fix on tablet/lg-desktop viewports.
