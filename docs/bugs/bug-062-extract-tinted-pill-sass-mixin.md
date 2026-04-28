---
id: bug-062
title: Three near-identical `.status-pill--{chart,success,warning}` rules — extract a `tinted-pill` mixin
status: Open
---

# Bug 062 — Extract `tinted-pill` SASS mixin

**Status**: Open

## Description

After bug-031, bug-033, and bug-035 introduced themed pill variants
for chart, success, and warning, `status-pill.component.scss`
carries three structurally identical rules:

```scss
.status-pill--chart {
  color: var(--cui-info, #42A5F5);
  background: color-mix(in srgb, var(--cui-info, #42A5F5) 14%, transparent);
}

.status-pill--success {
  color: var(--cui-success, #66BB6A);
  background: color-mix(in srgb, var(--cui-success, #66BB6A) 14%, transparent);
}

.status-pill--warning {
  color: var(--cui-warning, #FFA726);
  background: color-mix(in srgb, var(--cui-warning, #FFA726) 14%, transparent);
}
```

Bug-035's audit deferred extracting a mixin until "cross-cutting
demand emerges". With 3 themed variants stable, the demand is now
present — adding a 4th variant via copy-paste would compound the
duplication, while a mixin scales linearly with one new line per
variant.

## Affected files

- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.scss`
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.spec.ts`
  (loosen the existing regex assertions to be tolerant of either
  the literal `var(--cui-…)` form or the new `@include
  tinted-pill(--cui-…, …)` form)

## Reproduction

```bash
grep -A3 'status-pill--' frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.scss
```

Three near-identical rules.

## Expected

```scss
@mixin tinted-pill($token, $fallback) {
  color: var($token, $fallback);
  background: color-mix(in srgb, var($token, $fallback) 14%, transparent);
}

.status-pill--chart   { @include tinted-pill(--cui-info, #42A5F5); }
.status-pill--success { @include tinted-pill(--cui-success, #66BB6A); }
.status-pill--warning { @include tinted-pill(--cui-warning, #FFA726); }
```

Net change: +3 mixin lines, -8 rule-body lines = 5 fewer lines
total, plus a single mixin call per future themed variant.

## Verification

- Unit: existing bug-031/033/035 specs are loosened to match
  `--cui-info` / `--cui-success` / `--cui-warning` (without the
  `var(` prefix), which works for both the old literal form and
  the new `@include` arg form.
- New spec asserts the mixin exists in the SCSS source.
- All 22 affected suites continue to pass.
