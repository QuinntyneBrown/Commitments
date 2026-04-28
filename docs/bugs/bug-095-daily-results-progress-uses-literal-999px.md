---
id: bug-095
title: Daily Results .progress uses literal border-radius 999px instead of the --cui-radius-full design-system token
status: Fixed
---

# Bug 095 — daily-results progress radius hardcoded

**Status**: Fixed

## Fix

Replaced `.progress { border-radius: 999px }` with
`var(--cui-radius-full, 9999px)`, matching the design's
`cornerRadius: 9999` and the existing
`button`/`chip` token pattern. `.progress span` (the fill)
already used `border-radius: inherit`, so it picks up the new
token automatically. 292/292 workspace tests green.

## Description

`docs/tiles/daily-results-tile/ui-design.pen` specifies
`cornerRadius: 9999` on both `drTrack` and `drFill`. The cui
tokens module declares the matching token:

```scss
--cui-radius-full: 9999px;
```

`button` and `chip` already consume it via
`border-radius: var(--cui-radius-full, 9999px)`. The
daily-results progress bar instead hardcodes:

```scss
.progress {
  border-radius: 999px;
}
```

A literal one-off 999 (instead of 9999) — slightly wrong number
*and* not bound to the design system. If the radius token ever
changes, this rule won't follow.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.spec.ts`

## Reproduction

```bash
grep -n "border-radius\s*:\s*999px" frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.scss
```

Returns one match.

## Expected

`.progress { border-radius: var(--cui-radius-full, 9999px); }`,
matching the existing button/chip pattern and the design.

## Verification

- Unit (CSS source): assert `.progress` block uses
  `var(--cui-radius-full, 9999px)` and not literal `999px`.
- All existing daily-results specs continue to pass.
