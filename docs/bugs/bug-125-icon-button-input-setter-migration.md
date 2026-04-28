---
id: bug-125
title: icon-button uses legacy @Input({alias}) setters forwarding to internal signals
status: Fixed
---

# Bug 125 — icon-button @Input setter migration

**Status**: Fixed

## Fix

Replaced three @Input setter + internal signal pairs with
direct `input()` calls (icon, ariaLabel, disabled). Net diff:
4 insertions, 16 deletions. `pressed` stays as a writable
signal + setter for now — that 2-way pattern would need
`model()` to fully modernize.

322/322 workspace tests green.

## Description

`icon-button.component.ts` defines four inputs through the
legacy decorator + setter + internal-signal pattern:

```ts
readonly icon = signal('');
@Input({ alias: 'icon' }) set iconInput(value: string) { this.icon.set(value ?? ''); }
readonly ariaLabel = signal('');
@Input({ alias: 'ariaLabel' }) set ariaLabelInput(value: string) { this.ariaLabel.set(value ?? ''); }
readonly disabled = signal(false);
@Input({ alias: 'disabled' }) set disabledInput(value: boolean) { this.disabled.set(Boolean(value)); }
readonly pressed = signal(false);
@Input({ alias: 'pressed' }) set pressedInput(value: boolean) { this.pressed.set(Boolean(value)); }
```

The first three are read-only — the component never mutates
`icon`, `ariaLabel`, or `disabled`. Modern signal-based
`input()` is a strict simplification:

```ts
readonly icon = input('');
readonly ariaLabel = input('');
readonly disabled = input(false);
```

`pressed` is mutated by `toggle()` and emits `pressedChange`,
so it stays as a writable `signal` + setter — that 2-way
binding shape would need `model()`, which is a separate
refactor.

## Affected files

- `frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.ts`
- `frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.spec.ts`

## Reproduction

```bash
grep -c '@Input' frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.ts
```

Returns 4.

## Expected

`@Input` count drops to 1 (the `pressed` setter remains).
`icon`, `ariaLabel`, `disabled` use `input()`. The `Input`
import is kept (still needed for `pressed`); `input` is added
to the imports list.

## Verification

- Unit (TS source): assert exactly 1 `@Input` declaration
  remains, and that `icon = input(`, `ariaLabel = input(`, and
  `disabled = input(` are declared.
- All existing icon-button specs continue to pass.
