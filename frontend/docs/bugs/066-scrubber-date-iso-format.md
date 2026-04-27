# 066 — Review scrubber date displays ISO format instead of the design's friendly format

## Status

FIXED — scrubber now reads `Mon · April 27, 2026`; helper unit tests
12/12, scrubber test suites 20/20.

## Symptom

The scrubber's date readout reads `2026-04-27` (ISO yyyy-mm-dd).
The design shows a friendly readout like `Wed · April 15, 2026`.

## Root cause

`review-scrubber.component.html:44`:

```html
<span class="review-scrubber__date">{{ controller.selectedDate() }}</span>
```

`controller.selectedDate()` is the ISO string from
`mode-service.selectedReviewDate()`. Nothing formats it for display.

## Fix

Add a `selectedDateLabel` computed signal on the controller that formats
the ISO date with `Intl.DateTimeFormat` to look like
`Wed · April 27, 2026` (matching the design's separator). Bind that
in the template instead of the raw ISO string.

The raw ISO `selectedDate()` stays unchanged for the rest of the system
(persistence, slider value lookup, etc.).

## Resolution

- [x] 2 failing unit cases added in `review-scrubber.helpers.spec.ts`.
- [x] `formatFullDate(iso)` helper + `selectedDateLabel` computed signal.
- [x] Template binds `selectedDateLabel()`.
- [x] Visual screenshot post-fix shows `Mon · April 27, 2026`.
