---
id: 035
title: Outstanding To Dos tile title wraps awkwardly - design uses hyphenated "Outstanding To-Dos"
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: c02ffc3
flow: dashboard-layout
severity: low
---

# Outstanding To Dos tile title wraps awkwardly — design uses hyphenated "Outstanding To-Dos"

## Summary

The `commitments-outstanding-todos-tile` declares
`title="Outstanding To Dos"` (three space-separated words). At
the default lg-desktop layout the tile is ~210 px wide, which
forces the title to wrap as **"Outstanding To"** + **"Dos"** —
an awkward break that splits the noun phrase
("To Dos" is the term).

`docs/ui-design.pen` → `Dashboard — LG (1280)` (id `OxYKj`)
labels the same tile **"Outstanding To-Dos"** (hyphenated).
The hyphen treats `To-Dos` as a single token: when the title
wraps, it now breaks as **"Outstanding"** + **"To-Dos"** which
is the natural English line break and matches the design.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Look at the Outstanding To Dos tile.

**Expected:** title reads "Outstanding To-Dos" (hyphenated),
wrapping cleanly between the two words.
**Actual:** title reads "Outstanding To Dos" with the line
break landing between "To" and "Dos".

## Fix outline (radically simple)

`replace_all` `'Outstanding To Dos'` → `'Outstanding To-Dos'`
across the four files that own the title:

  - `projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.html`
  - `projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.ts`
  - `projects/commitments-app/e2e/dashboard.spec.ts`
  - `projects/commitments-app/e2e/pages/dashboard.page.ts`

The unused `to-do-dashboard-card.component.ts` is dormant code
(no route renders it) and is left alone for a separate
cleanup.

## Tests to add (failing first)

The existing dashboard suite already asserts the title via
`tileByTitle('Outstanding To Dos')` and `DEFAULT_TILE_TITLES`.
Updating both assertions and the production literal in the
same commit keeps the suite green; running them pre-fix shows
the title text mismatch (the fix is value-correcting).
