---
id: 041
title: Outstanding To-Dos description text is mid-word clipped at the tile bottom - apply line-clamp ellipsis
status: open
discovered: 2026-04-27
flow: dashboard-layout
severity: low
---

# Outstanding To-Dos description text is mid-word clipped at the tile bottom — apply line-clamp ellipsis

## Summary

The Outstanding To-Dos tile's body copy
`"items need attention before the next review."` doesn't fit
inside the default `cols: 3, rows: 2` gridster footprint
(~140 px tall after header + 58 px metric). The browser
chops the text mid-sentence at the gridster-item's
`overflow: hidden` boundary — the user sees
`"items need attention before the next…"` with no ellipsis,
just a hard clip.

`docs/ui-design.pen` `Dashboard-Tile` (id `0UH2Q`) gives each
tile a fixed 200 px height; the design's wider footprint
absorbs the body copy on two lines. We can't grow the tile
without breaking saved layouts, but we can make the truncation
intentional with a `-webkit-line-clamp: 2` ellipsis so users
see a clean `…` instead of a sliced word.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the bottom of the Outstanding To-Dos tile.

**Expected:** the copy ends with `…` after a complete word.
**Actual:** the copy is sliced mid-sentence with no ellipsis;
the page simply cuts at the tile boundary.

Screenshot: `screenshots/dashboard-actual-1280.png`.

## Fix outline (radically simple)

Add four CSS declarations to the existing `.todo-copy` rule in
`projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss`:

```scss
.todo-copy {
  …
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
```

The `-webkit-` prefix is the broadly-supported invocation that
Chrome / Edge / Safari / Firefox all honour. No HTML / TS
change.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`outstandingTodosCopyTextOverflow()` returning the computed
`text-overflow` property and assert it is `ellipsis` after the
fix. Currently the property defaults to `clip` (the
browser's hard-cut behaviour) so the test fails pre-fix and
passes post-fix.
