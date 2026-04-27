# 071 — Monthly Progress bars use forced min-height; same flex pattern as 070

## Status

FIXED — `.bars { min-height: 120px }` → `min-height: 0`; bars now
render cleanly at the bottom of the cell with no overflow.

## Symptom

`monthly-progress-tile.component.scss`:

```scss
.bars {
  ...
  height: 100%;
  min-height: 120px;
}
```

At the default tile size (`{ cols: 3, rows: 2 }`) the cell is roughly
257×150. The body has ~100px of flex space left after the header, but
`.bars` forces 120px — same pattern as 070's `.plot { min-height: 220 }`,
making the bars overflow the cell's `overflow: hidden`.

## Fix

Drop `min-height: 120px` (keep `height: 100%` so the bars still fill
whatever vertical space the body gives them).

## Resolution

- [x] Visual screenshot pre-fix surfaced via earlier dashboard captures.
- [x] SCSS updated: `.bars { min-height: 120px }` → `min-height: 0`.
- [x] Visual screenshot post-fix shows the 4 bars cleanly at the
      bottom of the tile.
