# 073 — Scrubber prev/play/next sit before the track; design `zNLnf` has them after

## Status

OPEN — surfaced via design comparison after 072.

## Symptom

Running scrubber after 072:

```
[Reviewing] [Date] [prev] [play] [next] ── timeline ── [Jump to today]
```

Design `Scrubber/Timeline (zNLnf)`:

```
[Reviewing] [Date] ── timeline ── [prev] [play] [next] [Jump to today]
```

The transport controls cluster on the **right** in the design, next to
the Jump-to-today button.

## Fix

Move the three `<cui-icon-button>` elements (prev, play, next) so they
follow `<div class="review-scrubber__track">` in
`review-scrubber.component.html`. Pure template re-order; no class /
testid changes.

## Resolution

- [ ] Visual screenshot pre-fix.
- [ ] Template re-ordered.
- [ ] e2e tests still green; visual screenshot post-fix.
