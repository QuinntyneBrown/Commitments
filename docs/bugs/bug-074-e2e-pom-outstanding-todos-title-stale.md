---
id: bug-074
title: e2e Page Object Model and dashboard.spec.ts still reference the old "Outstanding To‑Dos" title (U+2011 hyphen) that bug-027 removed
status: Fixed
---

# Bug 074 — Outstanding Todos title rename broke e2e tests

**Status**: Fixed

## Fix

Replaced `'Outstanding To‑Dos'` (U+2011 hyphen) with
`'Outstanding Todos'` in five sites across the e2e suite:

- `dashboard.page.ts` — `DEFAULT_TILE_TITLES` constant
- `dashboard.spec.ts` — `addTileDialogLabels` expected list
- `dashboard.spec.ts` — three test names (lines 42, 54, 106)

A regression guard was added to
`outstanding-todos-tile.component.spec.ts` that reads both e2e
files and asserts neither matches `/Outstanding To.Dos/` (covers
both the regular and non-breaking hyphen forms). Future drift is
caught early in the unit-test layer.

`grep -rn 'Outstanding To.Dos' frontend/projects/commitments-app/e2e`
returns no matches; 9/9 outstanding-todos-tile specs pass.

## Description

Bug-027 changed the Outstanding Todos tile's title from
`"Outstanding To‑Dos"` (with the U+2011 non-breaking hyphen) to
`"Outstanding Todos"`. The Playwright e2e suite still expects the
old string in two places:

- `dashboard.page.ts:9` — `DEFAULT_TILE_TITLES` constant lists
  `'Outstanding To‑Dos'` (used by `expectDefaultDashboard()` to
  iterate and locate each tile by title).
- `dashboard.spec.ts:123` — same string in the
  `addTileDialogLabels` expected list.
- `dashboard.spec.ts:42`, `:54`, `:106` — three test names
  reference the old title cosmetically.

Any test calling `expectDefaultDashboard()` would fail to locate
the renamed tile; the add-tile-dialog catalog comparison would
fail likewise.

The fix is text-only: replace the U+2011 form with `"Outstanding
Todos"` everywhere in the e2e suite (constant, expected list, and
test names for consistency).

## Affected files

- `frontend/projects/commitments-app/e2e/pages/dashboard.page.ts`
- `frontend/projects/commitments-app/e2e/dashboard.spec.ts`

## Reproduction

```bash
grep -rn 'Outstanding To.Dos\|Outstanding To‑Dos' frontend/projects/commitments-app/e2e
```

Five matches.

## Expected

All five sites read `'Outstanding Todos'` — matching the new
component title.

## Verification

- Unit: source-level grep — neither e2e file matches the old
  `'Outstanding To‑Dos'` string (with U+2011) nor the
  `'Outstanding To-Dos'` form (with regular hyphen).
- e2e: `expectDefaultDashboard()` locates all five default tiles;
  the add-tile-dialog catalog test passes.
