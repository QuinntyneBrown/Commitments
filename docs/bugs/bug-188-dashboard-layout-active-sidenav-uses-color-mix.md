---
id: bug-188
title: dashboard-layout active sidenav uses literal #9FA8DA22 instead of color-mix on --cui-primary
status: Fixed
---

# Bug 188 — dashboard-layout active-sidenav background should use `color-mix()` on `--cui-primary`

**Status**: Fixed

## Fix

```scss
- background: #9FA8DA22;
+ background: color-mix(in srgb, var(--cui-primary, #9FA8DA) 13%, transparent);
```

Continues the bug-187 token-mix convention. 388/388 workspace
tests green.

## Description

`dashboard-layout.component.scss` line 145:

```scss
.sidenav-item--active {
  background: #9FA8DA22;
  border-left-color: var(--cui-primary, #9FA8DA);
  color: var(--cui-primary, #9FA8DA);
  ...
}
```

That's `--cui-primary` (#9FA8DA) at 13.3% opacity (`0x22 / 0xFF
≈ 13%`). Same shape as bug-187: every other "primary tinted
overlay" surface (status-pill --review, review-scrubber date
pill) uses `color-mix()`. Normalize:

```scss
background: color-mix(in srgb, var(--cui-primary, #9FA8DA) 13%, transparent);
```

The active sidenav now ties its active-state tint to the
design token alongside its border-left and color. If the
primary palette ever changes, all three properties update
together.

## Affected files

- `frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.scss`

## Reproduction

```bash
grep -n '#9FA8DA22' frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.scss
```

Returns one match.

## Expected

The literal hex+alpha is replaced with the `color-mix()` form.
A regression-guard spec asserts the swap.

## Verification

- New regression spec confirms the fix.
- All other tests continue to pass.
