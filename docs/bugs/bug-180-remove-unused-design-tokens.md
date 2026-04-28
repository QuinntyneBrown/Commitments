---
id: bug-180
title: 31 design tokens declared in _tokens.scss have zero consumers — drop
status: Fixed
---

# Bug 180 — Drop 30 unused design tokens

**Status**: Fixed

## Fix

After verifying each token by grep, 30 unused declarations
removed (the bug doc initially counted 31, but `--cui-shadow-login-card`
turned out to be used by login-page and was kept).

`_tokens.scss` shrinks from 79 → 41 lines. The bug-179 + 180
arc has reduced this foundational file from 93 lines to 41
lines (-56% size). 380/380 workspace tests green.

## Description

After bug-179 removed the 16 legacy non-prefixed duplicates,
`_tokens.scss` still carries 31 `--cui-` tokens that have
**zero `var()` consumers** anywhere in the workspace:

**Typography scale (13):**
- `--cui-fs-xs`, `--cui-fs-sm`, `--cui-fs-body`, `--cui-fs-md`,
  `--cui-fs-lg`, `--cui-fs-xl`, `--cui-fs-h3`, `--cui-fs-h2`,
  `--cui-fs-h1`, `--cui-fs-display`
- `--cui-fw-regular`, `--cui-fw-medium`, `--cui-fw-bold`

**Spacing scale (10):**
- `--cui-sp-1` through `--cui-sp-10`

**Layout (3):**
- `--cui-toolbar-height`, `--cui-sidenav-width`,
  `--cui-text-on-primary`

**Shadows (3):**
- `--cui-shadow-raised`, `--cui-shadow-floating`,
  `--cui-shadow-login-card`

**Misc (2):**
- `--cui-font-body`, `--cui-font-mono`, `--cui-focus-ring`

(Note: `--cui-shadow-login-card` is referenced in
login-page.component.scss but only as `var(--cui-shadow-login-card)`
without fallback — kept for now and revisited in a follow-up
if needed; verifying usage on a per-token basis.)

Cross-repo grep:

```bash
grep -rn 'var(--cui-fs-\|var(--cui-fw-\|var(--cui-sp-' frontend/projects --include='*.scss'
```

…returns no matches outside `_tokens.scss`.

YAGNI: drop the unused declarations. If a future component
needs them, they can be reintroduced as a single design-token
block.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tokens/_tokens.scss`

## Expected

The 25-30 unused declarations are removed (need to verify
each is truly dead before deletion). The token file shrinks
considerably. Used tokens (`--cui-bg`, `--cui-surface-*`,
`--cui-text-primary`, `--cui-divider`, `--cui-accent`,
`--cui-primary`, `--cui-success`, `--cui-warning`,
`--cui-warn`, `--cui-info`, `--cui-on-primary`,
`--cui-on-accent`, `--cui-radius-*`, `--cui-hover-overlay`,
`--cui-tile-*`, `--cui-font-display`, `--cui-text-disabled`,
`--cui-text-secondary`, `--cui-divider-soft`,
`--cui-primary-strong`, `--cui-primary-dim`,
`--cui-accent-strong`, `--cui-toolbar`, `--cui-sidenav`,
`--cui-shadow-login-card`, `--cui-header-height`) remain.

A regression-guard spec asserts the dead declarations don't
re-appear.

## Verification

- New regression spec confirms the file shrinks.
- All other tests continue to pass.
