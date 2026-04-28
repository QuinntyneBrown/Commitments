---
id: bug-087
title: Consistency Trend tile uses Material's default 16px padding where the design specifies 20px
status: Fixed
---

# Bug 087 — Consistency Trend padding 20 vs default 16

**Status**: Fixed

## Fix

Added a `--cui-tile-padding` custom property to tile-shell
(default `16px`, matching Material's mat-card defaults so every
existing tile renders unchanged). The header explicitly sets
`padding: var(--cui-tile-padding, 16px) var(--cui-tile-padding, 16px) 0`
and the body explicitly sets `padding-inline` and
`padding-bottom` to consume the same token (top stays as
`var(--cui-tile-body-gap, …)`).

`consistency-trend-tile`'s `:host` now declares
`--cui-tile-padding: 20px`, matching the .pen design's root
padding of 20.

The token pattern matches the existing tile-shell customization
slots (header-gap, body-gap, icon-color, icon-size). 41/41
tile-shell + consistency-trend specs green; 283/283 workspace.

## Description

`docs/tiles/consistency-trend/ui-design.pen`'s root frame
`XUZrO` specifies `padding: 20`. The other five tiles' designs
(daily-results, monthly-progress, outstanding-todos, relations,
weekly-focus) all specify `padding: 16`.

The implementation relies on Angular Material's
`mat-card-header` / `mat-card-content` defaults, which are 16px
all around. There is no per-tile padding override mechanism, so
every tile gets 16. The consistency-trend tile is therefore 4px
short of the design on every edge.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
  — introduce `--cui-tile-padding` (default 16px) and apply
    explicit padding on `.tile-shell__header` and
    `.tile-shell__body`, replacing Material's defaults.
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.spec.ts`
  — assert tile-shell consumes the new token.
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.scss`
  — add `:host { --cui-tile-padding: 20px }`.
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`
  — assert the override.

## Reproduction

Run the dashboard, add the Consistency Trend tile. Compared to
the .pen design at the same scale, the chart and surrounding
chrome sit 4px tighter against the tile edge than the design
shows.

## Expected

Tile-shell exposes a `--cui-tile-padding` token (default 16px)
that controls the outer padding on header and body. Every
existing tile inherits the default and renders unchanged.
Consistency-trend overrides to 20px, matching the .pen.

## Verification

- Unit (tile-shell CSS source): assert `.tile-shell__header`
  and `.tile-shell__body` consume `--cui-tile-padding`.
- Unit (consistency-trend CSS source): assert
  `:host { --cui-tile-padding: 20px }`.
- All existing tile specs continue to pass.
