---
id: 030
title: review-scrubber.component.scss hard-codes hex literals for colours that have --cui-* tokens
status: open
discovered: 2026-04-27
flow: dashboard-modes
severity: low
---

# review-scrubber.component.scss hard-codes hex literals for colours that have --cui-* tokens

## Summary

The review scrubber's stylesheet renders correctly because every
literal *happens* to match the design palette today:

| Selector | Literal | Token equivalent |
|---|---|---|
| `.review-scrubber { background }` | `#1E1E1E` | `--cui-surface` |
| `.review-scrubber { color }` | `#FFFFFF` | `--cui-text-primary` |
| `.review-scrubber__slider { accent-color }` | `#9FA8DA` | `--cui-primary` |
| `.review-scrubber__ticks { color }` | `#B0B0B0` | `--cui-text-secondary` |
| `.review-scrubber__date { color }` | `#9FA8DA` | `--cui-primary` |

So the scrubber visually fits the dark theme — but if a future
theme tweak shifts any token (e.g. surface goes `#1E1E1E` →
`#171717`), the scrubber will drift and quietly violate the
design system.

After bug 029 the controller suite runs, and after bug 027 +
028 + 011 the rest of the dashboard is consistently
token-driven. This is the last component still hard-coding hex
values that have first-class tokens.

## Reproduction

`grep -rn "color: #\|background: #\|accent-color: #" projects/dashboard-framework/src` → only
`review-scrubber.component.scss` lines 7, 8, 20, 26, 40 match.

## Fix outline (radically simple)

Five hex-to-token swaps in
`projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.scss`:

  - `.review-scrubber { background: var(--cui-surface); color: var(--cui-text-primary); }`
  - `.review-scrubber__slider { accent-color: var(--cui-primary); }`
  - `.review-scrubber__ticks { color: var(--cui-text-secondary); }`
  - `.review-scrubber__date { color: var(--cui-primary); }`

The translucent date-pill background
(`rgba(159, 168, 218, 0.18)`) is left as a literal for now —
moving it to `color-mix(in srgb, var(--cui-primary) 18%, transparent)`
would broaden the change. Tracked separately if it surfaces.

## Tests to add (failing first)

E2E (Playwright, POM): extend `e2e/pages/dashboard.page.ts` with
`reviewScrubberBackground()` (after switching to review mode)
and add a `dashboard.spec.ts` test asserting the scrubber's
computed `background-color` is `rgb(30, 30, 30)`. Currently
**passes** because the literal happens to equal the token —
but that's the whole point: the test pins the token's value as
the contract. After the fix, the test still passes; if the
token changes, the test catches drift.
