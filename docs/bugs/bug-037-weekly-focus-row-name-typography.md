---
id: bug-037
title: Weekly Focus — `.focus-list strong` doesn't pin font-size/font-weight; design specifies 14px / 700
status: Open
---

# Bug 037 — Weekly Focus row name typography drifts on browser defaults

**Status**: Open

## Description

`docs/tiles/weekly-focus-tile/ui-design.pen` (frame `wfRow*` text node
`E9Ig4` and friends) renders the focus-row name (e.g. `"Move"`) at:

- `font-family: Inter`
- `font-size: 14`
- `font-weight: 700`
- `fill: #FFFFFF`

The implementation in `weekly-focus-tile.component.scss` styles
`.focus-list strong` with only:

```scss
.focus-list strong {
  color: var(--cui-text-primary);
}
```

`font-size` and `font-weight` are *unset*. The `<strong>` HTML
element renders bold by default in every browser, but the actual
font-weight keyword is `bolder` (relative), and the font-size
inherits from the surrounding context — which depends on whether
the dashboard runs inside Material's `mat-card-content` defaults,
the host's body styles, or some other inheritance chain. The
result is fragile: the row name can render at 16px / `bolder`
depending on context and look heavier than the .pen specifies.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss`

## Reproduction

1. Render the Weekly Focus tile.
2. Inspect a `.focus-list strong` element — `font-size` and
   `font-weight` come from inheritance/browser defaults, not the
   tile's own SCSS.
3. Compare to the .pen — name is explicitly Inter 14 / 700.

## Expected

```scss
.focus-list strong {
  color: var(--cui-text-primary);
  font-size: 14px;
  font-weight: 700;
}
```

## Verification

- Unit: source-level SCSS spec — `.focus-list strong` declares
  `font-size: 14px` and `font-weight: 700`.
- Visual: screenshot the rendered tile vs the .pen.
