---
id: bug-078
title: Relations tile renders a status pill the design and README do not include
status: Open
---

# Bug 078 — Relations tile shouldn't have a status pill

**Status**: Open

## Description

`docs/tiles/relations-tile/ui-design.pen`'s header (`rlHd`)
contains only the icon and the title-vertical group — no status
pill. The README's "Visual Description" lists shell title,
eyebrow, layout, and type colours — no pill mentioned.

`relations-tile.component.html` projects
`<cui-status-pill tile-status …>` into the tile-shell header
anyway, taking up header space the design reserves for nothing.

This is the same shape as bug-076 (Monthly Progress) — a tile
that's a "purely the body" design but inherited the pill from a
copy-paste of a tile that does need one.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.spec.ts` (invert the bug-057-style "projects pill" assertion)
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.ts` (drop unused `StatusPillComponent` import + `statusLabel` computed)

## Reproduction

1. Open the dashboard at <http://localhost:4200>.
2. The Relations tile shows a `LIVE` (or `REVIEW`) pill in its
   header.
3. The corresponding `ui-design.pen` shows no pill.

## Expected

Relations tile renders **no** pill in the header. Template drops
the `<cui-status-pill>`. Component class drops the unused
`StatusPillComponent` import and `statusLabel` computed.

## Verification

- Unit (template source): assert `<cui-status-pill` does **not**
  appear in `relations-tile.component.html`.
- The bug-057 "projects pill" spec is replaced with the inverted
  bug-078 assertion.
- All existing relations-tile specs continue to pass.
