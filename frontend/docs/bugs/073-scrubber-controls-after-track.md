# 073 — Scrubber prev/play/next sit before the track; design `zNLnf` has them after

## Status

FIXED — controls now cluster after the track adjacent to Jump-to-today.

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

- [x] Visual screenshot pre-fix surfaced via earlier review captures.
- [x] Template re-ordered: prev/play/next moved after `.review-scrubber__track`.
- [x] 3/3 scrubber e2e tests pass; visual screenshot post-fix matches
      design `zNLnf` order.
