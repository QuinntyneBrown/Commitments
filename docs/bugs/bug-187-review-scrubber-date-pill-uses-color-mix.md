---
id: bug-187
title: review-scrubber date pill uses literal rgba instead of color-mix on --cui-primary
status: Open
---

# Bug 187 — review-scrubber date pill should use `color-mix()` on `--cui-primary`

## Description

`review-scrubber.component.scss` line 74:

```scss
.review-scrubber__date {
  ...
  background: rgba(159, 168, 218, 0.18);
  color: var(--cui-primary, #9FA8DA);
  ...
}
```

That's `--cui-primary` (#9FA8DA = rgb(159, 168, 218)) at 18%
opacity. The `status-pill` component expresses the same
visual via `color-mix()` against the design token:

```scss
.status-pill--review {
  background: color-mix(in srgb, var(--cui-primary, #9FA8DA) 18%, transparent);
}
```

Normalize the review-scrubber date pill to the same form so
both "review-mode pill" surfaces are tied to the same token.

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.scss`

## Reproduction

```bash
grep -n 'rgba(159, 168, 218' frontend/projects/dashboard-framework/src/lib/dashboard/review-scrubber/review-scrubber.component.scss
```

Returns one match.

## Expected

```scss
background: color-mix(in srgb, var(--cui-primary, #9FA8DA) 18%, transparent);
```

A regression-guard spec asserts the literal rgba is gone.

## Verification

- New regression spec confirms the swap.
- All other tests continue to pass.
