---
id: bug-126
title: icon-button.pressed still uses three separate declarations (signal + @Input setter + output) — should be model()
status: Open
---

# Bug 126 — icon-button pressed → model()

**Status**: Open

## Description

bug-125 migrated `icon`, `ariaLabel`, and `disabled` to
`input()`. `pressed` was deliberately left in the legacy
2-way-binding shape:

```ts
readonly pressed = signal(false);
readonly pressedChange = output<boolean>();

@Input({ alias: 'pressed' }) set pressedInput(value: boolean) {
  this.pressed.set(Boolean(value));
}

toggle(): void {
  if (this.disabled()) return;
  const next = !this.pressed();
  this.pressed.set(next);
  this.pressedChange.emit(next);
}
```

Angular 17.2+ exposes `model()` for exactly this shape:

```ts
readonly pressed = model(false);

toggle(): void {
  if (this.disabled()) return;
  this.pressed.set(!this.pressed());
}
```

`model()` returns a `WritableSignal` that:
- accepts input via `[pressed]="…"` binding,
- auto-emits `pressedChange` whenever its value is `set()`,
- removes the need for the separate `output()` declaration and
  the wrapping setter.

Net diff: ~12 lines replaced by 2. Last `@Input` decorator in
the file disappears.

## Affected files

- `frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.ts`
- `frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.spec.ts`

## Reproduction

```bash
grep -n '@Input\|pressedChange\|signal(false)' frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.ts
```

Returns three matches.

## Expected

`pressed = model(false)` declared, `model` imported, the
`@Input` setter and `pressedChange` output declarations gone,
and `toggle()` calls `this.pressed.set(...)` directly without
explicit emit.

## Verification

- Unit (TS source): `@Input` count is 0, `model` is imported,
  `pressed = model(` is declared, `pressedChange` output
  declaration is gone.
- Existing icon-button specs (`pressed`, `toggle`, disabled
  guard) continue to pass — `model()` returns a writable
  signal that supports `.set()` exactly like the previous
  internal `signal(false)`.
