---
id: bug-170
title: DeltaTone type aliases are exported but used only file-locally
status: Open
---

# Bug 170 — Drop `export` from `DeltaTone` type aliases

## Description

Two controllers each declare an identical `DeltaTone` type
alias and `export` it:

```ts
// outstanding-todos.controller.ts:9
export type DeltaTone = 'success' | 'warn' | 'neutral';

// goal-metrics.controller.ts:13
export type DeltaTone = 'success' | 'warn' | 'neutral';
```

Cross-repo grep confirms each `DeltaTone` is consumed only by
the corresponding controller's own `deltaTone` computed
signal — no external import. The `export` keyword leaks an
internal helper type into the module surface for no benefit.

Drop `export` on both. The types remain valid for their one
internal usage.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos.controller.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics.controller.ts`

## Reproduction

```bash
grep -rn 'DeltaTone\b' frontend/projects --include='*.ts'
```

Each match is inside the controller's own file (declaration +
the single `computed<DeltaTone>` use).

## Expected

Both type aliases declared with `type DeltaTone = …` (no
`export`). Regression-guard spec asserts neither file has the
exported form.

## Verification

- New regression spec confirms removal of the `export`
  keywords.
- All other tests continue to pass.
