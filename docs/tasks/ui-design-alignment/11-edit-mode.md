# 11 — Edit Mode: pink PrimaryHeader, EDIT MODE pill, DONE button, editable tile chrome

**Status: COMPLETE**

> 11.1 — `DashboardLayoutStore.isEditMode` + `setEditMode`/`toggleEditMode` already existed; reused. 11.2 — Right-side `edit` icon-button added to the `PrimaryHeader` `[header-actions]` slot when `editMode === false && mode === 'live'`. 11.3 — `PrimaryHeaderComponent` gained an `editMode` boolean input (option a from the task) that swaps the `.primary-header` background to `var(--cui-accent-strong)`. 11.4 — `EDIT MODE` pill rendered in the new `[title-adornment]` slot (`13%` white fill, `33%` white border, `edit` icon + Inter 11/700/0.8 label). 11.5 — `DONE` button in `[header-actions]` (white fill, accent-strong icon+label, 0 2px 6px shadow) clears edit mode via `setEditMode(false)`. 11.6 — `cui-mode-toggle` is hidden inside `@if (!isEditMode())`. 11.7 — first slice: `dashboard-grid` gets a `--edit-mode` class that overrides `gridster-item` border to `2px solid var(--cui-accent)`. The full editable-tile chrome (drag pill / resize handle / dedicated close-button position from `KB9Mx`) is deferred. 11.8 — FAB is unchanged so it still renders in edit mode (its visibility only depends on live mode).

> Added 2026-04-27 to track the new "Dashboard — LG (1280) — Edit Mode" frame (`fJpM0`) and the "Dashboard — LG (1280) — Add Tile Dialog" frame (`a2Cjz`), which both render the dashboard in edit mode.

**Design**: edit mode is a documented toggle on the dashboard. While active:
- The **PrimaryHeader** background flips from `$primary-dim` (#3F51B5) to `$accent-strong` (#F50057). Same height **80**, same horizontal padding **24**, same `space_between` justification.
- The **title group** (left, `titleGrp`, gap 16) holds:
  - "Dashboard" Inter / `$fs-h2` (28) / `$fw-medium` / `$text-primary`.
  - An **EDIT MODE pill** to the right of the title (`tpRmI`): pill-shaped (`$r-full`), `#FFFFFF22` fill, 1px `#FFFFFF55` stroke, padding `[6, 12]`, gap 6, contents — `edit` Material Symbols Rounded icon 14×14 `$text-primary` + "EDIT MODE" Inter / `$fs-xs` (11) / `$fw-bold` / letter-spacing 0.8 / `$text-primary`.
- A **DONE button** sits on the right side of the PrimaryHeader (`B3q9n` / `doneBtn`): white (`$text-primary`) fill, `$r-sm` corner, drop-shadow blur 6 y2 `#00000066`, padding `[8, 20]`, gap 8, align-centred contents — `check` icon 18×18 `$accent-strong` + "DONE" Inter / `$fs-body` (14) / `$fw-bold` / `$accent-strong` / letter-spacing 0.5.
- All dashboard tiles render using `Dashboard-Tile/Editable` (`KB9Mx`) — see [06-tile-chrome-and-styling.md](06-tile-chrome-and-styling.md). The 2px accent stroke and chrome strips reinforce the mode.
- The **FAB** (`FAB/Accent` `e9DYo`) is still rendered in edit mode (the dialog frame `a2Cjz` shows it covered by the dialog backdrop, not removed).
- The `cui-mode-toggle` from [05.2](05-dashboard-page-layout.md) is **not** visible in this frame; treat edit mode as orthogonal to live/review (you cannot enter edit mode from review). When edit mode turns on, hide the mode toggle until DONE is clicked.

**Implementation**: `dashboard-shell` already has an "Edit Layout"/"Done" button in the toolbar — but the toolbar is the wrong place (it's a Dashboard-page concern, not a global app-shell concern), and the visual state is wrong (no header colour change, no EDIT MODE pill, no editable tile chrome). [02-remove-extraneous-header-buttons.md](02-remove-extraneous-header-buttons.md) removes the toolbar button.

## Tasks
- [ ] **11.1** Add an `editMode` signal/state to the dashboard layer (e.g. on `LayoutStoreService` or a sibling `DashboardEditModeService`). Default `false`. Mode is dashboard-scoped, not app-scoped.
- [ ] **11.2** Surface entry into edit mode. Recommended affordance: a single "Edit" icon-button on the right of the PrimaryHeader when `editMode === false` and `mode === 'live'` (use `edit` Material Symbols Rounded icon, `Btn/Icon` style — see component `vl4SK`). It is **not** in the toolbar. Alternative: long-press / context menu on a tile. Pick whichever is consistent with the design system; if unsure, raise an ADR before adding additional non-design affordances.
- [ ] **11.3** When `editMode === true`, change the `PrimaryHeaderComponent` background to `var(--cui-accent-strong)`. Either:
  - (a) Add an `[editMode]` input on `PrimaryHeaderComponent` that toggles a CSS class swapping the background variable, or
  - (b) Expose a `[background]` input that defaults to `var(--cui-primary-dim)` and is overridden to `var(--cui-accent-strong)` by the consumer.
  Pick (a) if `PrimaryHeaderComponent` is dashboard-aware; pick (b) for a cleaner separation. Document the choice in the component header.
- [ ] **11.4** Render the **EDIT MODE pill** to the right of the page title inside `PrimaryHeaderComponent` body (only when `editMode === true`): `border-radius: var(--cui-radius-full)`, fill `rgba(255,255,255,0.13)`, 1px `rgba(255,255,255,0.33)` border, padding `6px 12px`, gap 6, with `edit` icon (14×14, `var(--cui-text-primary)`) + label "EDIT MODE" Inter / 11 / 700 / `letter-spacing: 0.8` / `var(--cui-text-primary)`.
- [ ] **11.5** Render the **DONE button** at the right side of `PrimaryHeaderComponent` (only when `editMode === true`): `var(--cui-text-primary)` (white) background, `border-radius: var(--cui-radius-sm)`, padding `8px 20px`, gap 8, `box-shadow: 0 2px 6px #00000066`, `check` icon 18×18 `var(--cui-accent-strong)` + label "DONE" Inter / 14 / 700 / `letter-spacing: 0.5` / `var(--cui-accent-strong)`. Click sets `editMode = false`.
- [ ] **11.6** Hide the `cui-mode-toggle` when `editMode === true` (the design doesn't show it in `fJpM0` or `a2Cjz`).
- [ ] **11.7** Switch all dashboard tiles to the editable chrome ([06.4](06-tile-chrome-and-styling.md)) when `editMode === true`. Drag, close, and resize affordances become active.
- [ ] **11.8** FAB stays rendered in edit mode (matches `fJpM0` and `a2Cjz`). It opens the same Add Tile dialog ([03](03-add-tile-fab-and-dialog.md)).

See [_reference.md](_reference.md) for tokens and node IDs.
