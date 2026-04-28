---
id: bug-185
title: 4 SCSS files use literal border-radius 999px instead of --cui-radius-full token
status: Open
---

# Bug 185 — Normalize `border-radius: 999px` to `var(--cui-radius-full, 9999px)`

## Description

Four pill-shaped UI elements declare a literal `border-radius: 999px`:

- `mode-toggle.component.scss` lines 4, 14
- `status-pill.component.scss` line 11
- `review-scrubber.component.scss` line 73

The canonical token is `--cui-radius-full: 9999px`. Both
values render identically (CSS clamps border-radius to half
the smallest dimension), but the codebase already uses the
token form in:

- `delta-badge.component.scss` (effectively, via `4px` literal
  matching `--cui-radius-sm`)
- `dashboard-grid.component.scss` (after bug-184)
- `add-tile-dialog.component.scss`

Routing all pill-shaped corners through the token keeps any
future radius change consistent.

## Affected files

- `frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.scss` (2)
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.scss` (1)
- `frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.scss` (1)

## Reproduction

```bash
grep -rn 'border-radius: 999px' frontend/projects --include='*.scss'
```

Returns 4 matches.

## Expected

All four lines use `border-radius: var(--cui-radius-full, 9999px);`.
A regression-guard spec asserts no `border-radius: 999px` remains.

## Verification

- New regression spec confirms removal across the three files.
- All other tests continue to pass.
