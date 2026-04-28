---
id: bug-097
title: outstanding-todos-tile.component.scss var() calls omit the design-system fallback hex
status: Open
---

# Bug 097 — outstanding-todos var() fallback hex

**Status**: Fixed once the bug-096 pattern is applied.

**Status**: Open

## Description

Same shape as bug-096 (daily-results) — six `var(--cui-*)`
references in `outstanding-todos-tile.component.scss` are
missing the cui-ui-style hex fallback:

```scss
:host { --cui-tile-icon-color: var(--cui-warning); }
.todo-count       { color: var(--cui-warning); }
.todo-copy        { color: var(--cui-text-secondary); }
.todo-delta       { color: var(--cui-text-secondary); }
.todo-delta--success { color: var(--cui-success); }
.todo-delta--warn    { color: var(--cui-warning); }
```

The fallback both documents the intended palette at the point of
use and keeps the tile rendering correctly when the tokens
stylesheet is late or scoped out (isolated tests).

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.spec.ts`

## Reproduction

```bash
grep -nE 'var\(--cui-[a-z-]+\)\B' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss
```

Returns six matches.

## Expected

Each `var()` reference includes the design-system hex fallback:

- `var(--cui-warning, #FFA726)` (icon-color, count, delta--warn)
- `var(--cui-text-secondary, #B0B0B0)` (copy, delta default)
- `var(--cui-success, #66BB6A)` (delta--success)

## Verification

- Unit (CSS source): assert no `var(--cui-*)` reference appears
  without a `, #fallback` argument (same shape as bug-096 test).
- All existing outstanding-todos specs continue to pass.
