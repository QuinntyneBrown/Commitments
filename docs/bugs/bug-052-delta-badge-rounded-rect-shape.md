---
id: bug-052
title: Delta-badge renders as a 24×pill (999px corner-radius); design specifies 20px rounded rect with 4px corners
status: Open
---

# Bug 052 — Delta-badge dimensions and corner-radius

**Status**: Open

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `EP7NN` =
`ctDelta`) shapes the delta badge as a small rounded rectangle:

| Property       | .pen value | Current impl |
| -------------- | ---------- | ------------ |
| corner-radius  | `4`        | `999px` (pill) |
| height         | `20`       | `min-height: 24px` |
| padding        | `[0, 6]`   | `0 8px`       |
| gap            | `4`        | `6px`         |

The implementation in `delta-badge.component.scss` reads as a
generic Material chip-style pill — the design wants a tighter
rounded-rect closer to a tag than a chip.

Out of scope (separate bugs to be filed if the design discrepancy
warrants them):
- The .pen ctDelta also includes an `arrow_upward` 12×12 leading
  icon coloured `#66BB6A`. The current `.delta-badge` template
  has no icon slot.
- The .pen renders the whole `+12% vs prior 14d` string as a
  single Inter 11/500 line. The current template splits the value
  (700 weight) from the caption (500 weight, secondary colour),
  so the rendered output diverges typographically.

This bug fixes only the geometric/chrome match — corner-radius,
height, padding, gap.

## Affected files

- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.scss`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect `.delta-badge` — `border-radius: 999px`,
   `min-height: 24px`, `padding: 0 8px`, `gap: 6px`.
3. Compare to the .pen — 4px corners, 20px height, 6px horizontal
   padding, 4px gap.

## Expected

```scss
.delta-badge {
  display: inline-flex;
  align-items: baseline;
  gap: 4px;
  min-height: 20px;
  padding: 0 6px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: 600;
}
```

## Verification

- Unit: source-level SCSS spec — `.delta-badge` declares
  `border-radius: 4px`, `min-height: 20px`, `padding: 0 6px`,
  and `gap: 4px`.
- Visual: the badge reads as a small rounded rectangle rather
  than a chip pill.
