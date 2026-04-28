---
id: bug-135
title: daily-results.service mixes inject() with @Optional()/@Inject() constructor decorators
status: Open
---

# Bug 135 — daily-results.service: drop decorator-based DI for `inject()` form

## Description

Same shape as bug-134, but for `daily-results.service.ts`.
`HttpClient` already comes from `inject()`, while the optional
base URL still uses constructor-decorator DI:

```ts
import { Inject, Injectable, InjectionToken, Optional, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);

constructor(@Optional() @Inject(DAILY_RESULTS_BASE_URL) private readonly _baseUrl: string | null) {}
```

Should become:

```ts
import { Injectable, InjectionToken, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);
private readonly _baseUrl = inject(DAILY_RESULTS_BASE_URL, { optional: true });
```

Scope: only the daily-results service, since this loop
iteration is auditing the daily-results tile. Bugs 136-140 will
follow for the remaining 5 services as their tiles are audited.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/data/daily-results.service.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/data/daily-results.service.spec.ts` (new)

## Reproduction

```bash
grep -n '@Optional()' frontend/projects/commitments-dashboard-plugin/src/lib/data/daily-results.service.ts
```

Returns one match.

## Expected

- `Inject` and `Optional` are not imported.
- The class has no constructor.
- `_baseUrl` is declared as
  `private readonly _baseUrl = inject(DAILY_RESULTS_BASE_URL, { optional: true });`.

## Verification

- Source-level spec asserts the imports and shape.
- All existing daily-results tile / controller specs continue to pass.
