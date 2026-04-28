---
id: bug-075
title: tile-shell icon is co-mingled with the title row, so it does not vertically center against the title+eyebrow group as the designs require
status: Fixed
---

# Bug 075 — tile-shell icon vertical-centering against the heading group

**Status**: Fixed

## Fix

Moved the icon out of the (now removed) `.tile-shell__title-row`
wrapper and made it a direct child of `.tile-shell__header`,
sibling to `.tile-shell__heading`. The header switched from
`align-items: flex-start` to `align-items: center` so all three
direct children (icon, heading column, status-pill) center on the
header's main axis — matching every tile's ui-design.pen.

`.tile-shell__heading` now takes `flex: 1 1 auto` (with
`min-width: 0` to permit text shrink), which keeps the status-pill
flush right and replaces the prior
`justify-content: space-between`.

The internal `.tile-shell__title-row` wrapper was deleted: with the
icon hoisted out, the wrapper would have contained only the title
text. The bug-050 spec that asserted on its `gap` was dropped (the
gap moves to the header, already covered by bug-064).

All 18 tile-shell specs pass; full workspace 260/260.

## Description

Every tile's `ui-design.pen` (daily-results, weekly-focus,
consistency-trend, etc.) has the same header structure:

```
header (horizontal, alignItems: center)
├── icon (20×20, Material Symbols Rounded)
├── titleGroup (vertical, gap 2)
│   ├── title (16px / 500)
│   └── eyebrow (11px)
├── spacer
└── status-pill
```

The icon is a **sibling** of the title-vertical-group, and the
parent flex row uses `alignItems: center`. The icon's vertical
center therefore aligns with the title-group's center — i.e. the
icon sits roughly between the title baseline and the eyebrow
baseline.

`tile-shell.component.html` instead nests the icon **inside** a
`.tile-shell__title-row` together with the `<mat-card-title>`:

```html
<div class="tile-shell__heading">
  <div class="tile-shell__title-row">
    <span class="material-symbols-rounded tile-shell__icon">
      {{ icon() }}
    </span>
    <mat-card-title class="tile-shell__title">
      {{ title() }}
    </mat-card-title>
  </div>
  <p class="tile-shell__eyebrow">{{ eyebrow() }}</p>
</div>
```

Combined with `.tile-shell__header { align-items: flex-start }`,
the icon is centered **only with the title text** and is pinned
to the top of the heading column. Because the heading column is
~37px tall (16px + 2 + 11px) but the title row is ~16px tall, the
icon ends up visually higher than the designs show.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.html`
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`

## Reproduction

1. Open the dashboard at <http://localhost:4200> with any
   tile-shell-based tile (Daily Results, Weekly Focus, etc.).
2. Compare the icon's vertical position against the corresponding
   `docs/tiles/<tile>/ui-design.pen` screenshot.
3. The implementation places the icon level with the title text;
   the design centers it against the entire title+eyebrow group.

## Expected

The icon should be a sibling of `.tile-shell__heading`, not nested
inside `.tile-shell__title-row`. The header row should align its
direct children (icon, heading, status-pill) to the vertical
center.

```html
<mat-card-header class="tile-shell__header">
  @if (icon()) {
    <span class="material-symbols-rounded tile-shell__icon">
      {{ icon() }}
    </span>
  }
  <div class="tile-shell__heading">
    <mat-card-title class="tile-shell__title">
      {{ title() }}
    </mat-card-title>
    @if (eyebrow()) {
      <p class="tile-shell__eyebrow">{{ eyebrow() }}</p>
    }
  </div>
  <ng-content select="[tile-status]"></ng-content>
</mat-card-header>
```

```scss
.tile-shell__header {
  align-items: center;          // was: flex-start
}
```

## Verification

- Unit: a source-level test on `tile-shell.component.html`
  asserting the icon `<span>` is **not** inside a
  `.tile-shell__title-row`, plus an SCSS test asserting
  `.tile-shell__header` uses `align-items: center`.
- All existing tile specs continue to pass.
