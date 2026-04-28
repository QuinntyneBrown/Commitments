---
id: bug-110
title: dashboard-layout.component.scss var() calls omit the design-system fallback hex
status: Fixed
---

# Bug 110 — dashboard-layout var() fallback hex

**Status**: Fixed

## Fix

Added the design-system hex fallback to all 17 `var()`
references in `dashboard-layout.component.scss` across text,
bg, toolbar, sidenav, divider, hover-overlay, primary, and
primary-strong tokens. The bug-110 spec is appended to the
existing dashboard-layout spec with the same regex iteration
guard. 305/305 workspace tests green.

## Description

Continuing the bug-096–109 series.
`commitments-app`'s
`dashboard-layout.component.scss` (the host that wraps the
dashboard-shell — provides the topbar above the tile grid) has
17 `var(--cui-*)` references missing the cui-ui-style hex
fallback.

## Affected files

- `frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.scss`
- `frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.spec.ts` (or new spec)

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z0-9-]+\)\B' frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.scss
```

Returns 17 matches.

## Expected

Each `var()` reference includes its design-system hex fallback:

- `var(--cui-text-primary, #FFFFFF)`
- `var(--cui-text-secondary, #B0B0B0)`
- `var(--cui-bg, #121212)`
- `var(--cui-toolbar, #1F2233)`
- `var(--cui-sidenav, #181A24)`
- `var(--cui-divider, #3A3A3A)`
- `var(--cui-hover-overlay, #FFFFFF14)`
- `var(--cui-primary, #9FA8DA)`
- `var(--cui-primary-strong, #7986CB)`

## Verification

- Source-level grep: no `var(--cui-*)` reference appears
  without a `, #fallback`.
- Existing tests stay green.
