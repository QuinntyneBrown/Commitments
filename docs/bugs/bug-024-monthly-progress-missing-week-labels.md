---
id: bug-024
title: Monthly Progress — bar chart has no W1/W2/W3/W4 labels beneath the bars
status: Open
---

# Bug 024 — Monthly Progress missing week labels

**Status**: Open

## Description

`docs/tiles/monthly-progress-tile/ui-design.pen` (frame `mpLabels`)
shows a horizontal label row beneath the bar chart with `W1` `W2` `W3`
`W4`, justified `space-between`, in Inter 11/400/#666666. Each label
sits centered beneath its corresponding bar.

The implementation (`monthly-progress-tile.component.html`) renders
only the bars (`<div class="bars">`) — there is no labels row at all.
The user can identify the *most recent* week only via aria-label on
each bar, which is not visible. Visually the chart shows four bars of
varying heights but no axis legend.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`

## Reproduction

1. Render the Monthly Progress tile.
2. Observe four bars but no labels beneath.
3. Compare to the .pen — `W1`–`W4` labels appear under the bars.

## Expected

A row of week labels beneath the bars, one per bucket, generated from
the loop index (`W{{ i + 1 }}` over `controller.buckets()`), styled
Inter 11/400/#666666 and laid out `space-between` to align with the
bar columns. The labels are hidden when `controller.isEmpty()` is
true (no bars to label).

## Verification

- Unit: source-level HTML check — template must contain a
  `class="bar-labels"` container that loops over `controller.buckets()`
  and emits a label per bucket.
- Visual: screenshot the rendered tile vs the .pen.
