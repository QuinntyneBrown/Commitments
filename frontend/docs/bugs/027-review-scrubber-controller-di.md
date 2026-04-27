---
id: 027
title: Switching to review mode crashes - ReviewScrubberController constructor incompatible with Angular DI
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 9497810
flow: dashboard-modes
severity: critical
---

# Switching to review mode crashes — ReviewScrubberController constructor incompatible with Angular DI

## Summary

Clicking the **Review** segment of the dashboard mode toggle
silently fails. The browser console reports:

```
ERROR Error: This constructor was not compatible with Dependency Injection.
  at Module.ɵɵinvalidFactory (...)
  at NodeInjectorFactory.ReviewScrubberController_Factory (...)
  at ... ReviewScrubberComponent_Factory ...
```

`projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.controller.ts` declares the controller as:

```ts
@Injectable()
export class ReviewScrubberController {
  constructor(
    private readonly _modeService: DashboardModeService,
    initialWindowDays: number = DEFAULT_WINDOW_DAYS
  ) {
    this.windowDays.set(initialWindowDays);
  }
}
```

The second positional parameter is a primitive `number` with a
default value. Angular's DI cannot resolve a `Number` token, so
the runtime factory throws `ɵɵinvalidFactory`. Result: the
review-mode component fails to mount, the scrubber never
appears, and the toggle silently reverts to live mode.

The controller's unit tests (`review-scrubber-controller.spec.ts`)
construct the controller directly with both args
(`new ReviewScrubberController(modeService, 5)`), so the bug is
invisible to the unit suite.

## Reproduction

1. Navigate to `http://127.0.0.1:4200/`.
2. Open DevTools console.
3. Click the **Review** segment of the dashboard mode toggle.

**Expected:** review mode mounts; the review scrubber renders
under the dashboard.
**Actual:** console error `This constructor was not compatible
with Dependency Injection.`; the toggle stays on live; the
scrubber slot is empty.

## Fix outline (radically simple)

Use Angular's modern functional injection: drop the constructor
service parameter and inject the service via the `inject()`
function inside the field declaration. Keep the existing
`initialWindowDays` parameter on the constructor so the unit
tests don't change.

```ts
@Injectable()
export class ReviewScrubberController {
  private readonly _modeService = inject(DashboardModeService);

  // unchanged signal field:
  // readonly windowDays = signal(DEFAULT_WINDOW_DAYS);

  constructor(initialWindowDays: number = DEFAULT_WINDOW_DAYS) {
    this.windowDays.set(initialWindowDays);
  }
}
```

Wait — Angular DI still cannot resolve a primitive `number`.
The next step: drop the constructor parameter too and let the
caller (component or test) call `controller.windowDays.set(N)`
explicitly. That mirrors what the component already does in
`ReviewScrubberComponent.ngOnInit()`:

```ts
@Input() windowDays?: number;
ngOnInit(): void {
  if (this.windowDays !== undefined) {
    this.controller.windowDays.set(this.windowDays);
  }
}
```

So the controller becomes:

```ts
@Injectable()
export class ReviewScrubberController {
  private readonly _modeService = inject(DashboardModeService);
  // ... no constructor needed ...
}
```

Tests update from `new ReviewScrubberController(modeService, 5)`
to `new ReviewScrubberController(); controller.windowDays.set(5);`
inside an injection context (TestBed.runInInjectionContext) — or,
since the unit tests don't use TestBed, pass the modeService via
a tiny factory:

```ts
static createForTest(modeService: DashboardModeService, initialWindowDays = DEFAULT_WINDOW_DAYS): ReviewScrubberController {
  const c = Object.create(ReviewScrubberController.prototype) as ReviewScrubberController;
  (c as any)._modeService = modeService;
  // re-init signal-bearing fields:
  Object.assign(c, new ReviewScrubberController());
  c.windowDays.set(initialWindowDays);
  return c;
}
```

Too clever. The cleanest minimal patch is the
**InjectionToken** approach — keep both constructor parameters,
but mark the second with an `@Inject(REVIEW_WINDOW_DAYS)` token
that has a default factory. That way Angular DI can resolve it,
and the unit tests' direct `new ReviewScrubberController(modeService, 5)`
call still works because TS treats the second param as a normal
positional `number`.

```ts
import { InjectionToken, Inject, Optional } from '@angular/core';

export const REVIEW_INITIAL_WINDOW_DAYS =
  new InjectionToken<number>('REVIEW_INITIAL_WINDOW_DAYS', {
    factory: () => DEFAULT_WINDOW_DAYS,
  });

@Injectable()
export class ReviewScrubberController {
  constructor(
    private readonly _modeService: DashboardModeService,
    @Optional() @Inject(REVIEW_INITIAL_WINDOW_DAYS) initialWindowDays = DEFAULT_WINDOW_DAYS,
  ) {
    this.windowDays.set(initialWindowDays);
  }
}
```

Adds 1 InjectionToken + 1 import + 2 decorator annotations on
the existing parameter. Unit tests don't change.

## Tests to add (failing first)

E2E (Playwright): a new `dashboard-modes` test asserting that
after clicking the review segment, the page does **not** emit
any `invalidFactory` console error and the scrubber renders.
Currently fails on master.
