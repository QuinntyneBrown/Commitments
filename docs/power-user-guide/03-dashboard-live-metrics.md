# Dashboard Live Metrics

Live mode is the operational view. It shows current dashboard metrics and listens for real-time updates from the backend so progress changes can appear without a page refresh.

## What Live Mode Shows

Live mode is intended for current-state monitoring:

- Today's goal progress.
- Daily completion counts.
- Weekly focus.
- Monthly progress.
- Outstanding to-dos.
- Relations or balance metrics.
- Consistency trend charts.
- Live goal metrics streamed from activity changes.

The current dashboard tile catalog includes these reusable tiles:

| Tile | Best use |
|---|---|
| Daily Results | Today's completed vs expected commitments |
| Weekly Focus | Current week priorities |
| Monthly Progress | Thirty-day progress scan |
| Outstanding To Dos | Open task count |
| Relations | Balance across relation categories |
| Consistency Trend | Goal completion rate over a window |
| Live Goal Metrics | Real-time count, target, percent, and last 14 days |

## Enter Live Mode

1. Open Dashboard.
2. Use the mode toggle and choose Live.
3. Confirm live tiles show current values and any `LIVE` status pill where applicable.

When Live is active, the dashboard shows the tile selector and Add Tile action. These controls are hidden from Review mode because review is for historical inspection, not changing the live monitoring setup.

## Add a Live Tile

1. Open Dashboard.
2. Switch to Live mode.
3. Choose a tile from the tile selector.
4. Select Add Tile.
5. Confirm the tile appears in the dashboard grid.

Power users commonly keep these tiles visible:

- Live Goal Metrics for one important commitment.
- Consistency Trend for rate-of-change.
- Daily Results for today's operating status.
- Outstanding To Dos for execution pressure.

## Read the Live Goal Metrics Tile

The Live Goal Metrics tile shows:

- `LIVE` status pill with pulse.
- Current percent complete.
- Caption such as count of target done today.
- Delta versus yesterday.
- Last 14 day bar history.
- Count and target readout.

When a matching `goalProgressUpdated` message arrives, the tile updates its count signal immediately. It filters messages by goal id so unrelated activity does not change the wrong tile.

## Read the Consistency Trend Tile

The Consistency Trend tile shows:

- Live or Review status depending on dashboard mode.
- Current completion percentage.
- Peak and low percentages.
- Delta badge.
- Chart.js line chart for the trend window.

In Live mode, the trend represents the latest available state. Activity changes can cause the latest point to refresh.

## How Metrics Stream

The app uses a SignalR hub at `/hub` for live messages. After authentication, the client connects with the active profile context. The backend publishes profile-scoped messages such as:

- `goalProgressUpdated`
- `dashboardTileDataInvalidated`
- optional tile-specific update messages

The important behaviour for users is simple: when an activity, commitment, frequency, to-do, or profile-scoped dashboard input changes, affected live tiles should update or refetch.

## Actions That Change Live Metrics

These actions commonly affect live tiles:

- Recording an activity.
- Editing or deleting an activity.
- Completing a to-do.
- Creating, editing, or deleting a commitment.
- Changing a commitment frequency.
- Switching profiles.

After making one of these changes, return to Dashboard Live mode and confirm the relevant tile changed.

## Edit the Dashboard Layout

1. Open Dashboard.
2. Select Edit Layout.
3. Reorder, resize, or adjust tiles as supported by the grid.
4. Select Done.

Use Reset if the layout becomes noisy or you want to return to the default arrangement.

## Live Mode Operating Routine

Morning:

1. Open Dashboard.
2. Switch to Live.
3. Confirm Daily Results, Weekly Focus, and Outstanding To Dos.
4. Add any missing tile needed for the day.

During the day:

1. Record activities as they happen.
2. Complete to-dos when finished.
3. Keep the dashboard visible if you need live feedback.

End of day:

1. Confirm live totals.
2. Switch to Review mode.
3. Scrub through the day or recent days to compare how metrics moved.

