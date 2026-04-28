---
id: bug-138
title: outstanding-todos.service mixes inject() with @Optional()/@Inject() constructor decorators
status: Fixed
---

# Bug 138 — outstanding-todos.service: drop decorator-based DI for `inject()` form

**Status**: Fixed

## Fix

```ts
import { Injectable, InjectionToken, inject } from '@angular/core';
...
export class OutstandingTodosService {
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(OUTSTANDING_TODOS_BASE_URL, { optional: true });
  ...
}
```

`Inject` and `Optional` decorator imports dropped, empty
constructor removed. Both DI sites now use the `inject()`
function. 344/344 workspace tests green.

## Description

Same shape as bugs 134/135/136/137. `HttpClient` already comes
from `inject()`, while the optional base URL still uses
constructor-decorator DI:

```ts
import { Inject, Injectable, InjectionToken, Optional, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);

constructor(@Optional() @Inject(OUTSTANDING_TODOS_BASE_URL) private readonly _baseUrl: string | null) {}
```

Should become:

```ts
import { Injectable, InjectionToken, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);
private readonly _baseUrl = inject(OUTSTANDING_TODOS_BASE_URL, { optional: true });
```

Scope: only the outstanding-todos service, since this loop
iteration is auditing the outstanding-todos tile.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/data/outstanding-todos.service.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/data/outstanding-todos.service.spec.ts` (new)

## Reproduction

```bash
grep -n '@Optional()' frontend/projects/commitments-dashboard-plugin/src/lib/data/outstanding-todos.service.ts
```

Returns one match.

## Expected

- `Inject` and `Optional` are not imported.
- The class has no constructor.
- `_baseUrl` is declared as
  `private readonly _baseUrl = inject(OUTSTANDING_TODOS_BASE_URL, { optional: true });`.

## Verification

- Source-level spec asserts the imports and shape.
- All existing outstanding-todos tile / controller specs continue to pass.
