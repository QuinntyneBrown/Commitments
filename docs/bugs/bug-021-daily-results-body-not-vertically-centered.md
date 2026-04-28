---
id: bug-021
title: Daily Results — metric value and label are not vertically centered between header and progress bar
status: Open
---

# Bug 021 — Daily Results body content not centered

**Status**: Open

## Description

In `docs/tiles/daily-results-tile/ui-design.pen`, the body frame (`drBody`)
has `layout: vertical, alignItems: center, justifyContent: center,
height: fill_container, width: fill_container, gap: 4`. The "7 / 9" value
and "commitments completed" label sit visually centered both horizontally
and vertically between the header and the progress bar.

The implementation in
`frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`
defines `.metric` as a plain `display: grid; gap: 4px;` block with no
`justify-items` or `align-items` and no flex/height stretching. As a
result, the value+label cling to the top of the body, leaving a large
empty gap between the label and the progress bar — and "7 / 9" is left-
aligned, not centered, when the body is wider than the value.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`
- (optional) `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.html`
  — wrap value+label in a flex column that fills available height.

## Reproduction

1. Render the Daily Results tile at its default `cols: 3, rows: 2` size.
2. Note the "7 / 9" sits high in the tile, with a large gap between
   "commitments completed" and the progress bar.
3. Compare to the .pen — value+label are vertically centered between
   header and progress bar.

## Expected

`.metric` fills the available body height and centers its children both
horizontally and vertically:

```scss
.metric {
  display: flex;
  flex: 1 1 auto;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 4px;
  text-align: center;
}
```

## Verification

- Unit: spec asserting `.metric` is `display: flex` and centered.
- Visual: screenshot vs .pen.
