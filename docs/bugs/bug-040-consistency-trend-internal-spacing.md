---
id: bug-040
title: Consistency Trend — header→metric and metric→chart gaps drift from the .pen tile gap of 14
status: Open
---

# Bug 040 — Consistency Trend internal vertical spacing

**Status**: Open

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `XUZrO`) declares
`gap: 14` between the tile's vertical children. The tile body has
three rows visible:

1. Header (`ctHd`)
2. Metric row (`ctMetric` — `78%` + caption + delta)
3. Chart (`e5bRb`)

Both gaps in the design are therefore 14px.

The implementation produces uneven gaps:

- `tile-shell.component.scss` `.tile-shell__body { padding-top: 10px; }`
  (set by bug-020 for the success/warning tiles whose .pens have
  gap 10).
- `.trend-row { margin-top: 6px; }` adds another 6.
- `.plot { margin-top: 12px; }`.

So:
- header → trend-row: `10 + 6 = 16px` (.pen wants 14)
- trend-row → plot:    `12px` (.pen wants 14)

A 2px error each way.

The fix has two parts: tile-shell needs a per-variant override so
chart tiles get 14px body padding-top, and the consistency-trend
SCSS needs to drop the redundant `.trend-row` margin and bring
`.plot` into alignment.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss`

## Reproduction

1. Render the Consistency Trend tile.
2. Measure the gap between the bottom of the header (title row)
   and the top of the metric headline.
3. Measure the gap between the bottom of the metric row and the top
   of the chart canvas.
4. Compare to the .pen — both gaps are 14px; implementation is
   16px / 12px.

## Expected

Tile-shell adds a chart-variant override:

```scss
.tile-shell--chart .tile-shell__body {
  padding-top: 14px;
}
```

And the consistency-trend tile drops the redundant margin and
matches:

```scss
.trend-row { margin-top: 0; }
.plot     { margin-top: 14px; }
```

## Verification

- Unit:
  - `tile-shell.component.spec.ts` asserts `.tile-shell--chart
    .tile-shell__body` declares `padding-top: 14px`.
  - `consistency-trend-tile.component.spec.ts` asserts `.plot`
    declares `margin-top: 14px` and `.trend-row` does **not**
    carry a `margin-top:` declaration.
- Visual: screenshot the tile vs the .pen — both internal gaps
  are 14px.
