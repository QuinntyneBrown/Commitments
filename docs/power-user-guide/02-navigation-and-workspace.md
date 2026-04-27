# Navigation and Workspace

The Commitments workspace is organized around a persistent application shell and a small set of recurring interaction patterns.

## Application Shell

The authenticated shell contains:

- Top toolbar with the Commitments title.
- Menu button for opening the side navigation.
- Active profile name.
- Profile avatar.
- Side navigation with the primary destinations.
- Main content area for the selected page.

On narrower screens, the side navigation can be opened with the menu button and closes after selecting a destination.

## Primary Navigation

Use the side navigation for major context changes:

| Item | What you do there |
|---|---|
| Dashboard | Monitor live metrics, switch to review mode, adjust dashboard layout |
| Activities | Record and inspect behaviour activity |
| Behaviours | Maintain the behaviour catalog |
| Behaviour Types | Maintain behaviour categories |
| Commitments | Create and edit expectations |
| Cards | Manage dashboard card definitions |
| Card Layouts | Manage dashboard card layout definitions |
| Frequencies | Maintain frequency rules |
| Notes | Create or edit rich-text notes |
| Profiles | Manage profiles |
| To Do's | Create, complete, edit, and delete tasks |
| Logout | Clear the session and return to login |

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

