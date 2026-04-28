---
id: bug-068
title: Metric-header dynamic `[class]` binding overrides static `class="..."`, silently dropping `mat-typography`
status: Fixed
---

# Bug 068 — `[class]` binding silently drops mat-typography

**Status**: Fixed

## Fix

`metric-header.component.html` drops the static
`class="metric-header mat-typography"` attribute and folds all
classes into the single dynamic binding:

```html
<div [class]="'metric-header mat-typography metric-header--' + accent()">
```

Angular's `[class]="exp"` replaces the entire `class` attribute
on every change-detection cycle. The previous split form (static
class + dynamic binding) had Angular apply the static class on
attribute init then overwrite it on first CD, silently dropping
`mat-typography`. Folding all classes into the binding makes it
the single source of truth, so `mat-typography` persists across
CD cycles.

Same pattern as bug-063's tile-shell cleanup.

Coverage:
- Two new template-source specs assert `mat-typography` is in
  the dynamic binding string AND the split form (`class="…"
  [class]="…"`) is rejected.
- All 22 affected suites pass (157/157 — was 155/155 before).

## Description

`metric-header.component.html` line 1:

```html
<div class="metric-header mat-typography" [class]="'metric-header metric-header--' + accent()">
```

Angular's `[class]="exp"` binding **replaces** the entire `class`
attribute on each change-detection cycle. The static
`class="metric-header mat-typography"` is briefly applied at mount
but overwritten by the dynamic binding on the first CD pass —
which produces `'metric-header metric-header--<accent>'` without
`mat-typography`.

`mat-typography` enables Material's typography styles for content
inside the host. Losing it means the text falls back to default
browser typography or whatever the cascade provides. With the
app's global Inter cascade (styles.scss line 11), the visual
impact is minor in the typical-load case, but the design system's
typography contract is broken at the primitive level.

The fix bundles all classes into the dynamic binding so it is the
single source of truth (matching the pattern bug-063 applied to
tile-shell):

```html
<div [class]="'metric-header mat-typography metric-header--' + accent()">
```

## Affected files

- `frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.html`

## Reproduction

1. Render the consistency-trend tile (the metric-header's only
   live consumer).
2. Inspect the metric-header `<div>` — its `class` attribute
   reads `metric-header metric-header--chart` (or `--review`),
   without `mat-typography`.
3. Compare to the template — `mat-typography` is declared
   statically but not present in the rendered DOM.

## Expected

Single dynamic binding emits all classes:

```html
<div [class]="'metric-header mat-typography metric-header--' + accent()">
```

## Verification

- Unit: source-level template spec — the `[class]` binding string
  contains `'mat-typography'`; static `class="..."` declaring
  `metric-header mat-typography` is gone (single source).
- Visual: Material typography now applies to metric-header
  content (subtle in most environments).
- All 22 affected suites continue to pass.
