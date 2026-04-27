---
id: 003
title: TranslateService is not provided at the app config level
status: fixed
discovered: 2026-04-26
fixed: 2026-04-26
fixed_in: 5f391fc
flow: authentication
severity: critical
---

# TranslateService is not provided at the app config level

## Summary

`LoginPageComponent` (and many other page components) imports
`TranslateModule` from `@ngx-translate/core` for the `| translate`
pipe. But `app.config.ts` never calls `provideTranslateService(...)` /
`TranslateModule.forRoot(...)`, so when the route renders, Angular
throws `NG0201: No provider found for TranslateStore` and the page
cannot mount.

## Reproduction

After fixing bug #001 (routing) and bug #002 (broken import), navigate
to `/login` in the browser.

**Expected:** the login form mounts.
**Actual:** runtime error in console:

```
ERROR ɵNotFound: NG0201: No provider found for `TranslateStore`.
Source: Standalone[_LoginPageComponent].
Path: _ErrorService -> _TranslateService -> TranslateStore.
```

The router-outlet remains empty.

## Evidence

`grep` finds no `provideTranslateService` or
`TranslateModule.forRoot` anywhere in the workspace, but 18+ page
components import `TranslateModule`.

## Fix outline (radically simple)

Drop the `| translate` pipe from the login template (the pipe was
applied to four English string literals — `Login`, `Username`,
`Password`, `Submit` — none of which have actual translation data
shipped). Stop importing `TranslateModule` from
`LoginPageComponent`. This makes the login mountable without
introducing a new provider or a translation-loader configuration.

Other pages will need the same trim or a real
`provideTranslateService` setup — those are out of scope for the
authentication flow being exercised here and will be tracked as
their own bugs as those flows are exercised.
