---
id: bug-134
title: relations-summary.service mixes inject() with @Optional()/@Inject() constructor decorators
status: Fixed
---

# Bug 134 — relations-summary.service: drop decorator-based DI for `inject()` form

**Status**: Fixed

## Fix

```ts
import { Injectable, InjectionToken, inject } from '@angular/core';
...
export class RelationsSummaryService {
  private readonly _client = inject(HttpClient);
  private readonly _baseUrl = inject(RELATIONS_BASE_URL, { optional: true });
  ...
}
```

`Inject` and `Optional` decorator imports dropped, empty
constructor removed. Both DI sites now use the `inject()`
function. 332/332 workspace tests green.

## Description

`relations-summary.service.ts` already uses the `inject()`
function for `HttpClient`, but obtains the optional base URL
via the older constructor-decorator form:

```ts
import { Inject, Injectable, InjectionToken, Optional, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);

constructor(@Optional() @Inject(RELATIONS_BASE_URL) private readonly _baseUrl: string | null) {}
```

That is two DI styles for the same service. The modern, single
form is:

```ts
import { Injectable, InjectionToken, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);
private readonly _baseUrl = inject(RELATIONS_BASE_URL, { optional: true });
```

That drops the `Inject` and `Optional` imports, removes the
empty constructor, and matches every other DI site in the
plugin (controllers, components, framework helpers).

Scope: only the relations-summary service, since this loop
iteration is auditing the relations tile. The other six data
services use the same legacy pattern and should be migrated in
their own iterations.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/data/relations-summary.service.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/data/relations-summary.service.spec.ts` (new)

## Reproduction

```bash
grep -n '@Optional()' frontend/projects/commitments-dashboard-plugin/src/lib/data/relations-summary.service.ts
```

Returns one match.

## Expected

- `Inject` and `Optional` are not imported.
- The class has no constructor.
- `_baseUrl` is declared as
  `private readonly _baseUrl = inject(RELATIONS_BASE_URL, { optional: true });`.

## Verification

- Source-level spec asserts the imports and shape.
- All existing relations-tile / controller specs continue to pass.
