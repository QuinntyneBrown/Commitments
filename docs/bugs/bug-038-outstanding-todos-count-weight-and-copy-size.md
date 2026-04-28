---
id: bug-038
title: Outstanding Todos — count weight is 800 and copy size is 13px; design specifies 700 and 12px
status: Fixed
---

# Bug 038 — Outstanding Todos count weight + copy size

**Status**: Fixed

## Fix

`outstanding-todos-tile.component.scss`:
- `.todo-count` font-weight 800 → 700
- `.todo-copy` font-size 13px → 12px

Coverage:
- Two new specs in `outstanding-todos-tile.component.spec.ts`
  assert the count's weight 700 and the copy's 12px size.
- All 20 affected suites pass (100/100 — was 98/98 before).

The "big headline number at weight 700 with 12px supporting copy"
pattern is now consistent between daily-results-tile (bug-036) and
outstanding-todos-tile. SASS mixin extraction (`big-number-headline`)
deferred until a third tile adopts the same pattern.

## Description

`docs/tiles/outstanding-todos-tile/ui-design.pen` (frame `M68GO` →
`fEUu5` body) styles the headline and supporting copy as:

| Element                              | Family | Size | Weight | Colour    |
| ------------------------------------ | ------ | ---- | ------ | --------- |
| `4` (`xoafd`)                        | Inter  | 58   | 700    | `#FFA726` |
| `items need attention…` (`rJ7H4`)    | Inter  | 12   | 400    | `#B0B0B0` |

The implementation in `outstanding-todos-tile.component.scss`
specifies:

```scss
.todo-count { font-size: 58px; font-weight: 800; … }
.todo-copy  { font-size: 13px; … }
```

Two deviations: count weight is one stop heavier (800 vs 700), and
copy size is 1px larger (13 vs 12). Same shape of fix as bug-036
applied to daily-results.

The colour tokens (`var(--cui-warning)` and `var(--cui-text-secondary)`)
already match the .pen `#FFA726` / `#B0B0B0` after token resolution.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss`

## Reproduction

1. Render the Outstanding Todos tile.
2. Inspect `.todo-count` — `font-weight: 800`.
3. Inspect `.todo-copy` — `font-size: 13px`.
4. Compare to the .pen — count is 700, copy is 12px.

## Expected

```scss
.todo-count { font-weight: 700; }
.todo-copy  { font-size: 12px; }
```

## Verification

- Unit: source-level SCSS spec — `.todo-count` declares
  `font-weight: 700` and `.todo-copy` declares `font-size: 12px`.
- Visual: screenshot the rendered tile vs the .pen.
