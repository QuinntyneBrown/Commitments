# 053 — "Topbar action button" test points at the FAB, which isn't in the topbar

## Status

FIXED — `dashboard.spec.ts:20` passes on lg-desktop.

## Symptom

`dashboard.spec.ts:20` — `topbar action buttons use the stroked-on-dark style (transparent fill)` — fails:

```
expect(received).toBe(expected) // Object.is equality
- Expected: "rgba(0, 0, 0, 0)"
+ Received: "rgb(255, 64, 129)"
```

The received color is `--cui-accent` (#FF4081 — the FAB pink).

## Root cause

After 6130415 swapped `addTileButton` from `add-tile` → `add-tile-fab`, the
test became wrong-by-construction. The current shell renders:

- `<button data-testid="edit-mode-enter">` — sits **in** the PrimaryHeader's
  `header-actions` slot; SCSS gives it `background: transparent`.
- `<button data-testid="add-tile-fab">` — fixed-position FAB at bottom-right;
  SCSS gives it `background: var(--cui-accent)` (pink).

The test's intent — *topbar action buttons* are stroked-on-dark with a
transparent fill — describes the edit-mode-enter button, not the FAB.

## Fix

Add a dedicated `editModeEnterBackground()` accessor to `DashboardPage` and
point the L20 test at it. Single targeted accessor; no API churn elsewhere.

## Resolution

- [x] Failing test verified pre-fix (received `rgb(255, 64, 129)`).
- [x] POM accessor `editModeEnterBackground()` added; spec L20 switched to it.
- [x] Test verified passing post-fix.
