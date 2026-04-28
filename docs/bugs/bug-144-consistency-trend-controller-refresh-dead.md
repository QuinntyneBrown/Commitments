---
id: bug-144
title: ConsistencyTrendController.refresh() is unused after bug-142 — remove
status: Fixed
---

# Bug 144 — ConsistencyTrendController: drop unused `refresh()` method

**Status**: Fixed

## Fix

Deleted the four-line `refresh()` method from
`consistency-trend.controller.ts`. A new spec asserts the
controller does not re-grow the method:

```ts
it('does not expose a refresh() method (bug-144)', () => {
  const ts = readFileSync(...);
  expect(ts).not.toMatch(/^\s*refresh\s*\(\s*\)\s*:/m);
});
```

355/355 workspace tests green.

## Description

After bug-142 consolidated the consistency-trend tile's two
fetch effects into one, nothing in production calls
`controller.refresh()` anymore. The bug-142 doc deliberately
left the method in place "as a small public surface", but a
follow-up grep shows it is now genuinely dead:

```bash
grep -rn 'controller\.refresh\|\.refresh()' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend
```

…returns only the bug-142 *negative* regex assertion (a test
that asserts `controller.refresh()` is **not** called from the
tile). There is no positive caller, no controller spec for it,
and no other consumer in the plugin or in the dashboard host.

YAGNI: a public API with zero callers is dead code. Remove it.
If a future need arises, the same five-line method can be added
back trivially.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.spec.ts` (add regression guard)

## Reproduction

```bash
grep -n 'refresh' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend.controller.ts
```

Returns `refresh(): void { … }` with no caller.

## Expected

The method is deleted from the controller. A new spec asserts
the controller no longer declares a `refresh` method, so this
doesn't accidentally regrow.

## Verification

- New spec asserts `ConsistencyTrendController` does not have
  a `refresh` method.
- All existing consistency-trend specs continue to pass.
- The bug-142 negative-call regex on the *tile* component
  remains valid (refresh is still not called from the tile —
  it doesn't exist anywhere now).
