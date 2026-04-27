---
id: 029
title: review-scrubber-controller.spec.ts imports `vitest` in a Jest project - the suite never runs
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: bd13e05
flow: dashboard-modes
severity: medium
---

# review-scrubber-controller.spec.ts imports `vitest` in a Jest project — the suite never runs

## Summary

The Commitments frontend test runner is **Jest** (configured in
`frontend/jest.config.ts`, run via `npm test`). Every spec file
in the workspace uses Jest globals (`describe`, `it`, `expect`,
`jest.useFakeTimers()`).

`projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber-controller.spec.ts`
is the lone exception — it opens with:

```ts
import { vi } from 'vitest';
```

Jest's `transformIgnorePatterns` and module resolver treat
`vitest` as a node module, but `vitest`'s entry point isn't
written for Jest's CommonJS interop and crashes:

```
projects/.../review-scrubber-controller.spec.ts:1
  ● Test suite failed to run
    .../node_modules/vitest/index.cjs:1:171
```

So the entire `ReviewScrubberController` suite (~10 cases
covering `next`, `prev`, `jumpToToday`, debounce, autoplay)
fails to load, silently. The Jest summary shows
`1 failed, 1 passed, 2 total`.

## Reproduction

1. `cd frontend`
2. `npx jest review-scrubber-controller`

**Expected:** suite runs and the 10+ test cases pass/fail
individually.
**Actual:** suite errors out at import time;
`Test Suites: 1 failed, 1 total / Tests: 0 total`.

## Fix outline (radically simple)

Two-line change to
`projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber-controller.spec.ts`:

  - Drop the `import { vi } from 'vitest';` line.
  - `replace_all` `vi.` → `jest.` (the API surface used here —
    `useFakeTimers`, `advanceTimersByTime`, `useRealTimers` —
    is identical between vitest and jest).

No production-code change.

## Tests to add (failing first)

The suite itself is the failing test. Once the imports work it
should run and pass on its own.
