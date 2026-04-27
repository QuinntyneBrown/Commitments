# 072 — Scrubber date pill is in the wrong slot per design `zNLnf`

## Status

FIXED — date pill is now adjacent to the "Reviewing" legend on the
left of the scrubber; 3/3 scrubber e2e tests still pass.

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

- [x] Visual screenshot pre-fix surfaced via earlier review captures.
- [x] Template re-ordered: `.review-scrubber__date` moved adjacent to
      `.review-scrubber__legend`.
- [x] 3/3 scrubber e2e tests pass; visual screenshot post-fix shows
      the new layout.
