---
id: bug-026
title: Outstanding To-Dos — count and copy stack vertically; design lays them out horizontally
status: Open
---

# Bug 026 — Outstanding To-Dos body layout is vertical, design is horizontal

**Status**: Open

## Description

`docs/tiles/outstanding-todos-tile/ui-design.pen` (frame `otBody`)
arranges the body **horizontally**: a large count `4` sits on the
left, the copy `"items need attention before the next review."` runs
to its right (`textGrowth: fixed-width, width: fill_container`), with
`alignItems: center` and `gap: 14`.

The implementation in `outstanding-todos-tile.component.html` projects
the count and copy as separate block-level elements:

```html
<div class="todo-count">{{ controller.count() }}</div>
<p class="todo-copy">items need attention before the next review.</p>
```

`outstanding-todos-tile.component.scss` styles `.todo-count` (size 58)
and `.todo-copy` (size 13, max-width 220, line-clamp 2) as standalone
blocks with no flex/grid wrapper, so they stack vertically. The
visible result is `4` on top, copy beneath — not the side-by-side
layout the design specifies.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss`

## Reproduction

1. Render the Outstanding To-Dos tile.
2. Observe the count `4` on its own line and the copy beneath it.
3. Compare to the .pen — count is on the left, copy is on the right,
   vertically centered.

## Expected

The count and copy share a flex row container (e.g. `<div
class="todo-body">`) with `align-items: center; gap: 14px;`. The
copy stretches to fill the remaining horizontal space.

## Verification

- Unit: source-level template + SCSS check — template wraps count +
  copy in a `class="todo-body"` container; `.todo-body` declares
  `display: flex` and `align-items: center`.
- Visual: screenshot the rendered tile vs the .pen.
