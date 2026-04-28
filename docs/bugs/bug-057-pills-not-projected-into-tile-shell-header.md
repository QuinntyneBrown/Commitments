---
id: bug-057
title: Five tile pills lack the `tile-status` projection attribute, so they render in the body slot instead of the tile-shell header
status: Fixed
---

# Bug 057 — Pills not projected into the tile-shell header slot

**Status**: Fixed

## Fix

Added the `tile-status` attribute to the `<cui-status-pill>` in
five plugin tiles:

- weekly-focus-tile
- monthly-progress-tile
- outstanding-todos-tile
- relations-tile
- consistency-trend-tile

The shell's `<ng-content select="[tile-status]">` (introduced by
bug-018) catches each pill into the header's right slot,
matching every tile's .pen header layout.

`daily-results-tile` already had the attribute from bug-018; all
six in-scope plugin tiles now project their pills consistently
into the header.

`goal-metrics-tile` (in the dashboard plugin but outside
`docs/tiles/`) still projects into the body slot — candidate
for a follow-up consistency bug.

Coverage:
- Each affected tile's component spec asserts the pill carries
  the `tile-status` attribute.
- All 20 affected suites pass (141/141 — was 136/136 before).

## Description

Bug-018 added a named projection slot to `tile-shell`:

```html
<mat-card-header>
  …
  <ng-content select="[tile-status]"></ng-content>
</mat-card-header>
```

`daily-results-tile` was updated then to mark its pill with the
`tile-status` attribute, so the pill lands in the header on the
right side of the title — matching the .pen `drLive` placement.

The other five plugin tiles still project their `<cui-status-pill>`
without the attribute, so their pills fall into the default
`<ng-content>` inside `mat-card-content` and stack as the first
row of the body. This contradicts their .pen designs, where the
pill (or chip) sits **in the header on the right**:

- weekly-focus      — no pill in design (but if rendered, should
  sit in the header per the dashboard pattern)
- monthly-progress  — no pill in design (same)
- outstanding-todos — design's `otChip` ("4 OPEN") is in `otHd`
- relations         — no pill in design (same)
- consistency-trend — design's `ctLive` is in `ctHd`

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html`

## Reproduction

1. Render any of the five affected tiles.
2. Inspect the DOM — the `cui-status-pill` element is a child of
   `mat-card-content` (body), not `mat-card-header`.
3. Compare to the .pen — the pill (or equivalent chip) sits in
   the header row on the right.

## Expected

Each tile's `<cui-status-pill>` declares the `tile-status`
attribute:

```html
<cui-status-pill
  tile-status
  [variant]="…"
  …
></cui-status-pill>
```

The shell's `<ng-content select="[tile-status]">` then catches
the pill and renders it in the header row, where the design
specifies.

## Verification

- Unit: each affected tile's component spec asserts the pill in
  the template carries the `tile-status` attribute.
- Visual: every tile's pill now sits to the right of the title,
  not above the body content.
