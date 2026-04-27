---
id: 050
title: ag-grid cell components leave interface-required `params` arg unprefixed - 9 unused-args errors
status: open
discovered: 2026-04-27
flow: dashboard-layout
severity: low
---

# ag-grid cell components leave interface-required `params` arg unprefixed — 9 unused-args errors

## Summary

Four cell-renderer components implement
`ICellRendererAngularComp` from ag-grid:

  - `checkbox-cell.component.ts`
  - `delete-cell.component.ts`
  - `edit-cell.component.ts`
  - `star-cell.component.ts`

The interface requires `refresh(params)`, `agInit(params)`, and
`afterGuiAttached(params)` signatures. Most of these methods
have empty bodies (no logic depends on `params`) but the
parameter still has to be typed. The eslintrc rule
`@typescript-eslint/no-unused-vars` with
`argsIgnorePattern: '^_'` would let them pass if they were
named `_params` — they're named `params`, so each unused arg
trips an error.

Nine errors total across the four files.

## Reproduction

```
cd frontend
npm run lint 2>&1 | grep "'params' is defined but never used"
```

…returns the nine matches.

## Fix outline (radically simple)

Rename each unused `params` arg to `_params`. The interface
shape doesn't depend on parameter names, so the contract is
preserved.

PowerShell sweep across the four files. For each file rewrite
the matching signatures:

  - `refresh(params: any): boolean` → `refresh(_params: any): boolean`
  - `agInit(params: ICellRendererParams): void {}` → `agInit(_params: ICellRendererParams): void {}`
  - `afterGuiAttached?(params?: IAfterGuiAttachedParams): void {}` → `afterGuiAttached?(_params?: IAfterGuiAttachedParams): void {}`

(Only rename when the body is actually empty / never references
`params`; spot-check each file before pushing the regex.)

## Tests to add (failing first)

`npm run lint` itself: pre-fix it reports the nine `params`
errors; post-fix they're gone.
