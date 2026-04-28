---
id: bug-073
title: Only consistency-trend's spec has a line-length cap; six other tile specs lack the same regression guard
status: Open
---

# Bug 073 — Line-length regression guard missing from six tile specs

**Status**: Open

## Description

Bug-070 added a 110-char line-length cap to
`consistency-trend-tile.component.spec.ts`. The other six dashboard-
plugin tile templates (daily-results, weekly-focus,
monthly-progress, outstanding-todos, relations, goal-metrics) all
currently comply (no template line exceeds 110 chars), but their
spec files lack the same regression guard.

Adding a parallel line-length assertion to each tile's component
spec ensures any future contributor's long single-line attribute
edit fails CI early.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.spec.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.spec.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.spec.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.spec.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.spec.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/goal-metrics-tile/goal-metrics-tile.component.spec.ts`

## Reproduction

```bash
grep -lE 'length > 110' frontend/projects/commitments-dashboard-plugin/src/lib/tiles/**/*.spec.ts
```

Only the consistency-trend spec matches.

## Expected

Each of the six listed specs gains an assertion identical in
spirit to bug-070's:

```ts
it('keeps every template line under 110 characters (bug-073)', () => {
  const lines = html.split(/\r?\n/);
  const overLong = lines
    .map((line, i) => ({ n: i + 1, len: line.length }))
    .filter(({ len }) => len > 110);
  expect(overLong).toEqual([]);
});
```

(The `html` constant is already declared at the top of each
spec — no new file reads needed.)

## Verification

- Each of the six specs adds the line-length assertion. They
  all pass on the current templates (no actual reformatting
  needed).
- All affected suites continue to pass.

## Note

This entry is regression-coverage extension rather than a strict
design-vs-render fix — current templates comply. The "tests fail
before fix" pattern doesn't apply here; the value is preventing
future drift. Filed under the loop's audit-step (a) "make the
implementation more testable" rather than the bug-fix protocol's
"write failing tests" step.
