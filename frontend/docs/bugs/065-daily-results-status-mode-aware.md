# 065 — Daily Results tile shows "Live" status badge even in Review mode

## Status

OPEN — surfaced by visual capture
(`frontend/docs/bugs/screenshots/dashboard-review-1280.png`).

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

- [ ] Failing e2e test added (review mode shows "Review" badge).
- [ ] Tile updated.
- [ ] Test verified passing.
