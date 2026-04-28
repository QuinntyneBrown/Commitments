---
id: bug-081
title: outstanding-todos-tile.tileMetadata.displayName still has the U+2011 hyphen ("Outstanding To‑Dos") that bug-074 was supposed to remove
status: Open
---

# Bug 081 — Outstanding Todos tile metadata still has U+2011 hyphen

**Status**: Open

## Description

bug-074 propagated the `Outstanding To‑Dos` → `Outstanding Todos`
rename to the e2e POM (`DEFAULT_TILE_TITLES`) and the four
dashboard spec sites. It missed the **source of truth** for the
add-tile catalog: the component's own
`tileMetadata.displayName`.

`outstanding-todos-tile.component.ts:27`:

```ts
displayName: 'Outstanding To‑Dos',
```

The `‑` is U+2011 (non-breaking hyphen). The add-tile dialog
renders `tile.displayName` directly
(`add-tile-dialog.component.html:32`), so the dialog shows
"Outstanding To‑Dos" — which fails the bug-074 e2e expectation
of "Outstanding Todos".

The bug-074 cross-file regression spec only checks the e2e POM
and the dashboard.spec.ts, so it didn't catch the stale metadata.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.ts`
  — fix `displayName`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.spec.ts`
  — extend the bug-074 cross-file guard to also cover this file

## Reproduction

```bash
grep -n "Outstanding To.Dos" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.ts
```

Returns one match on the `displayName` line.

## Expected

`displayName` reads `'Outstanding Todos'` (regular characters, no
U+2011 hyphen). The bug-074 cross-file spec is extended so the
guard now also reads `outstanding-todos-tile.component.ts` and
asserts neither the regular `Outstanding To-Dos` form nor the
U+2011 `Outstanding To‑Dos` form appears.

## Verification

- Unit: extended bug-074-style spec reads the component's own .ts
  source and grep-checks for `Outstanding To.Dos`.
- All existing outstanding-todos-tile specs continue to pass.
