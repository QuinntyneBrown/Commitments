# Design 49 — Core Final Cleanup

Status: Complete

## Context

After designs 44–48, the `core/` directory has been reduced to 6 files:
`constants.ts`, `headers.interceptor.ts`, `jwt.interceptor.ts`,
`local-storage.service.ts` (deleted), `logger.service.ts`, `redirect.service.ts` (deleted).

Two files remain that are now dead:

1. **`logger.service.ts`** — provides `LoggerService` and `LogLevel`, but `LoggerService`
   is never injected anywhere in production code.

2. **`constants.ts`** — exports 8 constants; after designs 44–47 removed all their
   consumers:
   - `accessTokenKey` — inlined as a literal string in interceptors / login page
   - `currentProfileIdKey`, `dateFormat`, `storageKey`, `cultureKey`,
     `signalRConnectionIdKey` — zero remaining references
   - `baseUrl` — only used as a DI token in `app.config.ts`, but nothing injects it
   - `minimumLogLevel` — only used by `logger.service.ts` (also dead)

3. **`app.config.ts`** deadwood:
   - `import { baseUrl, minimumLogLevel } from './core/constants'`
   - `import { LogLevel } from './core/logger.service'`
   - `{ provide: baseUrl, useValue: 'http://localhost:52748/' }` — nothing injects `baseUrl`
   - `{ provide: minimumLogLevel, useValue: LogLevel.Trace }` — nothing injects it

## After this design

`core/` contains ONLY:
- `headers.interceptor.ts` + `.spec.ts`
- `jwt.interceptor.ts` + `.spec.ts`

`app.config.ts` loses 3 imports and 2 providers.

## Acceptance tests

Add two assertions to `core-dead-code-removal.spec.ts`:
- `core/logger.service.ts` is deleted
- `core/constants.ts` is deleted

## Implementation steps

1. Add failing assertions to `core-dead-code-removal.spec.ts`
2. Commit + push
3. Delete `logger.service.ts`, `constants.ts`; strip dead lines from `app.config.ts`
4. Verify tests pass → commit + push
