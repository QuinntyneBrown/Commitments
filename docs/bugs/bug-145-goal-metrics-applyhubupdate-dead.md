---
id: bug-145
title: GoalMetricsController.applyHubUpdate + GoalProgressUpdated DTO are dead — never wired to a hub
status: Fixed
---

# Bug 145 — Drop `applyHubUpdate` + `GoalProgressUpdated` (no production caller)

**Status**: Fixed

## Fix

Deleted 51 lines across three files:

- `goal-metrics.controller.ts`: removed `applyHubUpdate` method
  (4 lines) and dropped `GoalProgressUpdated` from the
  `goal-progress.service` import.
- `goal-progress.service.ts`: removed the `GoalProgressUpdated`
  interface declaration (5 lines).
- `goal-metrics.controller.spec.ts`: removed the 3 obsolete
  unit tests that exercised the now-deleted method
  (~30 lines), and added one regression-guard spec asserting
  the symbols don't reappear:

```ts
it('does not expose applyHubUpdate — no SignalR hub wired (bug-145)', async () => {
  const ts = readFileSync(... 'goal-metrics.controller.ts', 'utf8');
  expect(ts).not.toMatch(/\bapplyHubUpdate\s*\(/);
  expect(ts).not.toMatch(/\bGoalProgressUpdated\b/);
});
```

353/353 workspace tests green (down from 355 — net effect is
-3 obsolete tests, +1 regression guard).

## Description

`GoalMetricsController.applyHubUpdate(evt: GoalProgressUpdated)`
is a forward-looking hook that was meant to receive SignalR
hub updates. Cross-repo grep confirms it has never been wired:

```
applyHubUpdate / GoalProgressUpdated are referenced in exactly:
  - goal-metrics.controller.ts (definition)
  - goal-metrics.controller.spec.ts (3 unit tests of the method)
  - goal-progress.service.ts (the DTO interface)
```

There is **no** SignalR/HubConnection integration anywhere in
the dashboard-plugin or the host commitments-app. The DTO is
not consumed by any other module either.

This is the same shape as bug-144's dead `refresh()` removal:
zero production callers, the only "validation" is unit tests
exercising behavior that nothing else in the codebase needs.
YAGNI: drop it. If/when a SignalR hub is added later, both the
method (~4 lines) and the DTO (~5 lines) are trivially
re-creatable.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics.controller.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics.controller.spec.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/data/goal-progress.service.ts`

## Reproduction

```bash
grep -rn 'applyHubUpdate\|GoalProgressUpdated' frontend/projects
```

Returns matches only inside the three files above — no
production caller, no hub wiring.

## Expected

- `applyHubUpdate` method removed from `GoalMetricsController`.
- `GoalProgressUpdated` import dropped from the controller.
- `GoalProgressUpdated` interface removed from `goal-progress.service.ts`.
- The 3 controller specs that exercise `applyHubUpdate` are
  removed.
- A new regression-guard spec asserts the controller does not
  re-grow `applyHubUpdate`.

## Verification

- New spec asserts `goal-metrics.controller.ts` has no
  `applyHubUpdate` declaration.
- All other goal-metrics specs continue to pass.
- Workspace test count drops by 3 (the removed specs).
