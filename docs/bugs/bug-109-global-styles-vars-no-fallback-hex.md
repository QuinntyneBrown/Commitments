---
id: bug-109
title: Global commitments-app styles.scss var() calls omit the design-system fallback hex
status: Open
---

# Bug 109 — global styles.scss var() fallback hex

**Status**: Open

## Description

Continuing the bug-096–108 series at the application root.
`commitments-app/src/styles.scss` sets body color and
background using design-system tokens without the cui-ui-style
hex fallback:

```scss
body, html {
  …
  background-color: var(--cui-bg);
  color: var(--cui-text-primary);
}
```

This is the page-level fallback that every dashboard tile sits
on top of, so it's the highest-impact site to keep tokenized.

## Affected files

- `frontend/projects/commitments-app/src/styles.scss`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z0-9-]+\)\B' frontend/projects/commitments-app/src/styles.scss
```

Returns 2 matches.

## Expected

- `var(--cui-bg, #121212)` (body background)
- `var(--cui-text-primary, #FFFFFF)` (body color)

## Verification

- Source-level grep returns no matches after the fix.
- All existing tests stay green.
