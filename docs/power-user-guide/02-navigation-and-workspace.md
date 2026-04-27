# Navigation and Workspace

The Commitments workspace is organized around a persistent application shell and a small set of recurring interaction patterns.

## Application Shell

The authenticated shell uses a two-row header over a sidenav-and-content body:

- **Toolbar row** (height 64) — hamburger menu button on the left, "Commitments" brand text, flex spacer, active profile name, and a circular profile avatar on the right.
- **Primary header row** (height 80) — the page title (for example, "Dashboard"). On the dashboard page this row also hosts the right-aligned mode toggle (Live / Review) and, while edit mode is active, the EDIT MODE pill and DONE button.
- **Sidenav** (width 250) — left-anchored navigation list with active and hover states. The hamburger button in the toolbar toggles it open and closed.
- **Main content area** — fills the remaining space and scrolls independently.

On narrower screens, use the hamburger button to open the sidenav; on desktop it stays expanded by default. Inter is used for all text in the shell, with Material Symbols Rounded icons.

## Primary Navigation

The sidenav lists the primary destinations in this order, each with a Material Symbols Rounded icon:

| Item | Icon | What you do there |
|---|---|---|
| Dashboard | `dashboard` | Monitor live metrics, switch to review mode, edit dashboard layout |
| Activities | `event_available` | Record and inspect behaviour activity |
| Behaviours | `repeat` | Maintain the behaviour catalog |
| Behaviour Types | `category` | Maintain behaviour categories |
| Commitments | `task_alt` | Create and edit expectations |
| Cards | `style` | Manage dashboard card definitions |
| Card Layouts | `dashboard_customize` | Manage dashboard card layout definitions |
| Frequencies | `schedule` | Maintain frequency rules |
| Notes | `description` | Create or edit rich-text notes |
| Profiles | `person` | Manage profiles |
| To Do's | `format_list_bulleted` | Create, complete, edit, and delete tasks |
| Logout | `logout` | Clear the session and return to login |

The active item is highlighted with a translucent primary fill and a 3px primary left-border. Hover applies a subtle white overlay.

## Common Page Patterns

Most management pages use a grid:

- Rows represent records.
- Edit actions open a dialog or editor.
- Delete actions remove the record from active lists.
- Checkbox cells mark boolean state, such as completion.
- Pagination limits list size for scanning.
- Create actions appear as a button or floating action button.

## Dialogs and Editors

Dialogs are used for focused edits such as activities, behaviours, commitments, dashboard cards, frequencies, profiles, and to-dos.

Use this discipline:

1. Open the dialog from Add or Edit.
2. Change only the fields needed.
3. Save to persist.
4. Cancel to close without applying changes.
5. Watch the grid or dashboard tile update after save.

## Error Handling

If a save fails:

- Check required fields first.
- Check that referenced records still exist.
- Check whether a delete is blocked because another record depends on it.
- Check whether you are in the expected profile.
- Retry after reloading the page if the backend was restarted.

## Responsive Use

At small widths, treat the app as a single-column workflow:

- Use the menu button to navigate.
- Complete one edit at a time.
- Prefer dashboard review after you have enough room to read charts.

At desktop widths, use the dashboard and grids for scanning:

- Keep the dashboard open during the day.
- Use grid pagination and row actions for batch maintenance.
- Use review mode on a wider screen when comparing chart movement over time.

