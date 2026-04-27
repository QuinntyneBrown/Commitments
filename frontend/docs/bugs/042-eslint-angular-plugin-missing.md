---
id: 042
title: npm run lint fails - .eslintrc.js references @angular-eslint plugins that aren't installed
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: c05ce34
flow: dashboard-layout
severity: medium
---

# npm run lint fails — .eslintrc.js references @angular-eslint plugins that aren't installed

## Summary

Running `npm run lint` (or
`npx eslint "projects/.../**/*.ts"`) crashes with:

```
Oops! Something went wrong! :(
ESLint: 8.57.1
ESLint couldn't find the plugin "@angular-eslint/eslint-plugin".
```

`frontend/.eslintrc.js` extends:
- `plugin:@angular-eslint/recommended`
- `plugin:@angular-eslint/template/process-inline-templates`
- `plugin:@angular-eslint/template/recommended` (in the
  `*.html` override)

…and declares two `@angular-eslint/*` rules
(`directive-selector`, `component-selector`).

But `package.json` doesn't list any `@angular-eslint/*`
package in devDependencies, and `node_modules/@angular-eslint`
doesn't exist. So the entire `lint` script can never have
worked from a clean install.

## Reproduction

```
cd frontend
npm install     # already done
npm run lint
```

Crashes immediately with the missing-plugin error.

## Fix outline (radically simple)

Strip the `@angular-eslint/*` references from
`frontend/.eslintrc.js`. The eslintrc keeps:

- `eslint:recommended`
- `plugin:@typescript-eslint/recommended` (already installed)
- the `@typescript-eslint/no-explicit-any` and
  `@typescript-eslint/no-unused-vars` rules (already installed)
- `plugin:storybook/recommended` (already installed) on
  `*.stories.ts`
- the `*.html` override drops to an empty extends array (no
  template lint without the plugin)

This makes `npm run lint` runnable. Adding the Angular plugin
back later is a separate dev-tooling decision; the bug here is
the broken-out-of-the-box state.

## Tests to add (failing first)

The failing artefact is the lint command itself: pre-fix it
crashes with the missing-plugin error, post-fix it runs (and
either succeeds or returns lint findings).
