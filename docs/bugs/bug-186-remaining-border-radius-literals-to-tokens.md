---
id: bug-186
title: 5 SCSS files use literal border-radius values instead of --cui-radius-* tokens
status: Open
---

# Bug 186 — Migrate remaining `border-radius` literals to tokens

## Description

After bugs 184 and 185 routed pill and edit-mode border-radius
through tokens, 5 more literals remain. Each maps to an
existing canonical token:

| Component file | Literal | Token |
|----------------|---------|-------|
| `delta-badge.component.scss:11` | `4px` | `var(--cui-radius-sm, 4px)` |
| `login-page.component.scss:57` | `4px` | `var(--cui-radius-sm, 4px)` |
| `add-tile-dialog.component.scss:170` | `4px` | `var(--cui-radius-sm, 4px)` |
| `review-scrubber.component.scss:6` | `12px` | `var(--cui-radius-lg, 12px)` |
| `review-scrubber.component.scss:56` | `9999px` | `var(--cui-radius-full, 9999px)` |

The `monthly-progress-tile` `border-radius: 4px 4px 0 0` (top-only
on bars) is left as a literal — substituting four `var()`
references would clutter the rule for no semantic gain.

## Affected files

- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.scss`
- `frontend/projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss`
- `frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss`
- `frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.scss`

## Reproduction

```bash
grep -rn 'border-radius:\s*\d\+px;\|border-radius:\s*9999px;' frontend/projects --include='*.scss'
```

Returns the 5 lines.

## Expected

All 5 lines route through their canonical tokens. A
regression-guard spec asserts no bare-number `border-radius`
literal remains in the four files.

## Verification

- New regression spec confirms the migration.
- All other tests continue to pass.
