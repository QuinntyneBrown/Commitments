---
id: bug-089
title: Outstanding Todos body container is missing the 4px inner padding the design specifies
status: Fixed
---

# Bug 089 — outstanding-todos body padding

**Status**: Fixed

## Fix

Added `padding: 4px` to `.todo-body`, matching the design's
`otBody.padding`. The count and copy now sit 4px inside the
body container — restoring the small inner inset the design
specifies. 285/285 workspace tests green.

## Description

`docs/tiles/outstanding-todos-tile/ui-design.pen`'s body frame
`fEUu5` (`otBody`) specifies:

```
{
  alignItems: "center",
  gap: 14,
  padding: 4,
  …
  children: [{ "4" }, { "items need attention…" }]
}
```

The `padding: 4` is an inner inset that pushes the count and
copy 4px in from the body container's edges.

`outstanding-todos-tile.component.scss:5` — `.todo-body` —
matches `align-items: center` and `gap: 14px` but omits the
`padding: 4px`. As a result, the count and copy sit flush
against the body container, which sits flush against the tile
shell's body padding. That's 4px tighter than the design on
every side of the body content.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.spec.ts`

## Reproduction

```bash
grep -A 4 "\\.todo-body" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.scss
```

The `.todo-body` block has no `padding` declaration.

## Expected

`.todo-body { padding: 4px }`, matching `otBody.padding`.

## Verification

- Unit (CSS source): `.todo-body` block contains
  `padding: 4px`.
- All existing outstanding-todos specs continue to pass.
