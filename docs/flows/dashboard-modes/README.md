# Dashboard modes (Live / Review)

## Summary

The dashboard supports two modes, toggled by a segmented control at the top of the shell:

- **Live** — real-time stream of goal/commitment progress; tiles wear a `LIVE` badge; the Add Tile control is visible; the historical scrubber is hidden.
- **Review** — read-only snapshot at a chosen date in the past; the scrubber is visible at the top of the grid; the Add Tile control is hidden; tiles either wear a `REVIEW` badge or are hidden entirely if their `supportedModes` excludes review.

The selected mode persists across page reloads. The scrubber drives a `selectedReviewDate` signal that all review tiles read.

This flow is partially covered by `dashboard-mode.spec.ts`, `mode-persistence.spec.ts`, and `viewport.spec.ts`. Use those as reference tests.

## Surface area

- Shell: `dashboard-shell.component.{ts,html}` (the `cui-mode-toggle` + scrubber slot)
- Mode toggle component: `frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.{ts,html}`
- Scrubber: `df-review-scrubber` (`dashboard-framework`)
- Mode service: `dashboard-framework`'s `modeService` exposes `mode()` and `setMode(...)`
- Tile context: `TILE_CONTEXT.mode()`, `TILE_CONTEXT.selectedReviewDate()` (read by every plugin tile)
- Persistence: mode is stored in `localStorage` (verify the exact key in the dashboard-framework source).

## Preconditions

- Authenticated, dashboard reachable at `/`.
- Tests should reset relevant `localStorage` keys in `beforeEach`.

## Steps

1. **Default mode is Live.**
   - Visit `/`.
   - **Assert:** the `Live` segment of `dashboard-mode-toggle` has `aria-pressed="true"`; the `Add Tile` button is visible; the `dashboard-scrubber-slot` is hidden.

2. **Switch to Review.**
   - Click the `Review` segment.
   - **Assert:** `Review` segment has `aria-pressed="true"`; `Add Tile` button is hidden; `dashboard-scrubber-slot` is visible (renders `df-review-scrubber`).

3. **Switch back to Live.**
   - Click the `Live` segment.
   - **Assert:** behaviour matches step 1.

4. **Mode persists across reload.**
   - Switch to Review → reload the page.
   - **Assert:** `Review` segment remains active after the reload; scrubber is still visible.

5. **Catalog filters by supported modes.**
   - In Live mode, open the tile catalog.
   - **Assert:** only tiles whose `supportedModes` includes `live` are listed (`Review Goal History` is excluded).
   - Switch to Review and re-open the catalog (note: the catalog is hidden in review mode; if the design changes to keep it visible, this step should be updated).

6. **Scrubber drives review-only tiles.**
   - In Review mode, add a `Review Goal History` tile.
   - Move the scrubber to a chosen date.
   - **Assert:** the tile's `review-goal-history-date-badge` reflects the chosen date; the tile's count/target reflect the count of activities up to and including that date for the bound goal.

7. **Live tiles can also render in review mode.**
   - With `Consistency Trend` added, switch to Review and scrub.
   - **Assert:** the line graph's highlighted point moves to match `selectedReviewDate`; the tile's status pill switches from `LIVE` to `REVIEW`.

8. **Responsive: behaviour holds at three viewports.**
   - Repeat steps 1–3 at 768×1024 (tablet), 1280×800 (lg-desktop), 1920×1080 (xl-desktop).
   - **Assert:** mode toggle is visible and functional at every viewport (already covered by `viewport.spec.ts`).

## Selectors

| Need | Selector |
| --- | --- |
| Mode toggle root | `getByTestId('dashboard-mode-toggle')` |
| Live segment | `getByTestId('dashboard-mode-toggle').locator('.mode-toggle__segment--live')` |
| Review segment | `getByTestId('dashboard-mode-toggle').locator('.mode-toggle__segment--review')` |
| Scrubber slot | `getByTestId('dashboard-scrubber-slot')` |
| Add Tile button (live only) | `getByTestId('add-tile')` |
| Live badge on a tile | `getByTestId('live-goal-metrics-live-badge')` (and analogous per tile) |
| Review badge on a tile | `getByTestId('review-goal-history-review-badge')` |
| Date badge on review tile | `getByTestId('review-goal-history-date-badge')` |

> Existing e2e helpers: `DashboardModePage` (`switchToReview`, `switchToLive`, `expectLiveActive`, `expectReviewActive`).

## Edge cases

- Pre-existing DI bug: `ReviewScrubberController` may fail to instantiate (observed during demo recording); a Playwright test must either fix this or wrap the click in a soft assertion. Track in a follow-up.
- Mode `localStorage` corrupted → shell should fall back to `live`.
- Switching mode while a tile is mid-fetch → the tile must abort or ignore the stale response.
- Scrubber with date in the future → handler clamps `asOf` to `UtcNow` (`GetGoalTrendHandler`).
