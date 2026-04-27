---
id: 040
title: Outstanding To-Dos title still breaks at the regular hyphen - use U+2011 so the wrap stays between words
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: e6489b3
flow: dashboard-layout
severity: low
---

# Outstanding To-Dos title still breaks at the regular hyphen — use U+2011 so the wrap stays between words

## Summary

Bug 035 hyphenated the tile title from "Outstanding To Dos" to
"Outstanding To-Dos" so the break point would land between the
two words. The browser however treats `-` (U+002D, regular
hyphen) as a soft line-break opportunity, so at the default
~210 px tile width the title now wraps as **"Outstanding To-"**
+ **"Dos"** — slightly better than before but still mid-token.

The intended behaviour is the wrap landing as **"Outstanding"**
+ **"To-Dos"**. Swapping the literal `-` for U+2011
(non-breaking hyphen, `‑`) makes the token unbreakable: the
browser keeps `To‑Dos` together and breaks before it instead.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the Outstanding To-Dos tile title.

**Expected:** "Outstanding" on line 1, "To-Dos" on line 2.
**Actual:** "Outstanding To-" on line 1, "Dos" on line 2.

Screenshot: `screenshots/dashboard-actual-1280.png`.

## Fix outline (radically simple)

`replace_all` the literal `Outstanding To-Dos` → `Outstanding To‑Dos`
(only the hyphen between `To` and `Dos` flips to `‑`,
U+2011) across the four owner files:

  - `outstanding-todos-tile.component.html` (title attribute)
  - `outstanding-todos-tile.component.ts` (TileMetadata.displayName)
  - `e2e/dashboard.spec.ts` (text-match assertions)
  - `e2e/pages/dashboard.page.ts` (DEFAULT_TILE_TITLES)

Visually the character is identical to a regular hyphen; only
the line-break behaviour changes.

## Tests to add (failing first)

The same dashboard tests already pin this title via
`tileByTitle()` and `DEFAULT_TILE_TITLES`. Updating both the
production literal and the test literals in one commit keeps
the suite green; running pre-fix shows the awkward wrap in the
screenshot.
