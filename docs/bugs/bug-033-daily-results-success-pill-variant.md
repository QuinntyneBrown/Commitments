---
id: bug-033
title: Daily Results — LIVE pill renders accent-pink, but the .pen drLive pill is success-green (#66BB6A)
status: Fixed
---

# Bug 033 — Daily Results pill colour wrong for success-themed tile

**Status**: Fixed

## Fix

- `StatusPillVariant` gains a `'success'` member.
- New `.status-pill--success` rule uses `var(--cui-success)` for the
  text/dot colour and a 14% tint for the background.
- `DailyResultsTileComponent` adds a `pillVariant` computed signal
  that returns `'success'` for live mode and `'review'` for review.
- Template `[variant]` and `[pulse]` now read the computed; the user
  still sees `LIVE`/`REVIEW` because `statusLabel` is unchanged.

Mirrors the chart-variant pattern from bug-031. SASS mixin extraction
deferred until a third themed variant lands (currently chart and
success both follow the same `colour + color-mix(14%)` shape).

Coverage:
- `status-pill.component.spec.ts` asserts `.status-pill--success`
  references `var(--cui-success)`.
- `daily-results-tile.component.spec.ts` asserts the TS source has
  `pillVariant` returning `'success'` (regex tolerant of method or
  computed-signal forms).
- All 20 affected suites pass (90/90 — was 88/88 before).

## Description

`docs/tiles/daily-results-tile/ui-design.pen` (frame `drLive`) ships
the `LIVE` pill in success green:

- pill background `#66BB6A22`
- dot `#66BB6A`
- text `LIVE` `#66BB6A`

The whole daily-results tile is themed around `--cui-success`
(`#66BB6A`) — the headline `7 / 9` value, the progress bar fill, and
the design's pill all use that token. But `status-pill.component.scss`
only defines `live` (accent-pink), `review`, `neutral`, and the new
`chart` variant from bug-031. The default mapping for live-mode tiles
is `live`, so daily-results renders a pink pulsing pill inside an
otherwise green card.

Pattern is identical to bug-031 (consistency-trend → chart-blue): add
a themed pill variant and have the tile route live mode to it.

## Affected files

- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.ts`
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.ts`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/daily-results-tile/daily-results-tile.component.html`

## Reproduction

1. Render the Daily Results tile in live mode (default).
2. Observe a pink pulsing `LIVE` pill in the header right slot.
3. Compare to the .pen — the pill is success green (`#66BB6A`).

## Expected

- New `success` variant on `StatusPillComponent` mapped to
  `var(--cui-success)`.
- `DailyResultsTileComponent` exposes a `pillVariant()` method
  returning `'success'` for live mode and `'review'` for review
  (consistent with the consistency-trend pattern).
- Pulse keeps animating in live snapshots.
- `pillLabel()` reads the underlying mode so the user still sees
  `LIVE` / `REVIEW`.

## Verification

- Unit:
  - `status-pill.component.scss` declares a `.status-pill--success`
    rule that references `var(--cui-success)`.
  - `daily-results-tile.component.ts` `pillVariant()` body returns
    `'success'`.
- Visual: the `LIVE` pill renders green inside the daily-results
  tile.
