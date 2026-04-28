---
id: bug-189
title: add-tile-dialog selected-cell box-shadow uses literal #FF408155 instead of color-mix on --cui-accent
status: Open
---

# Bug 189 — Selected-cell shadow should use `color-mix()` on `--cui-accent`

## Description

`add-tile-dialog.component.scss` line 99:

```scss
.add-tile-dialog__cell--selected {
  background: var(--cui-accent, #FF4081);
  border: 2px solid var(--cui-accent, #FF4081);
  box-shadow: 0 4px 12px #FF408155;  // ← literal hex+alpha
  color: var(--cui-on-accent, #FFFFFF);
}
```

The shadow color is `--cui-accent` (#FF4081) at 33% alpha
(`0x55 / 0xFF ≈ 33%`). The same rule already uses the token
for background and border. Continue routing the shadow tint
through the same token via `color-mix()`:

```scss
box-shadow: 0 4px 12px color-mix(in srgb, var(--cui-accent, #FF4081) 33%, transparent);
```

If the accent palette ever shifts, all four properties update
together.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss`

## Reproduction

```bash
grep -n '#FF408155' frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss
```

Returns one match.

## Expected

The literal hex+alpha is replaced with the `color-mix()` form.
A regression-guard spec asserts the swap.

## Verification

- New regression spec confirms the fix.
- All other tests continue to pass.
