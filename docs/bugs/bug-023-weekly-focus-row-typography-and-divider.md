---
id: bug-023
title: Weekly Focus — supporting metric is 13px and the row divider is #3A3A3A; design is 11px and #2A2A2A
status: Open
---

# Bug 023 — Weekly Focus row typography + divider color drift

**Status**: Open

## Description

Two detail-level deviations from
`docs/tiles/weekly-focus-tile/ui-design.pen`:

1. **Supporting metric font size.** The design renders rows of the form
   `"5 sessions planned"`, `"3 sessions planned"`, `"2 notes pending"`
   at Inter 11/400/#B0B0B0. The implementation in
   `weekly-focus-tile.component.scss` styles `.focus-list span` at
   `font-size: 13px` (with the colour token `var(--cui-text-secondary)`
   already mapping to #B0B0B0).
2. **Row divider colour.** The design uses `#2A2A2A` for the bottom
   stroke between rows (`wfRow1` / `wfRow2` `stroke.fill`). The
   implementation uses `var(--cui-divider)` which resolves to
   `#3A3A3A`, a step lighter — visibly louder than the design.

Both are post-bug-022 polish; the row layout itself is now correct.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss`

## Reproduction

1. Render the Weekly Focus tile.
2. Inspect a `.focus-list span` — computed `font-size` is `13px`.
3. Inspect a `.focus-list li` — computed `border-bottom-color` is
   `rgb(58, 58, 58)`.
4. Compare to the .pen — supporting metric is 11px and the divider
   stroke is `#2A2A2A` (rgb 42, 42, 42).

## Expected

- `.focus-list span { font-size: 11px; }`
- `.focus-list li { border-bottom-color: #2A2A2A; }`

## Verification

- Unit: source-level SCSS check — `.focus-list span` declares
  `font-size: 11px`, `.focus-list li` declares the `#2A2A2A` border
  colour.
- Visual: screenshot the rendered tile; the supporting-metric line
  should sit one type-step quieter and the row divider should fade
  into the tile background.
