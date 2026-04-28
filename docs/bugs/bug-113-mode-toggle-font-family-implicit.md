---
id: bug-113
title: mode-toggle doesn't pin --cui-font-display, relies on body cascade
status: Fixed
---

# Bug 113 — mode-toggle font-family

**Status**: Fixed

## Fix

Added `font-family: var(--cui-font-display, 'Inter')` to the
`.mode-toggle` root. Propagates to `.mode-toggle__segment`
labels. Continues the bug-067 / bug-111 / bug-112 pinning
series.

310/310 workspace tests green.

## Description

Continuing the bug-067 / bug-111 / bug-112 font-family pinning
series. `mode-toggle.component.scss` is the most
tile-adjacent cui-ui component still missing an explicit
`font-family` declaration:

```scss
.mode-toggle__segment {
  height: 28px;
  ...
  font-weight: 500;
  font-size: 12px;
  letter-spacing: 0.6px;
  text-transform: uppercase;
  ...
}
```

The mode-toggle drives every tile's mode signal (live ↔ review),
so its label rendering should match the rest of the dashboard
chrome typography.

## Affected files

- `frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.scss`
- `frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.spec.ts`

## Reproduction

```bash
grep -n 'font-family' frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.scss
```

No matches.

## Expected

`.mode-toggle` (the root) declares
`font-family: var(--cui-font-display, 'Inter')`. Pinning at the
root propagates to `.mode-toggle__segment` (which has its own
font-size/weight but no font-family).

## Verification

- Unit (CSS source): assert `.mode-toggle` block contains
  `font-family: var(--cui-font-display`.
- All existing mode-toggle specs continue to pass.
