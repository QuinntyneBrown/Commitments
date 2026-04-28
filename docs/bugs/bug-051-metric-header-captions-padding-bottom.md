---
id: bug-051
title: Metric Header captions column has 4px padding-bottom; design specifies 8px
status: Fixed
---

# Bug 051 — Metric Header captions baseline lift

**Status**: Fixed

## Fix

`metric-header.component.scss` —
`.metric-header__captions` padding-bottom: 4px → 8px.

Mirrors the .pen `ctMcol` frame's
`padding: [0, 0, 8, 0]`. Bug-032's earlier 4px was a guess; this
is the authoritative value.

Coverage:
- New spec `lifts the captions column 8px to align with the value
  baseline` reads the SCSS source and asserts `padding-bottom:
  8px`.
- All 20 affected suites pass (124/124 — was 123/123 before).

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `JPmts` =
`ctMcol`, the captions column inside `ctMetric`) declares
`padding: [0, 0, 8, 0]` — 8px bottom padding inside the column
frame. With the parent `ctMetric` frame setting
`alignItems: end`, that 8px lifts the captions up so the bottom
of the "Peak X% / Low Y%" text sits closer to the **text baseline**
of the value (rather than its descender), matching the visual feel
of the .pen.

The implementation in
`metric-header.component.scss` (added by bug-032) uses
`padding-bottom: 4px` — only half the design's lift, so the
captions sit 4px lower than the .pen specifies.

## Affected files

- `frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.scss`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect `.metric-header__captions` — `padding-bottom: 4px`.
3. Compare to the .pen — the column carries 8px bottom padding,
   visibly lifting the captions 4px higher relative to the value's
   baseline.

## Expected

```scss
.metric-header__captions {
  …
  padding-bottom: 8px;
}
```

## Verification

- Unit: source-level SCSS spec — `.metric-header__captions`
  declares `padding-bottom: 8px`.
- Visual: the captions ("today" / "Peak/Low") read closer to the
  value's text baseline.
