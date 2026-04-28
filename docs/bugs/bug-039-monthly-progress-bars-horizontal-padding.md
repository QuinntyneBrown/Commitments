---
id: bug-039
title: Monthly Progress — bars container has no horizontal padding so bar columns drift 4px from the W1-W4 label columns
status: Fixed
---

# Bug 039 — Monthly Progress bars and labels columns misaligned by 4px

**Status**: Fixed

## Fix

`.bars` in `monthly-progress-tile.component.scss` now declares
`padding: 0 4px`, mirroring `.bar-labels`'s `padding: 0 4px` and
the .pen `mpBars` frame's `padding: [8, 4, 0, 4]`. Both grid
containers therefore have the same inner width, so their four
`repeat(4, 1fr)` columns share x-positions — bar centres align
with W1-W4 label centres.

Top/bottom padding stays 0 — `align-items: end` already pins bars
to the bottom and `.bar-labels { margin-top: 8px }` provides the
8px gap between rows.

Coverage:
- New spec `insets bars horizontally to align with the bar-labels
  row (bug-039)` reads the SCSS source and asserts `.bars`
  declares a `padding` containing `4px`.
- All 20 affected suites pass (101/101 — was 100/100 before).

## Description

`docs/tiles/monthly-progress-tile/ui-design.pen` (frame `mpBars`)
declares `padding: [8, 4, 0, 4]` — i.e. 4px horizontal inset matching
the `mpLabels` frame's `padding: [0, 4]`. Both rows therefore have
the same inner width, so `repeat(4, 1fr)` divides them into columns
that line up vertically: bar 1 sits directly above the `W1` label.

The implementation in `monthly-progress-tile.component.scss` matches
on labels but not on bars:

```scss
.bars {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  align-items: end;
  gap: 12px;
  height: 100%;
  min-height: 0;
  /* no horizontal padding */
}

.bar-labels {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 12px;
  margin-top: 8px;
  padding: 0 4px;     /* ← inset that .bars doesn't share */
  …
}
```

With the bars row 8px wider than the labels row, the bar columns
sit 4px further left than the matching label columns. Bar 1 is
~4px off-centre from `W1`, etc.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`

## Reproduction

1. Render the Monthly Progress tile.
2. Compare the centre of bar 1 to the centre of the `W1` label
   beneath it — they're offset by ~4px.
3. Compare to the .pen — bars are inset 4px horizontally,
   matching the labels.

## Expected

```scss
.bars {
  …
  padding: 0 4px;
  …
}
```

(top/bottom padding stays at 0 — the `align-items: end` already
pins bars to the bottom; the labels row carries the 8px gap via
its own `margin-top: 8px`.)

## Verification

- Unit: source-level SCSS spec — `.bars` declares
  `padding: 0 4px` (or equivalent that yields 4px horizontal).
- Visual: screenshot the tile vs the .pen — bar centres align
  with label centres.
