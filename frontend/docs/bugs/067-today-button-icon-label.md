# 067 — Scrubber "Today" button icon + label don't match design `zNLnf`

## Status

OPEN — surfaced via design comparison.

## Symptom

Running scrubber's jump-to-today button:

```html
<button class="review-scrubber__today" aria-label="Jump to today">
  <span class="material-symbols-rounded">today</span>
  <span class="review-scrubber__today-label">Today</span>
</button>
```

→ shows the calendar `today` icon + label **"Today"**.

Design `zNLnf` (`Scrubber/Timeline`) shows `>> Jump to today` —
the `keyboard_double_arrow_right` (or equivalent fast-forward) icon
plus the full label **"Jump to today"**.

## Fix

Two-line edit:
- icon: `today` → `keyboard_double_arrow_right`
- label text: `Today` → `Jump to today`

The existing e2e assertion (`label.toHaveText(/today/i)`) still
passes — `/today/i` matches "Jump to today" too.

## Resolution

- [ ] Visual screenshot pre-fix.
- [ ] Template updated.
- [ ] Test verified passing post-fix; visual screenshot post-fix.
