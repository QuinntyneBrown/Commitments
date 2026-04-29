# Design 44 — commitments-app core/ dead-code removal

Status: Accepted

## Context

After extracting all feature libraries (designs 34–41) and removing dead pages/dialogs/services
(designs 42–43), the `commitments-app/src/app/core/` directory still contains 15 files whose
functionality was absorbed by feature libraries and are no longer imported by any production code.

## Dead files to delete

| File | Reason |
|------|--------|
| `store.ts` + `store.spec.ts` | Note/Tag signal store — not imported outside its own spec; state lives in notes-feature |
| `auth.service.ts` + `auth.service.spec.ts` | Old HttpClient-based AuthService — replaced by identity-feature's AuthService |
| `hub-client.ts` + `hub-client.spec.ts` | SignalR hub — all consumers (store, auth.service, hub-client-guard) are dead |
| `hub-retry-policy.ts` + `hub-retry-policy.spec.ts` | Only imported by hub-client |
| `message-idempotence-cache.ts` + `message-idempotence-cache.spec.ts` | Only imported by hub-client |
| `auth.guard.ts` | Not applied to any route |
| `can-deactivate.guard.ts` | Not applied to any route |
| `hub-client-guard.ts` | Not applied to any route |
| `i-can-deactivate.ts` | Only referenced by can-deactivate.guard |
| `language.service.ts` | Only referenced by language-guard |
| `language-guard.ts` | Not applied to any route |
| `overlay-ref-provider.ts` | Not imported anywhere |
| `overlay-ref-wrapper.ts` | Not imported anywhere |
| `deep-copy.ts` | Not imported anywhere |
| `error.service.ts` | Not imported anywhere |

## Surviving core/ files

| File | Still alive |
|------|-------------|
| `constants.ts` | Used by interceptors and app.config.ts |
| `headers.interceptor.ts` | Registered in app.config.ts |
| `jwt.interceptor.ts` | Registered in app.config.ts |
| `local-storage.service.ts` | Used by both interceptors |
| `redirect.service.ts` | Used by jwt.interceptor |
| `logger.service.ts` | `LogLevel` enum imported by app.config.ts |

## Acceptance test

Add `core-dead-code-removal.spec.ts` that asserts each deleted file does NOT exist on disk
(filesystem existence checks, no Angular bootstrap needed — same pattern as design 43).

## Implementation steps

1. Write failing spec: `core-dead-code-removal.spec.ts`
2. Commit + push
3. Delete all 15 dead files
4. Verify tests pass
5. Commit + push
