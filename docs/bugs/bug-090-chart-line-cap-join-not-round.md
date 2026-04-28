---
id: bug-090
title: Consistency Trend chart line uses Chart.js default butt/miter cap+join, design specifies round
status: Fixed
---

# Bug 090 — chart line cap/join not round

**Status**: Fixed

## Fix

Added `borderCapStyle: 'round'` and `borderJoinStyle: 'round'`
to the `chartDataset` returned by `ConsistencyTrendController`.
The line's endings now end in soft half-circles (matching the
design's `cap: round`) and any sharp inflection between curves
rounds smoothly (matching `join: round`) — replacing Chart.js
defaults of butt/miter. 287/287 workspace tests green.

## Description

`docs/tiles/consistency-trend/ui-design.pen`'s `lineStroke`
specifies:

```
{ cap: "round", join: "round", thickness: 2.5, fill: "#42A5F5" }
```

The implementation's `chartDataset` (in
`consistency-trend.controller.ts`) sets `borderColor`,
`borderWidth: 2.5`, and `tension: 0.35`, but does not set
`borderCapStyle` or `borderJoinStyle`. Chart.js defaults are
`borderCapStyle: 'butt'` and `borderJoinStyle: 'miter'`.

With a 2.5px line and smooth curves the difference is visible
at line endings (square vs rounded) and at any inflection point
where curves don't fully smooth (sharp miter spikes vs rounded).
The design's rounded cap/join is part of the soft, modern feel
of the chart.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.spec.ts`

## Reproduction

```bash
grep -n "borderCapStyle\|borderJoinStyle" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts
```

No matches.

## Expected

The dataset includes:

```ts
borderCapStyle: 'round',
borderJoinStyle: 'round',
```

## Verification

- Unit (TS source): assert the dataset declaration contains both
  `borderCapStyle: 'round'` and `borderJoinStyle: 'round'`.
- Existing controller specs continue to pass.
