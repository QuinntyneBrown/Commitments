---
id: bug-096
title: daily-results-tile.component.scss var() calls omit the design-system fallback hex used everywhere else
status: Open
---

# Bug 096 — daily-results var() fallback hex

**Status**: Open

## Description

`commitments-ui` consistently writes CSS custom property
references with a literal hex fallback:
`var(--cui-text-primary, #FFFFFF)`,
`var(--cui-divider, #3A3A3A)`, etc. The fallback both documents
the intended palette value at the point of use and keeps the
component visually correct if the global tokens stylesheet is
late or scoped out (in unit tests, isolated harnesses, etc.).

`daily-results-tile.component.scss` writes them without the
fallback:

```scss
.metric__value  { color: var(--cui-success); }
.metric__label  { color: var(--cui-text-secondary); }
.progress       { background: var(--cui-divider); }
.progress span  { background: var(--cui-success); }
```

Four sites that diverge from the rest of the codebase and risk
a missing-token rendering. The other plugin tiles share the same
issue but this iteration scopes the fix to daily-results to
keep the change small.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss
```

Returns 4 sites without a `, #fallback` argument.

## Expected

Each `var()` reference includes the matching design-system
fallback hex:

```scss
color: var(--cui-success, #66BB6A);
color: var(--cui-text-secondary, #B0B0B0);
background: var(--cui-divider, #3A3A3A);
background: var(--cui-success, #66BB6A);
```

## Verification

- Unit (CSS source): assert no `var(--cui-` reference appears
  without a `, #...` fallback.
- All existing daily-results specs continue to pass.
