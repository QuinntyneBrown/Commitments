---
id: bug-178
title: --cui-on-accent fallback uses 3-char #fff while sibling --cui-text-primary uses 6-char #FFFFFF
status: Open
---

# Bug 178 — Normalize `--cui-on-accent` fallback to `#FFFFFF`

## Description

`var(--cui-on-accent, #fff)` appears 7 times across the
dashboard-framework, but every `var(--cui-text-primary, …)`
(29 occurrences across 14 files) uses the canonical 6-char
form `#FFFFFF`. Both tokens have the same value
(`#FFFFFF` / `#fff` resolve identically in CSS), so the
inconsistency is purely cosmetic but reads as a sloppy mix.

Cross-repo grep:

```
$ grep -rn 'var(--cui-on-accent, #fff)' frontend/projects --include='*.scss'
… 7 matches across 3 files (dashboard-shell, add-tile-dialog, dashboard-grid).

$ grep -rn 'var(--cui-text-primary, #fff)' frontend/projects --include='*.scss'
(no matches — every text-primary fallback uses #FFFFFF)
```

Normalize the 7 `--cui-on-accent` fallbacks to `#FFFFFF` so
the codebase has one convention for canonical-white fallbacks.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.scss` (1)
- `frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss` (4)
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss` (2)

## Reproduction

```bash
grep -rn 'var(--cui-on-accent, #fff)' frontend/projects --include='*.scss'
```

Returns 7 matches.

## Expected

All 7 occurrences use `var(--cui-on-accent, #FFFFFF)`.
A regression-guard spec asserts the 3-char form is gone.

## Verification

- New regression spec confirms the normalization.
- All other tests continue to pass.
