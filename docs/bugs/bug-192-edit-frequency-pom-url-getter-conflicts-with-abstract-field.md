---
id: bug-192
title: EditFrequencyPagePo get url() getter crashes at construction — abstract readonly url in BasePage compiles to a field initialiser that cannot set a getter-only property
status: Fixed
---

# Bug 192 — `EditFrequencyPagePo` crashes with "Cannot set property url … which has only a getter"

**Status**: Fixed

## Fix

`edit-frequency-page.po.ts` — replace the `get url()` accessor with a
constructor-assigned `readonly` class field, the same pattern all other
working POMs use:

```typescript
- export class EditFrequencyPagePo extends BasePage {
-   constructor(page: Page, private readonly frequencyId?: string) { super(page); }
-   get url() { return this.frequencyId ? `/edit-frequency/${this.frequencyId}` : '/edit-frequency'; }

+ export class EditFrequencyPagePo extends BasePage {
+   readonly url: string;
+   constructor(page: Page, frequencyId?: string) {
+     super(page);
+     this.url = frequencyId ? `/edit-frequency/${frequencyId}` : '/edit-frequency';
+   }
```

3/3 `e2e:frequencies-host` edit-frequency tests now pass. All 6 pass.

## Description

`BasePage` declares `abstract readonly url: string`. Playwright's TypeScript
runner (esbuild) compiles abstract class fields as class field initialisers:
in the base class constructor it effectively runs `this.url = undefined`.

`EditFrequencyPagePo` provided the URL via a prototype getter:

```typescript
get url() { return this.frequencyId ? `/edit-frequency/${this.frequencyId}` : '/edit-frequency'; }
```

A getter-only prototype accessor has no setter. When `super(page)` fires
inside `EditFrequencyPagePo`'s constructor, esbuild's emitted base-class
field init tries `this.url = undefined` on an instance whose prototype
defines `url` as a getter. JavaScript throws:

```
TypeError: Cannot set property url of #<EditFrequencyPagePo> which has only a getter
```

`FrequenciesPagePo` was unaffected because it uses `readonly url = '/frequencies'`
(a class field), which esbuild handles correctly.

## Affected files

- `frontend/projects/commitments-frequencies-feature-host/e2e/support/edit-frequency-page.po.ts`

## Reproduction

```bash
cd frontend && npm run e2e:frequencies-host
```

3 of 6 tests crash immediately on POM construction.

## Expected

All 6 `e2e:frequencies-host` tests pass. The dynamic URL
(`/edit-frequency/<id>` vs `/edit-frequency`) is computed once in the
constructor and stored as a plain field.

## Verification

- `npm run e2e:frequencies-host` — 6/6 pass.
