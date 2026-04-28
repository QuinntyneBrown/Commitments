---
id: bug-045
title: Consistency Trend chart applies a uniform point border colour; design has a `#121212` stroke only on the highlighted "today" dot
status: Open
---

# Bug 045 — Consistency Trend chart point border per-state

**Status**: Open

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `e5bRb` →
`ltPlot`):

- `d0`–`d12` (regular trend dots): plain `#42A5F5` ellipses 6×6,
  **no stroke**.
- `todayDot`: a 14×14 ellipse `#42A5F5` with
  `stroke: { fill: '#121212', thickness: 3 }` plus an outer glow.

The implementation in `consistency-trend.controller.ts`:

```ts
pointBorderColor: SURFACE_TILE,   // #242424 applied to every point
pointBackgroundColor: ACCENT_CHART
```

The colour also doesn't match the design (`SURFACE_TILE` is the
tile surface `#242424`; the design uses the dashboard background
`BG_APP` `#121212`, which is a stop darker).

To match the .pen the implementation needs:
- regular dots: no border (transparent / 0-width).
- highlighted dot: `BG_APP` border at 3px width.

The `pointRadius` array pattern (already in use to vary highlight
size) extends naturally to per-point border colour and width.

The `BG_APP` token already exists in `commitments-ui/tokens.ts`
(`#121212`), so this is a token usage change — no new tokens
introduced. The outer glow remains out of scope (Chart.js doesn't
support per-point box-shadow without a custom plugin).

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect any dot — every one carries a `SURFACE_TILE` (`#242424`)
   1px-ish ring matching the tile background.
3. Compare to the .pen — only the today (last) dot has a darker
   `#121212` ring, 3px thick.

## Expected

```ts
pointBorderColor: points.map((_, i) => i === highlighted ? BG_APP : 'transparent'),
pointBorderWidth: points.map((_, i) => i === highlighted ? 3 : 0),
```

## Verification

- Unit: source-level TS check on
  `consistency-trend.controller.ts` — `pointBorderColor` is an
  array (or function) referencing `BG_APP`, and a
  `pointBorderWidth` declaration exists with `3` as one of its
  values.
- Visual: regular dots render edge-to-edge blue; the highlighted
  today dot has a clear `#121212` ring around it.
