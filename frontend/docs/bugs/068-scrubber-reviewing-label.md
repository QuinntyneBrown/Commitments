# 068 — Scrubber missing "Reviewing" label per design `zNLnf`

## Status

OPEN — surfaced via design comparison.

## Symptom

Design `Scrubber/Timeline (zNLnf)` opens with a clock icon followed by
the static label **"Reviewing"** before the bold selected date:

```
🕐 Reviewing  Wed · April 15, 2026  ── timeline ──  ◀ ▶ ▶  >> Jump to today
```

Running scrubber has no such label — it jumps straight to the
prev/play/next icon buttons.

## Fix

Add a `<span class="review-scrubber__legend">` element at the start of
the scrubber containing a `history` (or `update` / clock) icon plus the
text "Reviewing". Keep it presentational only (`aria-hidden="true"`)
so the existing slider's `role="slider"` is still the announced
interaction.

## Resolution

- [ ] Failing e2e assertion added (legend visible in review mode).
- [ ] Template + scss updated.
- [ ] Visual screenshot post-fix shows the label.
