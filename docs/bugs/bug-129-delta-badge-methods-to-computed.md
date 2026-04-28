---
id: bug-129
title: delta-badge derivations are methods that recompute on every change detection cycle
status: Open
---

# Bug 129 — delta-badge methods → computed signals

**Status**: Open

## Description

`DeltaBadgeComponent` defines three derivations as methods:

```ts
formatted(): string { … }
tone(): DeltaBadgeTone { … }
icon(): string { … }
```

Each is called from the template (`{{ formatted() }}`,
`[class]="'delta-badge--' + tone()"`, `{{ icon() }}`). Because
they are plain methods, Angular re-runs them on every change
detection cycle for the host component, even when the
`delta()` / `format()` inputs haven't changed.

Migrating each to a `computed()` signal makes the call site
identical (`formatted()`) but memoizes the result — only
recomputes when the tracked input signals actually change.

The component is already fully signal-based (bug-125-style
inputs), so this is the natural next step.

## Affected files

- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.ts`
- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.spec.ts`

## Reproduction

```bash
grep -n 'formatted\(\)\|tone\(\)\|icon\(\):' frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.ts
```

Three method declarations.

## Expected

```ts
readonly formatted = computed(() => { … });
readonly tone = computed<DeltaBadgeTone>(() => { … });
readonly icon = computed(() => { … });
```

`computed` is added to the `@angular/core` import.

## Verification

- Unit (TS source): assert `computed` is imported, and that
  `formatted`, `tone`, `icon` are declared as `= computed(`
  (no longer method-form `methodName(): T { … }`).
- Existing delta-badge specs (formatting, tone, icon) keep
  passing — call sites `formatted()`, `tone()`, `icon()` are
  unchanged.
