# Review Mode and History Scrubbing

Review mode is the analysis view. It lets you select a historical date and inspect dashboard metrics as of that date.

## Enter Review Mode

1. Open Dashboard.
2. Use the mode toggle in the right side of the primary header and choose Review.
3. Confirm the review scrubber appears above the dashboard grid.
4. Confirm review-capable tiles show review status or historical values.

Review mode persists the selected review date in browser storage. If you leave and return, the dashboard can restore the last selected review date.

## What Changes in Review Mode

When Review mode is active:

- The review scrubber appears.
- The "+" FAB is hidden — review is for historical inspection, not for changing tile composition.
- Edit mode is unreachable from review; switch back to Live first.
- Tiles receive the selected review date through dashboard tile context.
- Review tiles show historical values, date badges, and deltas where available.
- Live push messages are not applied directly to historical tile values.

Use Review mode to answer questions such as:

- What did this metric look like last Tuesday?
- Did a frequency change alter the expected daily result?
- Was a drop caused by missing activities or by a changed commitment?
- How did today's value compare with the selected historical day?

## Scrubber Controls

The review scrubber includes:

| Control | Action |
|---|---|
| Previous day | Move the selected review date back one day |
| Play / Pause | Auto-scrub forward through the date window |
| Next day | Move the selected review date forward one day |
| Slider | Drag directly to a date in the review window |
| Date label | Shows the selected date |
| Jump to today | Move the selected date to today |

The default review window is 90 days ending today.

## Keyboard Controls

When focus is on the scrubber:

| Key | Action |
|---|---|
| ArrowLeft | Previous day |
| ArrowRight | Next day |
| PageUp | Move back seven days |
| PageDown | Move forward seven days |
| Home | Jump to start of the review window |
| End | Jump to today |

## Scrub Historical Metrics

1. Switch to Review mode.
2. Drag the slider to a date.
3. Pause briefly so the debounced historical query can resolve.
4. Watch review-capable tiles update.
5. Use Previous day and Next day for precise movement.
6. Use Play to animate through a date range.

The selected review date is passed to tiles as `selectedReviewDate`. Review-aware tiles use it to fetch or compute historical snapshots.

## Read the Review Goal History Tile

The Review Goal History tile shows:

- `REVIEW` status pill.
- Selected date badge.
- Historical count and target.
- Caption such as "Snapshot at end of day" or "No achievements that day".
- Delta versus today.

Use this tile when you need a point-in-time answer for one commitment.

## Read the Consistency Trend Tile in Review

In Review mode, Consistency Trend uses the selected review date as its `asOf` date. The chart answers: "What did the trend look like as of this historical point?"

Use it to compare:

- Current percentage at the selected date.
- Peak and low percentage in that historical window.
- Whether the selected day was part of a rising or falling trend.

## Review Mode Audit Workflow

To investigate a metric change:

1. Switch to Review mode.
2. Jump to today.
3. Step backward one day at a time until the metric changes.
4. Note the first date where the metric differs.
5. Open Activities for that date and inspect recorded behaviours.
6. Check Commitments and Frequencies if expected counts changed.
7. Return to Review mode and scrub around the change date.

## Live vs Review Interpretation

Live mode answers: "What is true right now?"

Review mode answers: "What was true as of this selected date?"

Do not use Live mode for historical audits. Do not use Review mode to validate whether the SignalR stream is working. Use the right mode for the question.

