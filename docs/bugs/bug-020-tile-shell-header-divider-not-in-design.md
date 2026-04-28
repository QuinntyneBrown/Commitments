---
id: bug-020
title: Tile shell renders a 1px divider beneath the header, but tile designs have no header divider
status: Fixed
---

# Bug 020 — Tile shell header has a divider not present in the design

**Status**: Fixed

## Fix

`tile-shell.component.scss` no longer sets `padding-bottom` or
`border-bottom` on `.tile-shell__header`. `.tile-shell__body`'s
`padding-top` was tightened from 14px to 10px to match the .pen
frame gap of 10. The hairline beneath every tile is gone.

Coverage:
- New spec `does not draw a divider beneath the header (bug-020)`
  reads the SCSS source and asserts the `.tile-shell__header` rule
  has no `border-bottom` declaration.
- All 10 affected suites pass (43/43 — was 42/42 before).

## Description

`frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
gives every tile header a `border-bottom: 1px solid var(--cui-divider, #3A3A3A)`
plus `padding-bottom: 12px`. In `docs/tiles/daily-results-tile/ui-design.pen`
the header (`drHd`) has no stroke beneath it — the body flows directly under
the title row with the tile's own gap-10. The same is true on inspection of
the other tile designs.

The visible result is a horizontal hairline running across every tile
between the title and the body, which the design does not call for.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`

## Reproduction

1. Render any plugin tile (daily-results, goal-metrics, consistency-trend).
2. Note the thin `#3A3A3A` line that runs full-width under the title row.
3. Compare to the .pen — there is no such line in any tile design.

## Expected

No horizontal divider beneath the header. The body sits directly beneath
the header with the tile's vertical gap (~10px), matching the design.

## Verification

- Unit: spec asserting `.tile-shell__header` has no bottom border in
  static markup. (jsdom doesn't compute SCSS at the unit level, so the
  unit assertion targets the CSS source — see suggested fix.)
- Visual: screenshot before/after; compare against the .pen.
