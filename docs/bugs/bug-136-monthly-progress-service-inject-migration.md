---
id: bug-136
title: monthly-progress.service mixes inject() with @Optional()/@Inject() constructor decorators
status: Fixed
---

# Bug 136 — monthly-progress.service: drop decorator-based DI for `inject()` form

**Status**: Fixed

## Fix

```ts
import { Injectable, InjectionToken, inject } from '@angular/core';
...
export class MonthlyProgressService {
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(MONTHLY_PROGRESS_BASE_URL, { optional: true });
  ...
}
```

`Inject` and `Optional` decorator imports dropped, empty
constructor removed. Both DI sites now use the `inject()`
function. 338/338 workspace tests green.

## Description

Same shape as bug-134 (relations) and bug-135 (daily-results),
but for `monthly-progress.service.ts`. `HttpClient` already
comes from `inject()`, while the optional base URL still uses
constructor-decorator DI:

```ts
import { Inject, Injectable, InjectionToken, Optional, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);

constructor(@Optional() @Inject(MONTHLY_PROGRESS_BASE_URL) private readonly _baseUrl: string | null) {}
```

Should become:

```ts
import { Injectable, InjectionToken, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);
private readonly _baseUrl = inject(MONTHLY_PROGRESS_BASE_URL, { optional: true });
```

Scope: only the monthly-progress service, since this loop
iteration is auditing the monthly-progress tile.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/data/monthly-progress.service.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/data/monthly-progress.service.spec.ts` (new)

## Reproduction

```bash
grep -n '@Optional()' frontend/projects/commitments-dashboard-plugin/src/lib/data/monthly-progress.service.ts
```

Returns one match.

## Expected

- `Inject` and `Optional` are not imported.
- The class has no constructor.
- `_baseUrl` is declared as
  `private readonly _baseUrl = inject(MONTHLY_PROGRESS_BASE_URL, { optional: true });`.

## Verification

- Source-level spec asserts the imports and shape.
- All existing monthly-progress tile / controller specs continue to pass.
