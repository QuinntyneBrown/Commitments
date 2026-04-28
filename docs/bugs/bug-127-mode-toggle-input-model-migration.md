---
id: bug-127
title: mode-toggle uses legacy @Input setters; should use input()/model() like bug-125/126 did for icon-button
status: Fixed
---

# Bug 127 — mode-toggle input/model migration

**Status**: Fixed

## Fix

Applied the bug-125/126 shape to mode-toggle:

- `disabled` → `input(false)` (read-only)
- `mode` → `model<DashboardMode>('live')` (2-way, auto-emits modeChange)
- `select()` simplifies to `this.mode.set(next)` (no manual emit)

Net diff: 3 inserts, 13 deletes. 324/324 workspace tests green.

## Description

`mode-toggle.component.ts` mirrors the pattern bug-125 + bug-126
fixed on icon-button:

```ts
readonly mode = signal<DashboardMode>('live');
readonly disabled = signal(false);
readonly modeChange = output<DashboardMode>();

@Input({ alias: 'mode' }) set modeInput(value: DashboardMode) {
  this.mode.set(value ?? 'live');
}

@Input({ alias: 'disabled' }) set disabledInput(value: boolean) {
  this.disabled.set(Boolean(value));
}

select(next: DashboardMode): void {
  if (this.disabled() || next === this.mode()) return;
  this.mode.set(next);
  this.modeChange.emit(next);
}
```

- `disabled` is read-only — straight `input(false)`.
- `mode` is the 2-way binding (component sets it via `select()`,
  emits `modeChange`) — collapses to `model<DashboardMode>('live')`.

## Affected files

- `frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.ts`
- `frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.spec.ts`

## Reproduction

```bash
grep -c '@Input' frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.ts
```

Returns 2.

## Expected

- `disabled = input(false)`
- `mode = model<DashboardMode>('live')`
- `select()` simplifies to `this.mode.set(next)` (no explicit emit)
- `Input` import dropped, `input` and `model` added
- `output` import dropped, explicit `modeChange = output<...>()` gone

## Verification

- Unit (TS source): assert `@Input` count is 0, `model` is
  imported, `mode = model(` and `disabled = input(` declared,
  `modeChange = output(` is gone.
- Existing mode-toggle specs (select/disabled/keyboard nav) keep
  passing — `model().set()` behaves like the previous internal
  `signal.set()`.
