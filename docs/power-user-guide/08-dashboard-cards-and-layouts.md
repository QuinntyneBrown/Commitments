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

## Add Dashboard Cards

Dashboard cards can be added from the dashboard add flow.

Basic workflow:

1. Open Dashboard.
2. Use the add action.
3. Select one or more cards.
4. Confirm.
5. Verify the cards appear in the dashboard grid.

## Configure Dashboard Cards

Some cards expose configuration options.

Basic workflow:

1. Open the card configuration action.
2. Change the option values.
3. Save.
4. Confirm the card updates.

Cancel should leave the current card unchanged.

## Remove Dashboard Cards

Remove a card when it no longer serves the current dashboard.

Basic workflow:

1. Open the card delete or remove action.
2. Confirm if prompted.
3. Verify the card disappears.
4. Refresh the dashboard if needed to confirm persistence.

## Modern Tile Layout

The dashboard framework provides a tile grid with mode-aware tile context.

Available tile actions can include:

- Add Tile.
- Edit Layout.
- Done.
- Reset.
- Remove tile.
- Maximize or restore tile.
- Focus tile.

Use Edit Layout for grid maintenance. Use Reset when manual layout changes no longer help scanning.

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

