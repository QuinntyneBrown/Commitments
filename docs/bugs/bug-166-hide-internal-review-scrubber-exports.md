---
id: bug-166
title: review-scrubber controller and helpers leak into dashboard-framework public API
status: Fixed
---

# Bug 166 — Hide internal review-scrubber implementation from public-api

**Status**: Fixed

## Fix

Two re-export lines deleted from
`dashboard-framework/src/lib/dashboard/index.ts`. The
controller and helper files remain in place — just no longer
published as part of the framework's public API. 366/366
workspace tests green.

## Description

`dashboard-framework/src/lib/dashboard/index.ts` re-exports
two files that are pure internal implementation:

```ts
export * from './review-scrubber/review-scrubber.controller';
export * from './review-scrubber/review-scrubber.helpers';
```

Cross-repo grep:

- `ReviewScrubberController` is referenced only by
  `review-scrubber.component.ts`, the controller itself, and
  its own spec file. No external consumer.
- The helpers (`clampIndex`, `formatFullDate`, `indexOfDate`,
  `isoDateAtIndex`, `tickLabels`) are referenced only by the
  controller and the helpers' own spec.

Both files belong inside the `review-scrubber/` folder; only
`ReviewScrubberComponent` is the actual public surface (and it
stays). Drop the two internal re-exports — keep the files
themselves, just stop leaking them.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/index.ts` (drop two re-export lines)

## Reproduction

```bash
grep -rn 'ReviewScrubberController\|clampIndex\|formatFullDate\|indexOfDate\|isoDateAtIndex\|tickLabels' frontend/projects --include='*.ts'
```

Each match falls inside the `review-scrubber/` folder (the
controller, the helpers, their specs, the component).

## Expected

- `index.ts` no longer re-exports
  `review-scrubber.controller` or `review-scrubber.helpers`.
- Regression-guard spec asserts both lines are absent.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
