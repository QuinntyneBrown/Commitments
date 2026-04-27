# 060 — Dashboard tiles render squished (~60px tall) on lg+ viewports

## Status

OPEN — surfaced via visual capture at 1280×800 and 1920×1080
(`frontend/docs/bugs/screenshots/index-actual-{1280,1920}.png`).

## Symptom

The five default tiles (Daily Results, Weekly Focus, Monthly Progress,
Outstanding To‑Dos, Relations) render in the upper-left of the grid as a
4×2 row of very short cards. Most tile bodies are clipped — only the
header strip ("TODAY", "THIS WEEK", …) and the title fit; metrics and
copy fall outside the cell. The lower 60–80% of the grid area is empty.

At 768×1024 (tablet) the tiles look correct because the grid wraps and
each cell scales to a sensible height.

The existing `dashboard.spec.ts` 47/47 stays green because none of those
tests measure `gridster-item` height.

## Root cause

`buildGridOptions` in `dashboard-layout.store.ts:129`:

```ts
gridType: 'fit',
margin: 24,
outerMargin: true,
minCols: DEFAULT_COLS,   // 12
maxCols: DEFAULT_COLS,   // 12
minRows: DEFAULT_ROWS,   //  8
maxRows: 24
```

With `gridType: 'fit'`, gridster divides the container height by
`maxRows` to compute the per-row pixel height. At 1280×800 the grid's
container is ~720px tall, so:

```
rowHeight = (720 - margins) / 24 ≈ 28px
tileHeight (rows: 2) ≈ 56px + 1 margin ≈ 80px
```

That matches the squished cards in the screenshots. `maxRows: 24` was
out of step with `minRows: DEFAULT_ROWS` (8). The default catalog only
ever uses 4 rows, so 24 over-allocates 6× the vertical space.

## Fix

Set `maxRows: DEFAULT_ROWS` (8) so the visible grid is the same height
as the natural minimum. Each row then becomes ~85px and 2-row tiles
~180px — matching what the design and tablet stack already render.

If a future flow needs more rows, prefer `gridType: 'verticalFixed'`
with an explicit `fixedRowHeight` so the grid scrolls instead of
shrinking each row.

## Resolution

- [ ] Failing test verified (visual-only at first; add a height assertion).
- [ ] Options updated.
- [ ] Test verified passing post-fix; visual screenshot at lg+ confirms.
