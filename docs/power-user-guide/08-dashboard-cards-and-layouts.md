# Dashboard Cards and Layouts

Dashboard cards and layouts are the advanced configuration layer for how metrics appear.

## Dashboard Concepts

| Concept | Meaning |
|---|---|
| Dashboard | The profile-owned workspace of cards and tiles |
| Card | A reusable dashboard content definition |
| Card Layout | A reusable layout definition for a card |
| Dashboard Card | A card instance placed on a profile dashboard |
| Tile | A modern dashboard component registered with the dashboard framework |

## Card Catalog

Use Cards to manage reusable dashboard card definitions. These records describe what can be added to a dashboard.

Basic workflow:

1. Open Cards.
2. Add or edit a card name and description.
3. Save.
4. Use it when adding dashboard cards.

## Card Layouts

Use Card Layouts to manage reusable layout definitions.

Basic workflow:

1. Open Card Layouts.
2. Add or edit a layout name and description.
3. Save.
4. Select the layout when configuring dashboard cards.

## Add Dashboard Tiles

The dashboard uses a FAB-and-dialog flow for adding tiles. The legacy inline tile selector and "Add Tile" / "Edit Layout" / "Reset" buttons in the header have been removed.

Basic workflow:

1. Open Dashboard in Live mode (or enter edit mode).
2. Click the "+" FAB at the bottom-right.
3. In the Add Tile dialog, click one tile cell to select it.
4. Click ADD TILE to commit, or CANCEL / close icon to discard.
5. Verify the tile appears in the dashboard grid.

Repeat the flow to add additional tiles.

## Configure Dashboard Tiles

Some tiles expose configuration options.

Basic workflow:

1. Open the tile configuration action from its header menu.
2. Change the option values.
3. Save.
4. Confirm the tile updates.

Cancel should leave the current tile unchanged.

## Remove Dashboard Tiles

Remove a tile when it no longer serves the current dashboard.

Basic workflow:

1. Enter edit mode from the dashboard primary header.
2. Use the close action on the editable tile chrome.
3. Click DONE in the primary header to exit edit mode.
4. Refresh the dashboard if needed to confirm persistence.

## Modern Tile Layout

The dashboard framework provides a tile grid with mode-aware tile context. Tile chrome and available actions depend on dashboard mode:

| Mode | What you see | Available actions |
|---|---|---|
| Live | Default tile chrome, mode toggle, "+" FAB | Add tile, enter edit mode, per-tile menu |
| Review | Default tile chrome, mode toggle, scrubber | Per-tile menu (no add or edit) |
| Edit (entered from Live) | Editable tile chrome with 2px accent border, EDIT MODE pill, DONE button, "+" FAB | Reorder, resize, remove tiles, add tiles |

## Tile Modes

Tiles may support:

- Live only.
- Review only.
- Live and Review.

Examples:

- Live Goal Metrics is designed for Live mode.
- Review Goal History is designed for Review mode.
- Consistency Trend can respond to both modes.

If a tile is missing from the selector, check the current dashboard mode. Some tiles are intentionally hidden outside their supported mode.

## Suggested Dashboard Layouts

Daily operations layout:

1. Daily Results.
2. Live Goal Metrics.
3. Outstanding To Dos.
4. Weekly Focus.
5. Consistency Trend.

Review layout:

1. Review Goal History.
2. Consistency Trend.
3. Relations.
4. Monthly Progress.

Executive summary layout:

1. Daily Results.
2. Monthly Progress.
3. Relations.
4. Outstanding To Dos.

