---
id: 043
title: 26 lint errors auto-fixable - var->let / prefer-const cleanup across 22 files
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 73d4d73
flow: dashboard-layout
severity: low
---

# 26 lint errors auto-fixable — var->let / prefer-const cleanup across 22 files

## Summary

After bug 042 unblocked `npm run lint`, the suite reports 184
findings (83 errors / 101 warnings). 26 of the errors carry the
"potentially fixable with the `--fix` option" tag — `npm run lint:fix`
resolves them in one pass. The remaining 57 errors and 101
warnings are project-meaningful (unused vars, unused imports,
explicit `any`) and need per-file review.

The fixable bucket is dominated by:
- `var` → `let` in legacy files (`local-storage.service.ts`,
  page components, etc.)
- `prefer-const` for variables never reassigned
  (`add-dashboard-cards-dialog.component.ts`, etc.)

22 files in total. None of the changes alters runtime
behaviour.

## Reproduction

```
cd frontend
npm run lint        # 184 problems (83 errors)
npm run lint:fix    # 158 problems (57 errors) – 26 errors fixed
```

## Fix outline (radically simple)

Run `npm run lint:fix`. Commit the resulting 22 .ts file diffs.
The remaining 57 errors and 101 warnings are tracked separately
as future cleanup; this bug just closes the auto-fixable
delta.

## Tests to add (failing first)

`npm run lint` itself is the failing artefact (returns exit
code 1 with 83 errors). After the autofix pass it returns 57
errors — still non-zero, but the auto-fixable delta is
resolved. The full e2e + jest suites must remain green to
prove no behavioural regression.
