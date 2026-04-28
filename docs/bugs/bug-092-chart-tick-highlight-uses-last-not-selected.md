---
id: bug-092
title: Consistency Trend tick label highlight always tracks the LAST date even when review mode selected a different one
status: Open
---

# Bug 092 — chart tick label vs point highlight mismatch

**Status**: Open

## Description

`ConsistencyTrendController.highlightedIndex` is mode-aware:

- live mode → last point's index
- review mode → the index of the point matching
  `selectedReviewDate`, or `-1` if not in window

The dataset uses this signal to enlarge the matching data point
(7px radius, BG_APP-stroked).

The X-axis tick *label* highlight is computed independently in
the component:

```ts
const isLastTick = (ctx) => ctx.index === ctx.scale.ticks.length - 1;
…
color: (ctx) => isLastTick(ctx) ? ACCENT_CHART : TEXT_MUTED,
font:  (ctx) => ({ weight: isLastTick(ctx) ? 700 : 400 }),
```

So in **review** mode the enlarged data point lands on the
selected date, but the bold-blue tick *label* still highlights
the last date — they disagree, and a user looking at "Apr 18"
(say) sees the dot pulled out at "Apr 18" while the bold blue
label is still "Apr 25". The design's xLabRow only has the
literal last date in info-blue/700 because the design represents
live mode; the dynamic intent is "highlight the date the data
point highlights."

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

1. Open the dashboard, add the Consistency Trend tile.
2. Switch the dashboard to review mode and pick a date a few
   days back via the scrubber.
3. The enlarged data point lands on that date, but the tick
   label below it stays grey/regular while the last date
   remains blue/700.

## Expected

The tick label highlight tracks `controller.highlightedIndex()`
(closure over the controller in the component). When
`highlightedIndex()` returns `-1`, no tick label is highlighted.

## Verification

- Unit (TS source): assert the tick `color` and `font`
  scriptable functions reference `controller.highlightedIndex`,
  not `isLastTick`. The `isLastTick` helper is removed since
  it has no remaining callers.
- Existing consistency-trend specs continue to pass (the
  bug-047 spec asserts ACCENT_CHART + weight 700 — both still
  appear, just with the new index source).
