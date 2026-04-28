---
id: bug-140
title: goal-progress.service mixes inject() with @Optional()/@Inject() constructor decorators
status: Open
---

# Bug 140 — goal-progress.service: drop decorator-based DI for `inject()` form

## Description

Final service in the bug-134..139 series. `HttpClient` already
comes from `inject()`, while the optional base URL still uses
constructor-decorator DI:

```ts
import { Inject, Injectable, InjectionToken, Optional, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);

constructor(@Optional() @Inject(GOAL_PROGRESS_BASE_URL) private readonly _baseUrl: string | null) {}
```

Should become:

```ts
import { Injectable, InjectionToken, inject } from '@angular/core';
...
private readonly _client = inject(HttpClient);
private readonly _baseUrl = inject(GOAL_PROGRESS_BASE_URL, { optional: true });
```

After this fix, every data service in the dashboard plugin uses
the modern `inject()` form for DI — no service mixes the two
styles, no service uses `@Optional`/`@Inject` decorators.

Scope: only the goal-progress service, since this loop iteration
is auditing the goal-metrics tile (which uses GoalProgressService).

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/data/goal-progress.service.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/data/goal-progress.service.spec.ts` (new)

## Reproduction

```bash
grep -n '@Optional()' frontend/projects/commitments-dashboard-plugin/src/lib/data/goal-progress.service.ts
```

Returns one match.

## Expected

- `Inject` and `Optional` are not imported.
- The class has no constructor.
- `_baseUrl` is declared as
  `private readonly _baseUrl = inject(GOAL_PROGRESS_BASE_URL, { optional: true });`.

## Verification

- Source-level spec asserts the imports and shape.
- All existing goal-metrics tile / controller specs continue to pass.
- Sweep grep across the whole `data/` folder for `@Optional()` returns 0 matches.
