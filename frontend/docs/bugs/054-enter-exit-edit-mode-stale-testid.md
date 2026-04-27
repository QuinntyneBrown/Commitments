# 054 — enterEditMode / exitEditMode use the stale `edit-layout` testid

## Status

OPEN.

## Symptom

Three `dashboard.spec.ts` tests time out clicking a button that does not exist:

- L60 — `tile-chrome remove button uses --cui-surface-3 in edit mode`
- L94 — `toggles edit layout mode and exposes tile chrome`
- L113 — `removes a tile in edit mode`

Failure log:

```
locator.click: Test timeout of 30000ms exceeded.
Call log:
  - waiting for getByTestId('edit-layout')
```

## Root cause

`DashboardPage.editLayoutButton = page.getByTestId('edit-layout')` and:

```ts
async enterEditMode() {
  await this.editLayoutButton.click();
  await expect(this.editLayoutButton).toHaveText('Done');
  ...
}
```

The current shell renders **two distinct** buttons depending on layout-store
state (`dashboard-shell.component.html`):

- `data-testid="edit-mode-enter"` — pencil icon when not editing.
- `data-testid="edit-mode-done"` — text "DONE" when editing.

There is no single `edit-layout` toggle anymore.

## Fix

Update `enterEditMode` to click `edit-mode-enter` and assert `edit-mode-done`
becomes visible; `exitEditMode` does the reverse. Drop the `editLayoutButton`
field (only it referenced the dead testid).

## Resolution

- [ ] Failing tests verified pre-fix.
- [ ] POM updated.
- [ ] Tests verified passing post-fix.
