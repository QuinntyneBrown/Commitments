# 058 — `ReviewGoalHistoryPage.reviewToggle` uses the stale tab role

## Status

OPEN.

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

- [ ] Failing test verified pre-fix.
- [ ] POM updated.
- [ ] Test verified passing post-fix.
