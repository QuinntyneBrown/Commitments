# 063 — Add Tile dialog leaves ~250px empty below tile cells (fixed `height: 620px`)

## Status

OPEN — surfaced via visual capture
(`frontend/docs/bugs/screenshots/add-tile-dialog-1280.png`) compared against
design frame `a2Cjz`.

## Symptom

The running Add Tile dialog renders the 7 default-mode tiles in a 4+3
grid (rows 1 and 2), then leaves a tall blank band before the
CANCEL / ADD TILE footer. Design frame `a2Cjz` shows the dialog
sized to its content with no extra empty space.

## Root cause

`add-tile-dialog.component.scss`:

```scss
.add-tile-dialog {
  height: 620px;          // <- forces a tall dialog
  max-height: 90vh;
  ...
}

.add-tile-dialog__grid {
  flex: 1 1 auto;          // grid expands to fill
  overflow-y: auto;
  ...
}
```

With 7 cells × 120px in 4 columns, the grid only needs ~250px, but
the dialog forces 620px so the grid stretches and shows white space
below the last row.

## Fix

Drop the fixed `height: 620px`. Keep `max-height: 90vh` so the dialog
never exceeds the viewport (and the grid still scrolls if a future
catalog grows past the viewport). The dialog will then size to the
header + grid + footer, matching the design.

## Resolution

- [ ] Visual screenshot captured pre-fix.
- [ ] SCSS updated.
- [ ] Visual screenshot post-fix confirms tight dialog.
