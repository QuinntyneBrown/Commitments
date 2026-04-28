---
id: bug-104
title: icon-button.component.scss var() calls omit the design-system fallback hex
status: Open
---

# Bug 104 — icon-button var() fallback hex

**Status**: Open

## Description

Continuing the bug-096–103 series in the cui-ui library itself.
`icon-button.component.scss` has three `var(--cui-text-primary)`
references without the cui-ui-style hex fallback:

```scss
.icon-button:hover:not(:disabled)  { color: var(--cui-text-primary); }
.icon-button:focus-visible         { outline: 2px solid var(--cui-text-primary); }
.icon-button--pressed              { color: var(--cui-text-primary); }
```

Three sites — the only ones in cui-ui still missing fallbacks
(every other cui-ui SCSS file already follows the convention).

## Affected files

- `frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.scss`
- `frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.scss
```

Returns three matches.

## Expected

`var(--cui-text-primary, #FFFFFF)` for each.

## Verification

- Unit (CSS source): no `var(--cui-*)` reference appears without
  a `, #fallback`.
- All existing icon-button specs continue to pass.
