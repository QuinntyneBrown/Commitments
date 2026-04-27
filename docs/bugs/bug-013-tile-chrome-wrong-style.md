---
id: bug-013
title: Edit-mode tile chrome uses neutral label + surface-3 close; design specifies accent DRAG chip + warn close
status: In Progress
---

# Bug 013 — Edit-mode tile chrome visual mismatch

**Status**: Fixed ✓

## Description

In edit mode the tile chrome (overlay on each gridster tile) shows:
- **Left**: a neutral pill label showing the tile name (`--cui-surface-3` bg, divider border)
- **Right**: a close button with `--cui-surface-3` background (warn color only on hover)

The design (`ui-design.pen` → `Dashboard-Tile/Editable` → `KB9Mx`) specifies:
- **Top-left**: a "DRAG" chip with `drag_indicator` icon + "DRAG" text, `$accent` fill (`--cui-accent`), `$on-accent` color, `$r-sm` radius, 36×20px
- **Top-right**: a close button with `close` icon, `$warn` fill (`--cui-warn`), `$r-full` radius, 22×22px

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.html`
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-grid/dashboard-grid.component.scss`
- `frontend/projects/commitments-app/e2e/dashboard.spec.ts` (tile-chrome test needs updating)

## Fix

1. Replace `.tile-chrome__label` (tile name pill) with a DRAG chip (`drag_indicator` icon + "DRAG" text, accent fill).
2. Update `.tile-chrome__button` → `.tile-chrome__remove` to use `$warn` fill by default (not just on hover).
