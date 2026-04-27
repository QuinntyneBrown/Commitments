# 072 — Scrubber date pill is in the wrong slot per design `zNLnf`

## Status

OPEN — surfaced via design comparison.

## Symptom

Running scrubber lays out as:

```
[Reviewing] [prev] [play] [next] ── timeline ── [Date pill] [Jump to today]
```

Design `Scrubber/Timeline (zNLnf)` lays out as:

```
[Reviewing] [Bold date] ── timeline ── [prev] [play] [next] [Jump to today]
```

So the date sits next to the "Reviewing" legend on the **left**, not
between the timeline and the "Jump to today" button.

## Fix

Move the `<span class="review-scrubber__date">` element so it follows
the legend (immediately after the existing `.review-scrubber__legend`)
in `review-scrubber.component.html`. Keep the existing testid + class
intact so the existing `review-scrubber-date` test continues to find it.

## Resolution

- [ ] Visual screenshot pre-fix.
- [ ] Template re-ordered.
- [ ] e2e tests still green; visual screenshot post-fix.
