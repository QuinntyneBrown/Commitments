# Dashboard modes (Live / Review)

## Summary

The dashboard supports two modes, toggled by a segmented control rendered in the PrimaryHeader's `header-actions` slot:

- **Live** — real-time stream of goal/commitment progress; tiles wear a `LIVE` badge; the Add Tile FAB is visible (bottom-right); the historical scrubber is hidden.
- **Review** — read-only snapshot at a chosen date in the past; the scrubber is rendered between the PrimaryHeader and the grid; the Add Tile FAB is hidden; tiles either wear a `REVIEW` badge or are hidden entirely if their `supportedModes` excludes review.

The selected mode persists across page reloads. The scrubber drives a `selectedReviewDate` signal that all review tiles read.

While **edit mode** is active, the mode-toggle is hidden — edit mode is orthogonal to live/review and only entered from live (see [`dashboard-layout`](../dashboard-layout/README.md)).

This flow is partially covered by `dashboard-mode.spec.ts`, `mode-persistence.spec.ts`, and `viewport.spec.ts`. Use those as reference tests; note that the existing `DashboardModePage` page-object still references the old `add-tile` testid — the FAB is now `add-tile-fab`.

## Surface area

- Dashboard shell: `dashboard-shell.component.{ts,html}` — composes `app-primary-header` + `cui-mode-toggle` (in `[header-actions]` slot when not in edit mode) + scrubber slot + grid + FAB.
- Mode toggle component: `frontend/projects/commitments-ui/src/lib/mode-toggle/mode-toggle.component.{ts,html}` (renders two `mat-button-toggle` segments).
- PrimaryHeader: `frontend/projects/commitments-ui/src/lib/primary-header/primary-header.component.{ts,html}` (with `[editMode]` input + `[title-adornment]` + `[header-actions]` slots).
- Scrubber: `df-review-scrubber` (`dashboard-framework/src/lib/dashboard/review-scrubber/...`) — only rendered when `mode === 'review'`.
- Mode service: `DashboardModeService` (`dashboard-mode.service.ts`) exposes `mode()`, `selectedReviewDate()`, `setMode(...)`, `setSelectedReviewDate(...)`.
- Tile context: `TILE_CONTEXT.mode()`, `TILE_CONTEXT.selectedReviewDate()`, `TILE_CONTEXT.isEditMode()` (read by every plugin tile).
- Persistence: mode at `localStorage[commitments.dashboardMode]`; selected review date at `localStorage[commitments.selectedReviewDate]` (constants `MODE_STORAGE_KEY`, `REVIEW_DATE_STORAGE_KEY` in `dashboard-mode.service.ts`).

## Preconditions

- Authenticated, dashboard reachable at `/`.
- Tests should reset relevant `localStorage` keys (`commitments.dashboardMode`, `commitments.selectedReviewDate`, and per-mode layout keys) in `beforeEach`.

## Steps

1. **Default mode is Live.**
   - Visit `/` with a clean `localStorage`.
   - **Assert:** the `Live` segment of `dashboard-mode-toggle` has the `mode-toggle__segment--active` class; the `add-tile-fab` is visible; no `dashboard-scrubber-slot` is in the DOM.

2. **Switch to Review.**
   - Click the `Review` segment.
   - **Assert:** `Review` segment now has `mode-toggle__segment--active`; `add-tile-fab` is no longer in the DOM; `dashboard-scrubber-slot` is visible (renders `df-review-scrubber`); the first time the user enters review, `localStorage[commitments.selectedReviewDate]` gets seeded with today's ISO date.

3. **Switch back to Live.**
   - Click the `Live` segment.
   - **Assert:** behaviour matches step 1.

4. **Mode persists across reload.**
   - Switch to Review → reload the page.
   - **Assert:** `Review` segment is active after the reload; scrubber is still visible; `localStorage[commitments.dashboardMode] === 'review'`.

5. **Catalog filters by supported modes.**
   - In Live mode, click the FAB to open the Add Tile dialog.
   - **Assert:** only tiles whose `supportedModes` includes `live` are listed (`Review Goal History` is excluded).
   - Cancel, switch to Review — the FAB is gone, so the catalog is unreachable from the UI in review. (If the design later surfaces an Add Tile affordance in review, update this step.)

6. **Edit mode hides the mode-toggle.**
   - In Live mode, click `edit-mode-enter`.
   - **Assert:** `dashboard-mode-toggle` is gone from the header; clicking `edit-mode-done` restores it.

7. **Scrubber drives review-only tiles.**
   - In Review mode, ensure a `Review Goal History` tile is on the layout (add via the registry/store; FAB is hidden in review).
   - Move the scrubber to a chosen date.
   - **Assert:** the tile's `review-goal-history-date-badge` reflects the chosen date; the tile's count/target reflect the count of activities up to and including that date for the bound goal.

8. **Live tiles can also render in review mode.**
   - With `Consistency Trend` added in live, switch to Review and scrub.
   - **Assert:** the line graph's highlighted point moves to match `selectedReviewDate`; the tile's status pill switches from `LIVE` to `REVIEW`.

9. **Responsive: behaviour holds at three viewports.**
   - Repeat steps 1–3 at 768×1024 (tablet), 1280×800 (lg-desktop), 1920×1080 (xl-desktop).
   - **Assert:** mode toggle is visible and functional at every viewport (already covered by `viewport.spec.ts`).

## Selectors

| Need | Selector |
| --- | --- |
| Mode toggle root | `getByTestId('dashboard-mode-toggle')` |
| Live segment | `.mode-toggle__segment--live` (active class: `.mode-toggle__segment--active`) |
| Review segment | `.mode-toggle__segment--review` (active class: `.mode-toggle__segment--active`) |
| Scrubber slot | `getByTestId('dashboard-scrubber-slot')` |
| Add Tile FAB (live only) | `getByTestId('add-tile-fab')` |
| Edit-mode enter | `getByTestId('edit-mode-enter')` |
| Edit-mode done | `getByTestId('edit-mode-done')` |
| Live badge on a tile | `getByTestId('live-goal-metrics-live-badge')` (and analogous per tile) |
| Review badge on a tile | `getByTestId('review-goal-history-review-badge')` |
| Date badge on review tile | `getByTestId('review-goal-history-date-badge')` |

> Existing e2e helpers: `DashboardModePage` (`switchToReview`, `switchToLive`, `expectLiveActive`, `expectReviewActive`). Note: `expectLiveActive`/`expectReviewActive` currently look for `aria-pressed` on a tab role and the `add-tile` testid — update them to use `mode-toggle__segment--active` and `add-tile-fab`.

## Edge cases

- Pre-existing DI bug: `ReviewScrubberController` may fail to instantiate (observed during demo recording); a Playwright test must either fix this or wrap the click in a soft assertion. Track in a follow-up.
- Mode `localStorage` corrupted → `readStoredMode()` falls back to `'live'`.
- Switching mode while a tile is mid-fetch → the tile must abort or ignore the stale response.
- Scrubber with date in the future → handler clamps `asOf` to `UtcNow` (`GetGoalTrendHandler`).
- Live and review have **independent persisted layouts** (`commitments.layout.live` vs `commitments.layout.review`); switching mode does not mutate the other.
