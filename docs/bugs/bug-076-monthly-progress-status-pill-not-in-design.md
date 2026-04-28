---
id: bug-076
title: Monthly Progress tile renders a status pill that the design and README explicitly forbid
status: Fixed
---

# Bug 076 — Monthly Progress tile shouldn't have a status pill

**Status**: Fixed

## Fix

Dropped the projected `<cui-status-pill>` from the template and
removed the now-unused `StatusPillComponent` import plus the
`statusLabel` computed from the component class. `bindTileMode`
still wires the controller to the dashboard's mode/asOf signals
— mode is just no longer surfaced in the header.

The bug-057-style "projects pill" spec was inverted to the
bug-076 form (`<cui-status-pill` must NOT appear). All 15
monthly-progress specs and the full 260-test workspace suite are
green.

## Description

`docs/tiles/monthly-progress-tile/ui-design.pen`'s header
(`mpHd`) contains only the icon and the title-vertical group —
no status pill. The README is explicit:

> **No metric headline, no delta badge, no status pill** —
> purely the bar chart inside the standard `TileShellComponent`.

`monthly-progress-tile.component.html` nevertheless projects
`<cui-status-pill tile-status …>` into the tile-shell header.
That pill takes a non-trivial slot of header width that the
design reserves entirely for the title group.

`monthly-progress-tile.component.spec.ts` even has a now-stale
spec (bug-057-style) asserting that the pill *is* projected —
that assertion contradicts the design.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.spec.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.ts` (drop unused `StatusPillComponent` import + unused `pillVariant` / `statusLabel` computeds)

## Reproduction

1. Open the dashboard at <http://localhost:4200>.
2. The Monthly Progress tile shows a `LIVE` (or `REVIEW`) pill in
   its header.
3. The corresponding design `ui-design.pen` shows no pill.

## Expected

The Monthly Progress tile renders **no** pill in the header. The
component template drops the `<cui-status-pill>`. The component
class drops the unused `StatusPillComponent` import,
`pillVariant`, and `statusLabel` members.

## Verification

- Unit (template source): assert `<cui-status-pill` does **not**
  appear in `monthly-progress-tile.component.html`.
- Unit (TS source): the prior bug-057-style "projects pill" spec
  is replaced with the inverted assertion.
- All existing monthly-progress-tile specs continue to pass.
