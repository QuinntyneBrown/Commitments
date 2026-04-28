---
id: bug-131
title: OutstandingTodosController._reviewDelta() is a method called from two computed signals; should itself be computed
status: Open
---

# Bug 131 — outstanding-todos _reviewDelta computed

**Status**: Open

## Description

Same shape as bug-130 (goal-metrics).
`outstanding-todos.controller.ts` declares
`_reviewDelta()` as a private method called from both
`deltaLabel` and `deltaTone` computed signals — re-running its
body on each evaluation. Migrating to a `computed` signal
memoizes the result.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos.controller.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos.controller.spec.ts`

## Reproduction

```bash
grep -n '_reviewDelta' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos.controller.ts
```

One method declaration; two callers from inside `computed(() => …)`.

## Expected

`_reviewDelta` declared as `computed<number | null>(...)` at
the class top, same call sites unchanged.

## Verification

- Unit (TS source): assert `_reviewDelta` is declared as
  `= computed(` (no method-form).
- Existing outstanding-todos specs continue to pass.
