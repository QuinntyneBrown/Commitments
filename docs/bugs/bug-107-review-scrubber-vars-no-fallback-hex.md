---
id: bug-107
title: review-scrubber.component.scss var() calls omit the design-system fallback hex
status: Fixed
---

# Bug 107 — review-scrubber var() fallback hex

**Status**: Fixed

## Fix

Added the design-system hex fallback to 8 `var()` references
in `review-scrubber.component.scss` across surface, surface-3,
text, and primary tokens. Final dashboard-framework component
in the bug-096–106 series.

The bug-107 spec regex was broadened from `[a-z-]+` to
`[a-z0-9-]+` so it also catches tokens with digits like
`surface-3` — without that, line 57 (`var(--cui-surface-3)`)
would have slipped past the regression guard.

304/304 workspace tests green.

## Description

Final dashboard-framework component in the bug-096–106 series.
`review-scrubber.component.scss` (the date scrubber that drives
every tile's review-mode `selectedReviewDate`) has 7
`var(--cui-*)` references missing the cui-ui-style hex fallback.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.scss`
- `frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.scss
```

Returns 7 matches.

## Expected

- `var(--cui-surface, #1E1E1E)`
- `var(--cui-text-primary, #FFFFFF)`
- `var(--cui-text-secondary, #B0B0B0)`
- `var(--cui-primary, #9FA8DA)`

## Verification

- Unit (CSS source): no `var(--cui-*)` reference appears without
  a `, #fallback`.
- All existing review-scrubber specs continue to pass.
