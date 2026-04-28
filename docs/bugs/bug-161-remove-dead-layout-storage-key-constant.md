---
id: bug-161
title: LAYOUT_STORAGE_KEY (legacy single-mode key) is exported but never imported — drop
status: Open
---

# Bug 161 — Drop unused `LAYOUT_STORAGE_KEY` constant

## Description

`dashboard.model.ts` exports three localStorage key constants:

```ts
export const LAYOUT_STORAGE_KEY = 'commitments.dashboard.layout.v1';
export const LIVE_LAYOUT_STORAGE_KEY = 'commitments.layout.live';
export const REVIEW_LAYOUT_STORAGE_KEY = 'commitments.layout.review';
```

The two mode-specific keys are imported by
`layout-persistence.service.ts`. The legacy
`LAYOUT_STORAGE_KEY` is **not imported anywhere** — it is the
old single-key value from before live/review separation. It
remains exported but unused.

```bash
grep -rn 'LAYOUT_STORAGE_KEY\b' frontend/projects --include='*.ts'
```

…shows the e2e POM uses a *local* constant with the value
`'commitments.layout.live'` (the new live key), unrelated to
the dead export.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard.model.ts` (drop one constant)

## Expected

- The line `export const LAYOUT_STORAGE_KEY = …` is removed.
- A regression-guard spec asserts the symbol no longer
  appears in `dashboard.model.ts`.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
