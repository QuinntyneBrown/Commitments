---
id: bug-122
title: Consistency Trend eyebrow text says "last 14 days" but windowDays defaults to 30
status: Open
---

# Bug 122 — eyebrow ↔ windowDays mismatch

**Status**: Open

## Description

`consistency-trend-tile.component.html:3` hardcodes the
eyebrow:

```html
eyebrow="7-day rolling average · last 14 days"
```

The component's `windowDays` input defaults to `30`:

```ts
readonly windowDays = input(30);
```

So the chart shows a 30-day window, but the user-facing label
under the title claims it's the last 14 days. Either the
default windowDays or the eyebrow drifts when these aren't
linked.

The `ui-design.pen` was authored with `windowDays` representing
14 days (`OPN6L` text content + 13-point `ltPlot`), but the
README and component default settled on 30. Tying the eyebrow
to the input fixes both today's mismatch and any future
override.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.html`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/consistency-trend/consistency-trend-tile/consistency-trend-tile.component.spec.ts`

## Reproduction

1. Open the dashboard, add the Consistency Trend tile.
2. The eyebrow says "last 14 days".
3. Use Chrome DevTools to count the chart's x-axis labels — 5
   covering 30 days at ~6-day intervals (matching
   `windowDays = 30`).

## Expected

The eyebrow is bound to `windowDays()`:

```html
[eyebrow]="'7-day rolling average · last ' + windowDays() + ' days'"
```

So the user-visible label always reflects the actual window
size.

## Verification

- Unit (template source): assert the eyebrow uses a `[eyebrow]`
  binding referencing `windowDays()`, not the literal "14 days"
  string.
- All existing consistency-trend specs continue to pass.
