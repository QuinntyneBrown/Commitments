---
id: bug-165
title: tokens.ts exports 40+ constants but only 3 are used in production — prune
status: Open
---

# Bug 165 — Prune unused TS token constants

## Description

`commitments-ui/src/lib/tokens/tokens.ts` mirrors design-system
CSS custom properties as TypeScript constants so canvas-drawn
artefacts (e.g. Chart.js gradients) can use the same palette.
Cross-repo grep shows that out of 40+ exported constants, only
**three** are actually imported by production code:

- `ACCENT_CHART` — used by consistency-trend.controller +
  consistency-trend-tile component
- `BG_APP` — used by consistency-trend.controller
- `TEXT_MUTED` — used by consistency-trend-tile component

Plus the `DashboardMode` type.

The other 40+ constants (BG_TOOLBAR, BG_SIDEBAR,
SURFACE_*, DIVIDER*, ACCENT_LIVE/REVIEW/SUCCESS/WARN,
TEXT_PRIMARY/SECONDARY/ON_DARK_ACCENT, RADIUS_*, SP_*, FS_*,
FW_*, FONT_*, TOOLBAR_HEIGHT, HEADER_HEIGHT, SIDENAV_WIDTH,
ON_ACCENT, etc.) are never imported. Their `tokens.spec.ts`
exists purely to assert their literal values — testing them
without any production caller.

YAGNI: drop the unused constants and the spec tests for them.
The CSS custom properties (in `_tokens.scss`) remain — they
are the canonical source for stylesheet usage. The TS
constants are only needed for canvas/runtime drawing, where
only the three above are exercised.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tokens/tokens.ts` (drop 40+ unused exports, keep ACCENT_CHART, BG_APP, TEXT_MUTED, DashboardMode)
- `frontend/projects/commitments-ui/src/lib/tokens/tokens.spec.ts` (update tests to match the trimmed surface)

## Reproduction

```bash
grep -rn 'BG_TOOLBAR\|BG_SIDEBAR\|SURFACE_TILE\|SURFACE_RAISED\|TEXT_PRIMARY\|TEXT_SECONDARY\|RADIUS_\|FS_\|FW_\|SP_' frontend/projects --include='*.ts'
```

Each match falls inside `tokens.ts` (declaration) or
`tokens.spec.ts` (test). No production caller.

## Expected

- Only `ACCENT_CHART`, `BG_APP`, `TEXT_MUTED`, and
  `DashboardMode` remain in `tokens.ts`.
- `tokens.spec.ts` tests only those three constants.
- Regression-guard spec asserts `tokens.ts` LOC drops below
  20 lines.

## Verification

- New regression spec confirms the file is significantly
  smaller.
- All other tests continue to pass.
