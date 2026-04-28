---
id: bug-068
title: Metric-header dynamic `[class]` binding overrides static `class="..."`, silently dropping `mat-typography`
status: Open
---

# Bug 068 — `[class]` binding silently drops mat-typography

**Status**: Open

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
