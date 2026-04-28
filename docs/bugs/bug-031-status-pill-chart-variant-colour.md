---
id: bug-031
title: Consistency Trend — LIVE pill renders pink (accent), but the .pen ctLive pill is info blue (#42A5F5)
status: Open
---

# Bug 031 — Status pill colour wrong for chart-variant tiles

**Status**: Open

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `ctLive`) shows
the `LIVE` pill in info blue:

- pill background `#42A5F522`
- dot `#42A5F5`
- text `LIVE` `#42A5F5`

The whole consistency-trend tile is themed around `--cui-info`
(`#42A5F5`) — icon, headline, dataset stroke. But
`status-pill.component.scss` only defines `live`, `review`, and
`neutral` variants. The `live` variant uses `--cui-accent`
(`#FF4081`, hot pink), so the tile renders a pink pulsing pill
inside a blue card — visually jarring.

The same shared pill is reused across all tiles, so the live
variant cannot simply be recoloured globally — `live` for the
non-chart tiles (e.g. daily-results) is correctly accent-pink
in the design system.

## Affected files

- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.ts`
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html`

## Reproduction

1. Render the Consistency Trend tile in live mode (default).
2. Observe a pink pulsing `LIVE` pill in the top-right of the header.
3. Compare to the .pen — the pill is info blue (`#42A5F5`).

## Expected

A new `chart` variant on the status pill that maps to `--cui-info`.
The consistency-trend tile's `pillVariant()` returns `'chart'` for
live mode and `'review'` for review mode (chart tiles never want the
pink accent).

## Verification

- Unit:
  - `status-pill.component.scss` declares a `.status-pill--chart`
    rule that references `var(--cui-info)`.
  - `consistency-trend-tile.component.ts` `pillVariant()` returns
    `'chart'` when mode is `'live'`, `'review'` when mode is
    `'review'`.
- Visual: the consistency-trend `LIVE` pill renders as info blue.
