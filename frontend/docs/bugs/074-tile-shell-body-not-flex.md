# 074 — `tile-shell__body` isn't flex; child `flex: 1 1 auto` rules are inert

## Status

OPEN.

## Symptom

After 070's fix (`.plot { min-height: 0 }`), the consistency-trend
chart still overflows the tile body. Measurements:

```
body:   237px tall, top 411, bottom 648
plot:   205px tall, top 585, bottom 791
canvas: 205px tall, top 585, bottom 791
```

Plot extends 142px past the body's bottom (and past the cell). The
chart canvas is correctly sized to its parent `.plot`, but `.plot`
itself does not shrink to share remaining space.

## Root cause

`tile-shell.component.scss`:

```scss
.tile-shell { display: flex; flex-direction: column; ... }
.tile-shell__body { flex: 1 1 auto; min-height: 0; padding-top: 14px; }
```

`.tile-shell__body` is a flex *child* of `.tile-shell` (good — it
gets the remaining vertical space). But it's not itself a flex
*container*, so its children (status pill, metric header, delta,
plot) flow as block elements and take their natural sizes. The plot's
own `flex: 1 1 auto; min-height: 0` is therefore **inert** — its
parent isn't flex, so flex rules don't apply.

## Fix

Make `.tile-shell__body` a vertical flex container:

```scss
.tile-shell__body {
  display: flex;
  flex-direction: column;
  flex: 1 1 auto;
  min-height: 0;
  padding-top: 14px;
}
```

Now any tile body whose final child uses `flex: 1 1 auto; min-height: 0`
(currently just `.plot` in consistency-trend) will take remaining space
and shrink instead of overflowing.

## Resolution

- [ ] Visual screenshot pre-fix.
- [ ] tile-shell SCSS updated.
- [ ] Visual screenshot post-fix shows the chart fully inside the cell.
- [ ] Other tiles unaffected (visual + e2e regression).
