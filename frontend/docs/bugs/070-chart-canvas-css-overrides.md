# 070 — Consistency Trend chart squished by forced CSS canvas dimensions

## Status

INVESTIGATED, NOT FIXED — removing the canvas `!important` rules did
not change the visual; the chart is still rendered in a thin band.
Needs deeper investigation (possibly Chart.js initial-resize timing,
or the `.plot` container not actually getting the 220px it claims).
Left open for a future iteration.

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

- [ ] Visual screenshot pre-fix.
- [ ] SCSS rule deleted.
- [ ] Visual screenshot post-fix shows the line spanning the chart area.
