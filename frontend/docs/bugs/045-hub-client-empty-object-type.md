---
id: 045
title: hub-client.ts uses deprecated `{}` type for empty hubResumed payload
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 93b0b73
flow: dashboard-layout
severity: low
---

# hub-client.ts uses deprecated `{}` type for empty hubResumed payload

## Summary

`hub-client.ts:66` builds a `RealtimeMessage` for the
`hubResumed` event and types its empty `payload: {}` literal
as `RealtimeMessage<{}>`. The TypeScript-ESLint
`@typescript-eslint/no-empty-object-type` rule flags this
because `{}` accepts any non-nullish value (numbers, strings,
…) — not what "empty object" implies.

The intent is "a payload with no fields". The idiomatic shape
for that in TS is `Record<string, never>`.

## Reproduction

```
cd frontend
npm run lint 2>&1 | grep no-empty-object-type
```

…returns the single finding at `hub-client.ts:66:30`.

## Fix outline (radically simple)

One-character-grade swap: `RealtimeMessage<{}>` →
`RealtimeMessage<Record<string, never>>`.

## Tests to add (failing first)

Lint is the failing artefact: pre-fix it reports the rule;
post-fix it doesn't. The hub-client unit suite
(`hub-client.spec.ts`) already covers the runtime behaviour
and stays green.
