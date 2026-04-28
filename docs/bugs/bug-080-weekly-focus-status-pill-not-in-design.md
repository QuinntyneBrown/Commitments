---
id: bug-080
title: Weekly Focus tile renders a status pill the design and README do not include
status: Open
---

# Bug 079 — Weekly Focus tile shouldn't have a status pill

**Status**: Open

## Description

`docs/tiles/weekly-focus-tile/ui-design.pen`'s header (`wfHd`)
contains only the icon and the title-vertical group. The
README's Visual Description lists shell title, eyebrow, body
shape, and layout — no pill mentioned.

`weekly-focus-tile.component.html` projects
`<cui-status-pill tile-status …>` into the tile-shell header
anyway. Same shape as bug-076 (Monthly Progress) and bug-078
(Relations) — a "purely the body" tile that inherited a pill
from a copy-paste of a tile that does need one.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.spec.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.ts` (drop unused `StatusPillComponent` import + `statusLabel` computed)

## Reproduction

1. Open the dashboard at <http://localhost:4200>.
2. Weekly Focus shows a `LIVE` (or `REVIEW`) pill in its header.
3. The corresponding `ui-design.pen` shows no pill.

## Expected

Weekly Focus tile renders **no** pill in the header. Template
drops the `<cui-status-pill>`. Component class drops the unused
`StatusPillComponent` import and `statusLabel` computed.

## Verification

- Unit (template source): assert `<cui-status-pill` does **not**
  appear in `weekly-focus-tile.component.html`.
- The bug-057 "projects pill" spec is replaced with the inverted
  bug-080 assertion.
- All existing weekly-focus specs continue to pass.
