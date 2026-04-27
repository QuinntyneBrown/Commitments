# Bug 011 — Review scrubber "Jump to today" button missing text label

**Status**: Fixed ✓

## Description

The "Jump to today" button in the review scrubber is an icon-only round button.
The design (`ui-design.pen` → `Scrubber/Timeline` → `scrJump`) specifies a pill-shaped
button with both the icon and the visible text label "Jump to today". Without the label,
new users cannot discover the button's purpose without hovering for the aria tooltip.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.html`
- `frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.scss`

## Fix

Replace the icon-only `cui-icon-button` for the today action with a labeled pill button
that shows `today` icon + "Today" text. Style it with `--cui-surface-3` background,
full border radius, `11px` font, and `0 12px` padding — matching the design spec.
