---
id: 049
title: Five page components share `const _ = i<0 ? push : replace` upsert pattern that trips no-unused-vars
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 97db099
flow: dashboard-layout
severity: low
---

# Five page components share `const _ = i<0 ? push : replace` upsert pattern that trips no-unused-vars

## Summary

`activities`, `behaviours`, `behaviour-types`, `card-layouts`,
and `frequencies` page components each contain the same
upsert micro-pattern:

```ts
const _ = i < 0
  ? collection.push(item)
  : collection[i] = item;
```

The `_` is a throwaway placeholder that traps the result of a
ternary used purely for side-effects. The lint rule
`@typescript-eslint/no-unused-vars` flags it because regular
variables don't get the `^_` parameter exemption.

Five identical findings, one fix shape: drop `const _ =` and
convert the ternary to an `if / else` statement.

## Reproduction

```
cd frontend
npm run lint 2>&1 | grep "_' is assigned a value but never used"
```

…returns the five `_` errors at:
- `activities-page.component.ts:83`
- `behaviour-types-page.component.ts:81`
- `behaviours-page.component.ts:90`
- `card-layouts-page.component.ts:75`
- `frequencies-page.component.ts:81`

## Fix outline (radically simple)

PowerShell sweep across the five files: replace the
single-line `const _ = i < 0 ? collection.push(item) : collection[i] = item;`
with the multi-line equivalent:

```ts
if (i < 0) {
  collection.push(item);
} else {
  collection[i] = item;
}
```

Same control flow, statement-typed.

The `$event` unused-arg errors flagged near each `_` are a
separate bug (rename the parameter to `_event` to comply with
the existing `^_` argsIgnorePattern) — left for a future
iteration.

## Tests to add (failing first)

`npm run lint` itself: pre-fix it reports the five
`_' is assigned a value but never used` errors; post-fix they
disappear.
