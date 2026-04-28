---
id: bug-177
title: add-tile-dialog disabled button fallbacks are off — text-disabled and undeclared cui-disabled
status: Open
---

# Bug 177 — add-tile-dialog disabled-button fallback fixes

## Description

`add-tile-dialog.component.scss` lines 186-190:

```scss
.add-tile-dialog__btn--primary[disabled] {
  background: var(--cui-disabled, #555);
  color: var(--cui-text-disabled, #888);
  cursor: not-allowed;
}
```

Two issues:

1. **`--cui-disabled` is not declared anywhere** in `_tokens.scss`.
   The `var()` always falls back to `#555`. Since the token
   reference is dead, drop the `var()` and use the literal —
   or, if "disabled background" is a recurring need, declare
   the token. Currently no other consumer needs it, so YAGNI:
   inline the literal `#555` and drop the orphan reference.

2. **`--cui-text-disabled` token is `#666666`, not `#888`**.
   Same fallback-mismatch shape as bug-176 (`#fff` vs
   `#1A1B23`). If the design tokens stylesheet were ever
   missing, the disabled-button text would render at `#888`
   (lighter than intended `#666`).

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss`

## Reproduction

```bash
grep -n 'var(--cui-disabled\|var(--cui-text-disabled, #888)' frontend/projects/dashboard-framework/src/lib/dashboard/add-tile-dialog/add-tile-dialog.component.scss
```

Returns two matches.

## Expected

```scss
.add-tile-dialog__btn--primary[disabled] {
  background: #555;
  color: var(--cui-text-disabled, #666666);
  cursor: not-allowed;
}
```

A regression-guard spec asserts:
- The orphan `var(--cui-disabled,` reference is gone.
- The corrected `var(--cui-text-disabled, #666666)` fallback
  is present.

## Verification

- New regression spec confirms both fixes.
- All other tests continue to pass.
