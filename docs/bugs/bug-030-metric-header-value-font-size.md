---
id: bug-030
title: Metric Header value renders at 42px; consistency-trend design specifies 48px
status: Open
---

# Bug 030 — Metric Header value font size

**Status**: Open

## Description

`cui-metric-header` is the shared UI primitive that renders a large
metric value plus a smaller caption. It is currently used by exactly
one consumer — `consistency-trend-tile.component.html` — and its
design (`docs/tiles/consistency-trend/ui-design.pen`, frame `ctMetric`
text node `M6f6C`) specifies the headline value at:

```
content: "78%"
fontFamily: Inter
fontSize: 48
fontWeight: 700
fill: #42A5F5
```

The implementation in
`frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.scss`
styles `.metric-header__value` as:

```scss
font-size: 42px;
font-weight: 300;
```

— a 6px shrink and a near-opposite weight.

For weight, the per-accent rules already pick the right colour
(`metric-header--chart` → info blue), so leaving the colour alone is
fine. Font size and weight are the only deviations.

## Affected files

- `frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.scss`

Blast radius: only `ConsistencyTrendTileComponent` projects
`<cui-metric-header>`, so bumping the primitive does not affect any
other tile.

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect the headline number (e.g. `78%`) — computed `font-size`
   is 42px, weight 300.
3. Compare to the .pen — the same number is rendered at 48px /
   weight 700.

## Expected

```scss
.metric-header__value {
  font-size: 48px;
  font-weight: 700;
}
```

## Verification

- Unit: source-level SCSS spec — `.metric-header__value` declares
  `font-size: 48px` and `font-weight: 700`.
- Visual: screenshot the consistency-trend tile vs the .pen.
