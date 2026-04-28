---
id: bug-063
title: Tile-shell template carries 3 boolean variant class bindings; collapse to a single dynamic class string
status: Fixed
---

# Bug 063 — Collapse tile-shell variant class bindings

**Status**: Fixed

## Fix

`tile-shell.component.html` replaces:

```html
<mat-card
  class="tile-shell"
  [class.tile-shell--metric]="variant() === 'metric'"
  [class.tile-shell--review]="variant() === 'review'"
  [class.tile-shell--chart]="variant() === 'chart'"
  …>
```

with:

```html
<mat-card
  [class]="'tile-shell tile-shell--' + variant()"
  …>
```

Same DOM result, -3 lines, single place to update for any future
variant rename or addition.

Coverage:
- New spec asserts the new `[class]` form exists and the three
  boolean `[class.tile-shell--…]` bindings are gone.
- All 22 affected suites pass (149/149 — was 148/148 before).

## Description

`tile-shell.component.html` declares three boolean class bindings:

```html
<mat-card
  class="tile-shell"
  [class.tile-shell--metric]="variant() === 'metric'"
  [class.tile-shell--review]="variant() === 'review'"
  [class.tile-shell--chart]="variant() === 'chart'"
  appearance="outlined"
  data-testid="tile-shell">
```

Each renders one class when the variant matches. The `variant`
input is `'metric' | 'review' | 'chart'`, so exactly one of the
three classes lands on the DOM at any time.

A single computed `[class]` binding produces the same DOM with
fewer lines and one place to update if a variant is added or
renamed:

```html
<mat-card
  [class]="'tile-shell tile-shell--' + variant()"
  appearance="outlined"
  data-testid="tile-shell">
```

(`class="tile-shell"` static attribute is dropped because the
`[class]` binding now provides the base class plus the variant
modifier.)

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.html`

## Reproduction

```bash
grep -n 'class.tile-shell--' frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.html
# Three lines.
```

## Expected

A single `[class]` binding emitting `tile-shell` plus the variant
modifier in one expression. The DOM result is identical for any
given `variant()` value.

## Verification

- Unit: the existing tile-shell variant spec
  (`defaults to the metric variant`) continues to pass since it
  reads `component.variant()` directly, independent of the class
  binding.
- New source-level template spec asserts the new `[class]` form
  is present and the three boolean `[class.tile-shell--…]`
  bindings are gone.
- All 22 affected suites continue to pass.
