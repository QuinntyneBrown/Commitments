---
id: bug-058
title: goal-metrics-tile pill still projects into the body slot; lone holdout after bug-057's pill-header sweep
status: Fixed
---

# Bug 058 — goal-metrics-tile pill not projected into the tile-shell header

**Status**: Fixed

## Fix

`goal-metrics-tile.component.html` adds `tile-status` to the
`<cui-status-pill>`. With the change, all seven dashboard-plugin
tiles project their pills consistently into the tile-shell
header named slot.

Coverage:
- New `goal-metrics-tile.component.spec.ts` (template-source
  describe block) asserts the pill carries the attribute.
- All 21 affected suites pass (142/142 — was 141/141 before).

## Description

Bug-018 introduced the named `[tile-status]` projection slot in
`tile-shell`'s header. Bug-057 then applied the projection
attribute to the five plugin tiles in `docs/tiles/`
(weekly-focus, monthly-progress, outstanding-todos, relations,
consistency-trend). `daily-results-tile` already had it from
bug-018.

`goal-metrics-tile` is also a dashboard-plugin consumer of
`tile-shell`, but it isn't in `docs/tiles/` so it sat outside
bug-057's scope. Its pill still falls into the body slot:

```html
<cui-status-pill
  [variant]="controller.mode()"
  [pulse]="controller.mode() === 'live'"
  [label]="statusLabel()"
></cui-status-pill>
```

Cross-plugin consistency: every other plugin tile renders its
pill in the header. Goal-metrics is the lone holdout.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.html`

## Reproduction

1. Render the Goal Metrics tile.
2. Inspect the DOM — the `cui-status-pill` is a child of
   `mat-card-content`, not `mat-card-header`.
3. Compare to the other six dashboard-plugin tiles — pill in
   header.

## Expected

```html
<cui-status-pill
  tile-status
  [variant]="…"
  …
></cui-status-pill>
```

## Verification

- Unit: source-level template check on
  `goal-metrics-tile.component.html` — pill carries the
  `tile-status` attribute.
- Visual: pill renders to the right of the title, in line with
  the other plugin tiles.
