---
id: bug-020
title: Tile shell renders a 1px divider beneath the header, but tile designs have no header divider
status: Open
---

# Bug 020 — Tile shell header has a divider not present in the design

**Status**: Open

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

## Suggested fix

`tile-shell.component.scss`:

```scss
.tile-shell__header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 12px;
  min-height: 0;
  // padding-bottom: 12px;          // removed
  // border-bottom: 1px solid …;    // removed
}

.tile-shell__body {
  …
  padding-top: 10px;   // matches design's tile gap of 10
}
```

## Verification

- Unit: extend `tile-shell.component.spec.ts` to assert
  `getComputedStyle(header).borderBottomWidth === '0px'`.
- Visual: screenshot before/after; compare against the .pen.
