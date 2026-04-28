---
id: bug-053
title: Delta-badge has no leading icon; design ctDelta carries an `arrow_upward` 12×12 icon
status: Fixed
---

# Bug 053 — Delta-badge missing directional icon

**Status**: Fixed

## Fix

`DeltaBadgeComponent` adds an `icon()` method mirroring `tone()`:

```ts
icon(): string {
  const t = this.tone();
  if (t === 'positive') return 'arrow_upward';
  if (t === 'negative') return 'arrow_downward';
  return '';
}
```

Template projects the icon as a Material Symbols Rounded span
ahead of the value when `icon()` is non-empty, marked
`aria-hidden="true"` (the formatted value + badge tone already
convey direction to assistive tech).

SCSS adds `.delta-badge__icon` sized 12×12 with `flex-shrink: 0`
so the icon retains its dimensions in tight badges.

The explicit if-chain in `icon()` is preserved over a more compact
lookup-table form to stay readable for junior devs (mirrors
`tone()`'s structure).

Coverage:
- Three branch specs cover positive (`'arrow_upward'`), negative
  (`'arrow_downward'`), and zero (`''`).
- One CSS-source spec asserts `.delta-badge__icon` sizes the icon
  to 12px on font-size, width, and height.
- All 20 affected suites pass (129/129 — was 125/125 before).

## Description

`docs/tiles/consistency-trend/ui-design.pen` (frame `EP7NN` =
`ctDelta`) renders the badge with a leading directional icon:

- node `waY8R`: `iconFontName: arrow_upward`, 12×12, fill
  `#66BB6A` (the same `--cui-success` accent as the text).

The icon visually anchors the directional meaning of the delta —
"+12% vs prior 14d" with an up-arrow signals improvement at a
glance. The implementation in `delta-badge.component.html`
projects only the value and caption text, no icon.

The icon should be:
- `arrow_upward` for `tone === 'positive'`
- `arrow_downward` for `tone === 'negative'`
- nothing for `tone === 'neutral'` (no clear .pen reference;
  omitting keeps the badge tight)

## Affected files

- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.ts`
- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.html`
- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.scss`

## Reproduction

1. Render the Consistency Trend tile.
2. Inspect the delta-badge — only `+12%` and the caption render.
3. Compare to the .pen ctDelta — there's an `arrow_upward` icon
   to the left of the text in the same green accent.

## Expected

`delta-badge.component.html`:

```html
<mat-chip class="delta-badge" …>
  @if (icon()) {
    <span class="material-symbols-rounded delta-badge__icon">{{ icon() }}</span>
  }
  <span class="delta-badge__value">{{ formatted() }}</span>
  …
</mat-chip>
```

`delta-badge.component.ts`:

```ts
icon(): string {
  if (this.tone() === 'positive') return 'arrow_upward';
  if (this.tone() === 'negative') return 'arrow_downward';
  return '';
}
```

`delta-badge.component.scss`:

```scss
.delta-badge__icon {
  font-size: 12px;
  line-height: 12px;
  width: 12px;
  height: 12px;
}
```

## Verification

- Unit:
  - `delta-badge.component.spec.ts` asserts `icon()` returns
    `'arrow_upward'` for positive delta, `'arrow_downward'` for
    negative, and `''` for zero.
  - Spec also asserts the SCSS declares a `.delta-badge__icon`
    rule sized 12px.
- Visual: the badge in the Consistency Trend tile shows a
  green up-arrow before "+12%".
