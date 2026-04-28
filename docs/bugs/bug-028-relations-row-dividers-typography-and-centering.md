---
id: bug-028
title: Relations — rows lack #2A2A2A dividers, body not vertically centered, row text 13px instead of 14px
status: Fixed
---

# Bug 028 — Relations row dividers, typography, body centering

**Status**: Fixed

## Fix

`relations-tile.component.html` wraps each iterated relation in a
`<div class="relations__row">`. `relations-tile.component.scss`:

- `.relations` switches from 2-column grid to flex column with
  `justify-content: center; height: 100%; gap: 8px; font-size: 14px`.
- `.relations__row` is a flex row (`justify-content: space-between`)
  with `padding: 0 0 8px` and a 1px `#2A2A2A` bottom border.
- `.relations__row:last-child` drops the divider and bottom padding,
  mirroring the .pen `rlRow3` having no stroke.
- `.relations strong` keeps `color: var(--cui-text-primary)` and
  pins `font-weight: 700`.

The pattern mirrors the weekly-focus-tile fix (bug-022) so juniors can
learn the row+last-child idiom from either tile. A shared SCSS mixin
is *not* introduced — two tiles isn't a strong abstraction case yet.

Coverage:
- Three new specs in `relations-tile.component.spec.ts` cover the
  row wrapper, font-size + vertical centering, and the divider colour.
- All 19 affected suites pass (80/80 — was 72/72 before).

## Description

`docs/tiles/relations-tile/ui-design.pen` (frame `rlGrid`) is a
vertical body that:

1. **Vertically centers** its row children within the available
   body height (`justifyContent: center, height: fill_container`).
2. **Separates rows** with a 1px `#2A2A2A` bottom stroke (last row
   has no stroke).
3. Renders row text at **Inter 14** — name in `#B0B0B0`, percentage
   in `#FFFFFF` weight 700.

The implementation in `relations-tile.component.scss` styles
`.relations` as a `display: grid; grid-template-columns: 1fr auto;
gap: 10px 16px; font-size: 13px;` with no row dividers and no
vertical centering, so rows cling to the top of the body in a
13px font.

The implementation's grid model is fine — but the visual treatment
diverges on three points: dividers, vertical centering, and font
size.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.html` (only if
  switching the grid into per-row blocks is needed for dividers)

## Reproduction

1. Render the Relations tile.
2. Body shows three rows (Health/Work/Personal) clinging to the top
   with no dividers, in 13px text.
3. Compare to the .pen — rows are vertically centered with thin
   `#2A2A2A` lines between them, in 14px text.

## Expected

- Each non-last row carries a 1px `#2A2A2A` bottom border with 8px
  bottom padding.
- The grid (or its parent) declares `align-content: center` and
  `height: 100%` so the rows sit centered vertically within the body.
- Row text is 14px.

## Verification

- Unit: source-level SCSS spec — `.relations` declares `font-size: 14px`,
  `align-content: center`, and the dividers (either via `grid-template`
  with row borders, or a per-row rule).
- Visual: screenshot the rendered tile vs the .pen.
