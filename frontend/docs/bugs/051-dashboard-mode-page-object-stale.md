# 051 — DashboardModePage POM uses stale selectors

## Status

FIXED — `dashboard-mode.spec.ts` 9/9 passing across tablet, lg-desktop, xl-desktop.

## Symptom

All 9 tests in `dashboard-mode.spec.ts` fail (3 viewport projects × 3 tests).
Sample failure:

```
Locator: getByTestId('dashboard-mode-toggle').getByRole('tab', { name: /live/i })
Expected: "true"
Timeout: 5000ms
Error: element(s) not found
```

## Root cause

`projects/commitments-app/e2e/pages/dashboard-mode.page.ts` was authored against an older mode-toggle that rendered as a tab list. The current implementation in `commitments-ui/.../mode-toggle.component.html` renders a `mat-button-toggle-group` with two `mat-button-toggle` segments:

- Active state lives on the class `mode-toggle__segment--active` (no `aria-pressed`, no `role="tab"`).
- The Add Tile FAB is `data-testid="add-tile-fab"` (the POM still asks for `add-tile`).

The flow doc itself flags this:
> the existing `DashboardModePage` page-object still references the old `add-tile` testid — the FAB is now `add-tile-fab`.
> `expectLiveActive`/`expectReviewActive` currently look for `aria-pressed` on a tab role and the `add-tile` testid — update them to use `mode-toggle__segment--active` and `add-tile-fab`.

## Fix

Update `dashboard-mode.page.ts`:

1. `liveSegment` / `reviewSegment` → target `.mode-toggle__segment--live` / `.mode-toggle__segment--review` directly.
2. `addTileButton` → `getByTestId('add-tile-fab')`.
3. `expectLiveActive` / `expectReviewActive` → assert the live/review segment carries the `mode-toggle__segment--active` class instead of `aria-pressed`.

The existing `dashboard-mode.spec.ts` is the failing test; once the POM is corrected, it goes green.

## Resolution

- [x] Failing tests verified (9/9 fail) — pre-fix run.
- [x] POM updated (`fix(e2e): align DashboardModePage POM…`).
- [x] Tests verified passing (9/9 pass post-fix, all viewports).
