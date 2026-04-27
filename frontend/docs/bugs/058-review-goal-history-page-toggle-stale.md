# 058 — `ReviewGoalHistoryPage.reviewToggle` uses the stale tab role

## Status

FIXED — `reviewToggle.click()` now succeeds (verified by failure migrating
from line 34 to line 35). The remaining `addTile` failure in this test is
tracked separately (FAB is hidden in review mode → no UI path to add a
review-only tile).

## Symptom

`review-goal-history.spec.ts:13` times out clicking the review toggle:

```
locator.click: Test timeout of 30000ms exceeded.
  - getByTestId('dashboard-mode-toggle').getByRole('tab', { name: /review/i })
```

## Root cause

Same pattern as 051 in a different POM
(`projects/commitments-app/e2e/pages/review-goal-history.page.ts:25`):

```ts
this.reviewToggle = page.getByTestId('dashboard-mode-toggle').getByRole('tab', { name: /review/i });
```

The mode-toggle component (`commitments-ui/.../mode-toggle.component.html`)
renders `mat-button-toggle` segments, not a tab list. The `DashboardModePage`
already migrated to the class-based selector `.mode-toggle__segment--review`.

## Fix

Use the same selector `DashboardModePage` uses:

```ts
this.reviewToggle = page.getByTestId('dashboard-mode-toggle').locator('.mode-toggle__segment--review');
```

## Resolution

- [x] Failing test verified pre-fix (timeout on getByRole('tab')).
- [x] POM updated to `.mode-toggle__segment--review`.
- [x] reviewToggle click now succeeds. Remaining test failure is at the
      next step (addTile in review mode) — separate bug.
