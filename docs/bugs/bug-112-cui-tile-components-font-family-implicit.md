---
id: bug-112
title: metric-header, status-pill, and delta-badge don't explicitly pin --cui-font-display
status: Fixed
---

# Bug 112 — cui tile-component font-family

**Status**: Fixed

## Fix

Added `font-family: var(--cui-font-display, 'Inter')` to the
root rule of all three components (`.metric-header`,
`.status-pill`, `.delta-badge`). Pinning at the root propagates
to every child span, mirroring the bug-067 / bug-111 pattern
on `tile-shell__title` / `tile-shell__eyebrow`.

309/309 workspace tests green.

## Description

The bug-067 / bug-111 pattern (explicit
`font-family: var(--cui-font-display, 'Inter')`) is currently
applied only to `tile-shell__title` and `tile-shell__eyebrow`.
The three other cui-ui components that render text inside
tiles still rely on the body cascade:

- `metric-header.component.scss` — value (48px), caption (11px),
  sub-caption (11px)
- `status-pill.component.scss` — label (11px, letter-spacing 1)
- `delta-badge.component.scss` — value (11px), caption (11px)

Every text node in the corresponding `ui-design.pen` files
specifies `fontFamily: "Inter"`. Pinning the design token at the
component root (`.metric-header`, `.status-pill`,
`.delta-badge`) propagates to all child spans and removes the
silent dependency on the body cascade.

## Affected files

- `frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.scss` + spec
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.scss` + spec
- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.scss` + spec

## Reproduction

```bash
grep -L 'font-family' frontend/projects/commitments-ui/src/lib/{metric-header,status-pill,delta-badge}/*.scss
```

All three SCSS files lack a `font-family` declaration.

## Expected

Each component's root rule includes
`font-family: var(--cui-font-display, 'Inter')`.

## Verification

- Unit (CSS source): each component's spec asserts the root
  rule contains `font-family: var(--cui-font-display`.
- All existing specs continue to pass.
