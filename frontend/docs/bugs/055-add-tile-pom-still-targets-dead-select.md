# 055 — `addTile()` and `tileOptions` still target the deleted select-dropdown

## Status

OPEN.

## Symptom

Two `dashboard.spec.ts` tests fail:

- L79 — `loads the dashboard with registered plugin tiles`:
  ```
  Locator: getByTestId('tile-select').locator('option')
  Error: locator(...) resolved to 0 elements
  ```
- L101 — `adds a plugin tile and persists the dashboard layout`:
  ```
  await dashboard.addTile('Monthly Progress');
  // -> tileSelect.selectOption(...) hangs; tile-select doesn't exist.
  ```

## Root cause

`DashboardPage` still models the old "pick from a `<select>` then click an Add
button" interaction:

```ts
this.tileSelect = page.getByTestId('tile-select');
this.tileOptions = this.tileSelect.locator('option');
…
async addTile(displayName: string) {
  await this.tileSelect.selectOption({ label: displayName });
  await this.addTileButton.click();
}
```

Neither testid renders any more. The current shell opens a MatDialog
(`AddTileDialogComponent`) when the FAB is clicked, with cells per tile
(`data-testid="add-tile-cell-<tileId>"`, label inside `.add-tile-dialog__label`)
and confirm/cancel buttons (`add-tile-dialog-confirm`, `add-tile-dialog-cancel`).

## Fix

Replace `tileSelect` / `tileOptions` with locators against the dialog, and
rewrite `addTile(displayName)` to:

1. Click the FAB.
2. Click the dialog cell whose label matches `displayName`.
3. Click confirm.

Then update L79 to open the dialog before reading the catalog labels.

## Resolution

- [ ] Failing tests verified pre-fix.
- [ ] POM rewired; spec L79 opens dialog.
- [ ] Tests verified passing post-fix.
