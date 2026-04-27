---
id: 044
title: Two `no-unused-expressions` lint errors flag a real route-guard bug
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: d61c2c8
flow: dashboard-layout
severity: medium
---

# Two `no-unused-expressions` lint errors flag a real route-guard bug

## Summary

`npm run lint` reports two
`@typescript-eslint/no-unused-expressions` errors. Both
indicate code that is grammatically valid TS but doesn't do
what the surrounding code seems to assume:

1. `add-dashboard-cards-dialog.component.ts:37` — `handleCardClick`
   uses a ternary `cond ? action() : action()` as a *statement*.
   Both arms have side-effects so the ternary's result is
   discarded; the file works, but is unidiomatic and trips
   the rule.

2. `edit-note-page.component.ts:78` — `canDeactivate()` body is
   the bare expression `!this.form.dirty;`. The expression is
   evaluated and thrown away — the method returns `undefined`.
   Anything depending on the return value (Angular's
   `CanDeactivate` route guard) sees `undefined` (falsy) and
   blocks navigation — i.e. the dirty-check is broken.

The second is a real bug shipping today; the first is a
readability nit.

## Reproduction

```
cd frontend
npm run lint 2>&1 | grep no-unused-expressions
```

…returns the two findings.

## Fix outline (radically simple)

  1. `add-dashboard-cards-dialog.component.ts` — replace the
     ternary statement with an `if` / `else` block. Same
     control flow, just typed as statements.

  2. `edit-note-page.component.ts` — prepend `return ` to the
     bare expression so the guard returns the boolean it
     intends.

Two surgical edits, one per file.

## Tests to add (failing first)

The lint command is the artefact for both: pre-fix it
reports the two errors; post-fix they're gone. The
edit-note-page change additionally restores the intended
guard behaviour, which would also be exercised by an
e2e test of the note-edit dirty-check flow — but that flow
is not currently routed at the active dashboard, so the
runtime impact stays latent.
