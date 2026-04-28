---
id: bug-022
title: Weekly Focus — each focus row renders name and supporting metric side-by-side; design stacks them vertically
status: Fixed
---

# Bug 022 — Weekly Focus row layout is horizontal, design is vertical

**Status**: Fixed

## Fix

`weekly-focus-tile.component.scss` switches `.focus-list li` to a flex
column with `gap: 2` and bottom-only padding (`0 0 8px`), matching the
.pen `wfRow*` frames. A new `.focus-list li:last-child` rule drops the
divider and bottom padding from the final row, mirroring the design
where `wfRow3` has no bottom stroke.

Coverage:
- New `weekly-focus-tile.component.spec.ts` reads the SCSS source and
  asserts `.focus-list li` declares `flex-direction: column`.
- All 13 affected suites pass (50/50 — was 44/44 before this loop).

## Description

In `docs/tiles/weekly-focus-tile/ui-design.pen` each list row (`wfRow1`,
`wfRow2`, `wfRow3`) is a vertical frame with gap 2:

```
Move                 (Inter 14 / 700 / #FFFFFF)
5 sessions planned   (Inter 11 / 400 / #B0B0B0)
```

The implementation renders the same data side-by-side using a flex row
with `justify-content: space-between`:

```
Move                       5 sessions planned
Read                       3 sessions planned
Reflect                    2 notes pending
```

(`frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss`,
rule `.focus-list li`).

The visual result is a two-column read where the supporting metric
crowds the right edge, instead of a stacked, scannable list with the
metric directly under the bold focus name as the design specifies.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss`

## Reproduction

1. Render the Weekly Focus tile.
2. Observe each row laid out horizontally (name left, metric right).
3. Compare to the .pen — name is on top, metric below in smaller text.

## Expected

Each `<li>` is a flex (or block) **column** with the name on top and
the supporting metric directly beneath it, gap ~2px, matching the .pen
`wfRow*` frames.

## Verification

- Unit: source-level SCSS check — `.focus-list li` declares
  `flex-direction: column` (or omits `display: flex` and renders
  vertically by default).
- Visual: screenshot the rendered tile vs the .pen.
