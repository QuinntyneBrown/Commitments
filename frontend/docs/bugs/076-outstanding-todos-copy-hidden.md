# 076 — Outstanding To-Dos description is invisible at default tile size

## Status

OPEN — surfaced via dashboard screenshot at lg-desktop default state.

## Symptom

The Outstanding To-Dos tile shows just the big "4" count; the
`"items need attention before the next review."` description is
nowhere on screen.

Measured at the default size (`{ cols: 3, rows: 2 }` → cell ≈ 227×150):

```
tile body:  191 × 65
.todo-count:159 × 58   (font-size: 58px)
.todo-copy: 159 × 0    (line-clamped to nothing)
```

The 58px-tall count exhausts the available 65px body, and the
two-line `-webkit-line-clamp: 2` on `.todo-copy` collapses to height 0.

## Fix

Reduce `.todo-count { font-size: 58px }` so the copy below has room
to show. 40px is in line with the design's "4" / "12" sizing in
`fJpM0`. The line-clamp keeps the copy compact (max 2 lines).

## Resolution

- [ ] Visual screenshot pre-fix.
- [ ] SCSS updated.
- [ ] Visual screenshot post-fix shows both the count and the copy.
