---
id: bug-014
title: Tile shell header is missing the icon that appears next to the title in the design
status: In Progress
---

# Bug 014 — Tile shell header missing icon

**Status**: Fixed ✓

## Description

The `commitments-tile-shell` component renders a header with only an eyebrow label and title.
The design (`ui-design.pen` → `Dashboard-Tile` → `0UH2Q` → `tileHead`) shows an icon
(Material Symbols Rounded, `$primary` color, 20×20) displayed inline to the left of the title.

Every registered tile already stores its icon name in `tileMetadata.icon` (fixed in Bug 008)
but that icon is never passed into `tile-shell` for rendering.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.ts` — add `icon` input
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.html` — render icon
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss` — icon style
- All tile component templates (8 tiles) — pass `icon` attribute

## Fix

1. Add `readonly icon = input('')` to `TileShellComponent`.
2. Render `<span class="material-symbols-rounded tile-shell__icon">{{ icon() }}</span>` 
   inline with the title (inside a `.tile-shell__title-row` flex wrapper).
3. Style `.tile-shell__icon` with `color: var(--cui-primary)`, `font-size: 20px`.
4. Update each tile template to pass the icon string.
