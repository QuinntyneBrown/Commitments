---
id: bug-016
title: Live / Review mode toggle text is not vertically centered within the segments
status: Fixed
---

# Bug 016 — Mode toggle text not vertically aligned

**Status**: Fixed

## Fix

Forced Material's inner `.mat-button-toggle-button` to `height: 100%` (it was
defaulting to 24px inside the 28px segment, leaving the label-content pinned 2px
above the segment midline) and flex-centered the projected
`.mat-button-toggle-label-content`. The leading dot/icon switched to
`flex: 0 0 auto` with `line-height: 1` to drop Material's 36px default. See
`frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.scss`.

Verified in browser: bounding-box midpoints of `.mat-button-toggle-label-content`
and `.mode-toggle__icon` now match the segment midpoint exactly (delta = 0px).

### Rendered toggle

Live state:

![Mode toggle — Live](assets/mode-toggle-live.png)

Review state:

![Mode toggle — Review](assets/mode-toggle-review.png)

## Description

The dashboard's `cui-mode-toggle` (the pill-shaped Live / Review switch in the primary header,
shown when not in edit mode) renders the segment content sitting visibly off-center vertically
inside each segment:

- The "Live" label and its leading status dot do not sit on the vertical midline of the segment.
- The "Review" label and its leading `history` icon do not sit on the vertical midline either.
- The icon and the text are also off-baseline relative to each other (the icon sits high or
  low, the text drifts the other way).

Each `.mode-toggle__segment` has an explicit `height: 28px` but no flex/grid centering is
applied, so it falls back to Material's default `.mat-button-toggle-label-content` line-box
behaviour. The line-box has its own intrinsic line-height, and the inline `<span class="…__dot">`
(8×8) and `<mat-icon>` (14×14) sit on the text baseline rather than being centered against the
segment's vertical midline. The result is a visibly misaligned pill that does not match the
design.

## Affected files

- `frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.scss` — center
  the segment content; normalise dot/icon vertical alignment.
- `frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.html` — only if
  a wrapper element is needed because Material's projected label-content can't be styled
  directly without `::ng-deep`.

## Reproduction

1. Open the dashboard in live mode (default).
2. Look at the Live / Review toggle in the top-right of the primary header.
3. Observe that the text label and the dot/icon glyph are not vertically centered inside the
   28px segment — they sit slightly above or below the segment's vertical midline.
4. Switch to Review and observe the same misalignment on the Review segment.

## Expected

Both segments render with the dot/icon and the text label visually centered on the vertical
midline of the 28px pill segment, matching the design (`ui-design.pen` → primary header → mode
toggle).

## Suggested fix

In `mode-toggle.component.scss`:

1. Make the segment a flex centering container for its label content. Because Material wraps
   projected content in `.mat-button-toggle-label-content`, target it via `::ng-deep` (the
   file already uses `::ng-deep` for `.mat-pseudo-checkbox`):

   ```scss
   .mode-toggle__segment ::ng-deep .mat-button-toggle-label-content {
     display: inline-flex;
     align-items: center;
     justify-content: center;
     height: 100%;
     line-height: 1;       // override Material's default 36px line-height
     padding: 0 12px;      // tighten if needed; current default is 0 16px
   }
   ```

2. Align the leading glyphs to the flex cross-axis instead of the text baseline:

   ```scss
   .mode-toggle__dot,
   .mode-toggle__icon {
     vertical-align: middle;   // harmless once flex is in place; safe fallback
     flex: 0 0 auto;
   }
   ```

3. Verify the 28px segment height still gives enough optical padding around the 12px uppercase
   text; bump to 30–32px if not.

## Verification

- Visual check in the browser at the dashboard route, both Live and Review states.
- Add a Playwright spec under `frontend/projects/commitments-app/e2e/` (alongside the existing
  `dashboard.spec.ts`) that asserts the bounding-box midline of `.mode-toggle__dot` and
  `.mode-toggle__icon` matches the bounding-box midline of their parent segment within ±1px.
- Snapshot of the toggle should match the design pill.
