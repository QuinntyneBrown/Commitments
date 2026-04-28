---
id: bug-183
title: primary-header references undeclared --cui-header-padding and uses literal font shorthand
status: Open
---

# Bug 183 — primary-header SCSS cleanups

## Description

Two issues in `primary-header.component.scss`:

1. **Dead `var(--cui-header-padding, 20px)` reference** (line
   14): the `--cui-header-padding` token is not declared
   anywhere in `_tokens.scss` (was never declared, and the
   bug-180 token sweep did not add it). The `var()` always
   falls back to `20px`. Drop the dead wrapper and use the
   literal:

   ```scss
   padding: 0 20px;
   ```

2. **Literal `font:` shorthand with Inter** (line 27):

   ```scss
   font: 500 28px/1.2 Inter, Roboto, Arial, sans-serif;
   ```

   Continues the bug-172/173/174/175/181 token-migration arc.
   The shorthand bundles font-weight/size/line-height/family
   into one rule. Split to long-form so the family routes
   through the token:

   ```scss
   font-family: var(--cui-font-display, 'Inter');
   font-weight: 500;
   font-size: 28px;
   line-height: 1.2;
   ```

## Affected files

- `frontend/projects/commitments-ui/src/lib/primary-header/primary-header.component.scss`

## Reproduction

```bash
grep -n '--cui-header-padding\|font: ' frontend/projects/commitments-ui/src/lib/primary-header/primary-header.component.scss
```

Returns the two lines.

## Expected

Both fixes applied. A regression-guard spec asserts the dead
token reference is gone and no literal `Inter` appears in the
SCSS.

## Verification

- New regression spec confirms both fixes.
- All existing primary-header specs continue to pass.
