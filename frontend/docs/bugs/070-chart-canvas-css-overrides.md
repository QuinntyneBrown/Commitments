# 070 — Consistency Trend chart squished by forced CSS canvas dimensions

## Status

FIXED — root cause was different from the initial hypothesis.

`.plot { min-height: 220px }` was forcing the chart container to keep
its 220px height even when the surrounding flex column had less room.
The body's available space (~237px) wasn't enough for the upper
content (~134px) plus a 220px chart, so the chart's lower portion
was clipped past the cell.

Changing `.plot { min-height: 220px }` → `min-height: 0` lets the
flex item shrink to its share of the body's remaining space; chart.js
(`responsive: true, maintainAspectRatio: false`) sizes the canvas to
fit. Visual screenshot post-fix shows the chart line traversing the
visible area.

## Symptom

The Consistency Trend tile renders the chart canvas at the right
*reported* size (411 × 220) but only a thin band of the actual line
is visible at the bottom of the tile. Most of the chart area appears
empty.

## Root cause

`consistency-trend-tile.component.scss`:

```scss
.plot {
  min-height: 220px;
  ...
}

.plot canvas {
  width: 100% !important;
  height: 100% !important;
}
```

Chart.js (in `responsive: true, maintainAspectRatio: false` mode) wants
to manage the `<canvas>` element's pixel dimensions directly via the
`width=""` / `height=""` HTML attributes. CSS `!important` overrides on
the same element cause the visible canvas size to differ from the
internal drawing buffer, so the line is drawn in a smaller logical
region than is rendered to the screen.

The Chart.js docs are explicit:

> Detecting when the canvas size changes can not be done directly from
> the canvas element. … For this reason, you should never set the size
> attributes manually or in the CSS.

## Fix

Drop the `.plot canvas` CSS rule. Let `.plot { min-height: 220px;
flex: 1 1 auto }` size the parent, and let Chart.js own the canvas
attributes.

## Resolution

- [x] Visual screenshot pre-fix shows chart squished to a sliver.
- [x] `.plot { min-height: 220px }` → `min-height: 0`.
- [x] Visual screenshot post-fix shows the chart traversing the area;
      28/28 dashboard + chart-tile e2e tests pass on lg-desktop.
