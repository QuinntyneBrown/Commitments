# 065 — Daily Results tile shows "Live" status badge even in Review mode

## Status

FIXED — visual screenshot post-fix shows the Daily Results badge as
"Review" when the dashboard is in review mode; full lg-desktop e2e
58/58 including the new mode-aware status guard.

## Symptom

After switching the dashboard to Review mode, the Daily Results tile
still wears a "Live" status pill.

The dashboard-modes flow doc explicitly says:

> tiles either wear a `REVIEW` badge or are hidden entirely if their
> `supportedModes` excludes review.

## Root cause

`daily-results-tile.component.html` hard-codes the status:

```html
<commitments-tile-shell title="Daily Results" eyebrow="Today"
                        icon="today" status="Live">
```

It never reads the dashboard mode, so the badge never updates.

## Fix

Inject `TILE_CONTEXT` and bind status from `mode()`:

- mode === 'live'   → "Live"
- mode === 'review' → "Review"

When `TILE_CONTEXT` isn't available (e.g. unit-test bare instantiation),
fall back to "Live" so existing behaviour holds.

## Resolution

- [x] Failing e2e test added in `dashboard-mode.spec.ts`.
- [x] `daily-results-tile` injects `TILE_CONTEXT` and computes
      `status() = mode === 'review' ? 'Review' : 'Live'`.
- [x] Test passes; full lg-desktop e2e 58/58.
