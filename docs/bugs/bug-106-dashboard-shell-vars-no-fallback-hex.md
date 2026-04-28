---
id: bug-106
title: dashboard-shell.component.scss var() calls omit the design-system fallback hex
status: Open
---

# Bug 106 — dashboard-shell var() fallback hex

**Status**: Open

## Description

Continuing the bug-096–105 series.
`dashboard-framework`'s
`dashboard-shell.component.scss` (the outer host that wraps
the dashboard-grid plus the toolbar with mode-toggle and add-tile
button) has 5 `var(--cui-*)` references missing the cui-ui-style
hex fallback.

The shell is directly tile-related: it owns the toolbar that
adds tiles, the mode-toggle that drives every tile's mode
signal, and the gridster scroll container.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.scss`
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.scss
```

Returns 5 matches.

## Expected

- `var(--cui-text-primary, #FFFFFF)`
- `var(--cui-bg, #121212)`
- `var(--cui-accent, #FF4081)`

## Verification

- Unit (CSS source): no `var(--cui-*)` reference appears without
  a `, #fallback`.
- All existing dashboard-shell specs continue to pass.
