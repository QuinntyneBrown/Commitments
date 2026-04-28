---
id: bug-128
title: review-scrubber uses legacy @Input + ngOnInit to forward windowDays
status: Open
---

# Bug 128 — review-scrubber input migration

**Status**: Open

## Description

`review-scrubber.component.ts` is the last component in the
codebase still using `@Input`:

```ts
@Input() windowDays?: number;

ngOnInit(): void {
  if (this.windowDays !== undefined) {
    this.controller.windowDays.set(this.windowDays);
  }
}
```

The decorator is the legacy form, and the forwarding only fires
once in `ngOnInit`. If a parent ever rebinds `windowDays`, the
controller's signal stays stale.

Same shape as bug-123 (consistency-trend reactive load) and
bug-125 (icon-button input migration). Migrate to:

```ts
readonly windowDays = input<number | undefined>(undefined);

constructor(readonly controller: ReviewScrubberController) {
  effect(() => {
    const w = this.windowDays();
    if (w !== undefined) {
      this.controller.windowDays.set(w);
    }
  });
}
```

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.ts`
- `frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.controller.spec.ts` (or new spec)

## Reproduction

```bash
grep -n '@Input' frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.ts
```

Returns one line.

## Expected

`@Input` count is 0 across the workspace; `windowDays` declared
via `input()`; the forward to the controller runs inside an
`effect()`.

## Verification

- Unit (TS source): assert `@Input` is gone, `input` imported
  from `@angular/core`, `windowDays = input(` declared, and the
  controller forwarding lives inside an `effect()`.
- Existing review-scrubber controller specs continue to pass.
