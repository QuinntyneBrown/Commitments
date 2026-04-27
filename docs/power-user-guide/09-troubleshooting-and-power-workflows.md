# Troubleshooting and Power Workflows

Use this chapter when something looks wrong or when you need repeatable expert workflows.

## Create a New Commitment From Scratch

1. Open Behaviour Types and create the category.
2. Open Behaviours and create the behaviour.
3. Open Frequency Types and confirm the needed type exists.
4. Open Frequencies and create the frequency rule.
5. Open Commitments and create the commitment.
6. Open Activities and record the first activity if it already happened.
7. Open Dashboard Live mode and add or inspect the relevant metric tile.
8. Switch to Review mode and confirm the historical baseline.

## View Live Metrics Streamed to the Dashboard

1. Sign in and confirm the active profile.
2. Open Dashboard.
3. Switch to Live mode.
4. Add Live Goal Metrics or Consistency Trend.
5. In another tab or workflow, record an Activity for the tracked behaviour.
6. Return to the dashboard.
7. Confirm the live tile updates without refreshing.

If the tile does not change, use the live-stream troubleshooting table below.

## Switch to Review Mode and Scrub Historical Metrics

1. Open Dashboard.
2. Switch the mode toggle from Live to Review.
3. Confirm the scrubber appears.
4. Drag the slider to a historical date.
5. Step with Previous day and Next day for precision.
6. Press Play to animate forward through the window.
7. Watch Review Goal History and Consistency Trend update as the selected date changes.
8. Use Jump to today when finished.

Use this workflow after editing activity history, changing frequencies, or investigating a metric drop.

## Diagnose a Dashboard Drop

1. Open Dashboard Review mode.
2. Jump to today.
3. Step backward until the metric returns to its previous level.
4. Note the first changed date.
5. Open Activities and inspect activity records around that date.
6. Open Commitments and Frequencies if expected counts changed.
7. Capture the finding in a tagged note.
8. Add a to-do if follow-up is needed.

## Weekly Review Workflow

1. Open Dashboard Review mode.
2. Scrub the last seven days.
3. Check Daily Results, Consistency Trend, and Outstanding To Dos.
4. Open Notes and write a weekly review.
5. Tag the note `weekly-review`.
6. Open Commitments and adjust expectations only after reviewing the evidence.
7. Open To Do's and create next actions.
8. Return to Dashboard Live mode for the new week.

## Troubleshooting

| Symptom | Likely cause | What to do |
|---|---|---|
| Cannot sign in | Bad credentials, backend offline, stale token | Confirm backend is running, retry credentials, clear session by visiting Login |
| Page data is empty | Wrong active profile or no data created | Check profile, then create catalog data in setup order |
| Behaviour dropdown is empty | No behaviours exist yet | Create Behaviour Types and Behaviours first |
| Frequency dropdown is empty | No frequencies exist yet | Create Frequency Types and Frequencies first |
| Commitment save fails | Required behaviour or frequency missing | Fill required fields and retry |
| Delete fails | Another record depends on it | Remove or edit dependent commitments first |
| Live tile does not update | Hub disconnected, wrong profile, wrong goal id, backend not publishing | Refresh dashboard, confirm profile, record matching activity, check backend hub/logs |
| Live tile updates wrong count | Activity was edited/deleted or target changed | Inspect Activities, Commitments, and Frequencies for the same date |
| Review tile does not change while scrubbing | Tile is not review-aware or historical query failed | Use Review Goal History or Consistency Trend, check network/API errors |
| Scrubber jumps unexpectedly | Selected date restored from browser storage | Use Jump to today or drag to desired date |
| Dashboard layout feels broken | Manual edits became noisy | Enter edit mode, remove the disruptive tiles, exit edit mode, then rebuild the order with the "+" FAB |
| Avatar or asset does not load | Asset belongs to another profile or URL is stale | Re-upload under the active profile and save again |

## Live Stream Checks

For live dashboard issues, check in this order:

1. You are signed in.
2. The active profile is correct.
3. Dashboard mode is Live.
4. The tile supports Live mode.
5. The activity uses the behaviour tied to the tracked commitment.
6. The backend is running.
7. The SignalR hub connection is established.
8. The server is publishing a matching `goalProgressUpdated` or invalidation message.

## Review Checks

For review dashboard issues, check in this order:

1. Dashboard mode is Review.
2. The scrubber date label changes.
3. The tile supports Review mode.
4. The selected date is inside the review window.
5. Historical activity exists for the selected date.
6. The backend historical endpoint returns data.
7. Browser storage is not restoring an unexpected selected date.

## Recovery Moves

Use these low-risk recovery moves before changing data:

1. Refresh the page.
2. Switch Live -> Review -> Live.
3. Jump to today.
4. Confirm active profile.
5. Sign out and sign in.
6. Restart the frontend dev server.
7. Restart the backend.

## Glossary

| Term | Meaning |
|---|---|
| Activity | A recorded instance of performing a behaviour |
| Behaviour | A reusable action that can be tracked |
| Commitment | An expectation that links behaviour and frequency |
| Frequency | A cadence rule for a commitment |
| Live mode | Dashboard mode for current streamed metrics |
| Profile | Data ownership boundary |
| Review mode | Dashboard mode for historical metrics |
| Scrubber | Timeline control for selecting a historical review date |
| Tile | Dashboard component that shows a metric or summary |
| To-do | One-off task |

