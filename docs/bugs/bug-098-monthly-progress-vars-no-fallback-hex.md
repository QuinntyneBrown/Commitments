---
id: bug-098
title: monthly-progress-tile.component.scss var() calls omit the design-system fallback hex
status: Open
---

# Bug 098 — monthly-progress var() fallback hex

**Status**: Open

## Description

Same shape as bug-096 (daily-results) and bug-097
(outstanding-todos): two `var(--cui-*)` references in
`monthly-progress-tile.component.scss` are missing the
cui-ui-style hex fallback:

```scss
:host       { --cui-tile-icon-color: var(--cui-info); }
.bar-labels { color: var(--cui-text-disabled, #666666); /* already OK */ }
.empty      { color: var(--cui-text-secondary); }
```

Wait — the `.bar-labels` color is already tokenized with a
fallback (post bug-065). The two offenders are the icon-color
host definition and the `.empty` state copy.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss
```

Returns two matches.

## Expected

- `var(--cui-info, #42A5F5)` for icon-color
- `var(--cui-text-secondary, #B0B0B0)` for .empty

## Verification

- Unit (CSS source): assert no `var(--cui-*)` reference appears
  without a `, #fallback` argument.
- All existing monthly-progress specs continue to pass.
