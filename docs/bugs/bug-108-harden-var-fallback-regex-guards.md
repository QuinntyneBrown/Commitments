---
id: bug-108
title: bug-096–106 regex guards use [a-z-]+ which misses tokens with digits like --cui-surface-3
status: Fixed
---

# Bug 108 — harden var() fallback regex guards

**Status**: Fixed

## Fix

Broadened the regex character class in all 11 bug-096–106
guard specs from `[a-z-]+` to `[a-z0-9-]+`. Forward-only
hardening — current SCSS files comply with both forms, so the
suite stays green (304/304). The change prevents the same
gap bug-107 ran into (where the narrower regex slipped past
`var(--cui-surface-3)` in review-scrubber).

This entry follows the bug-073 precedent: regression-coverage
extension where the "tests fail before fix" pattern doesn't
apply because no current code violates the broader form.

## Description

The bug-096–106 series each added a regex iteration spec
asserting that `var(--cui-*)` references include a fallback
hex. The character class used in those guards is `[a-z-]+`,
which matches tokens like `--cui-text-primary` but **not**
tokens with digits like `--cui-surface-2`, `--cui-surface-3`,
or `--cui-surface-4`.

bug-107 (review-scrubber) discovered this gap when its
component used `var(--cui-surface-3)` — the narrow regex
slipped past it. The bug-107 spec broadened the class to
`[a-z0-9-]+`, but the 11 earlier guards still use the narrow
form. If a future change introduces a digit-bearing token in
any of those scopes, the regression guard won't catch it.

## Affected files

The following spec files all use the narrower regex and need
broadening to `[a-z0-9-]+`:

- `projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.spec.ts` (bug-096)
- `projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.spec.ts` (bug-097)
- `projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.spec.ts` (bug-098)
- `projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.spec.ts` (bug-099)
- `projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.spec.ts` (bug-100)
- `projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts` (bug-101)
- `projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.spec.ts` (bug-102)
- `projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.spec.ts` (bug-103)
- `projects/commitments-ui/src/lib/icon-button/icon-button.component.spec.ts` (bug-104)
- `projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.spec.ts` (bug-105)
- `projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.spec.ts` (bug-106)

## Reproduction

```bash
grep -rn '\[a-z-\]+' frontend/projects --include="*.spec.ts" | grep "var(--cui-"
```

Returns 11 matches.

## Expected

Each guard's regex character class is `[a-z0-9-]+`.

## Verification

The tests still pass after broadening (since no untokenized
digit-bearing var ref currently exists in those scopes — the
broadening is forward-only hardening).
