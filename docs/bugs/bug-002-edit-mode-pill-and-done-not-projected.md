# Bug 002 — EDIT MODE pill and DONE button not projected in edit mode

**Status**: Fixed

## Description

When edit mode is entered (clicking `edit-mode-enter`), the "EDIT MODE" pill (`edit-mode-pill`) and the "DONE" button (`edit-mode-done`) are both invisible. The header shows only the "Dashboard" title with a pink background (the `[editMode]` input styling from `PrimaryHeaderComponent`), but no pill or done button appear.

The Angular compiler emits a warning that explains the root cause:

> "Node matches the '[title-adornment]' slot of the 'PrimaryHeaderComponent' component, but will not be projected into the specific slot because the surrounding @if has more than one node at its root."

Both `<ng-container title-adornment>` and `<ng-container header-actions>` live inside the **same** `@if (layoutStore.isEditMode())` block in `dashboard-shell.component.html`. Angular's named-slot content projection fails when multiple projected nodes share a single `@if` root — it cannot route them to their respective slots.

## Affected file

`frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.html`

## Root cause

```html
@if (layoutStore.isEditMode()) {
  <ng-container title-adornment>...</ng-container>   <!-- slot 1 -->
  <ng-container header-actions>...</ng-container>    <!-- slot 2 — kills slot 1 projection -->
}
```

Two content-projection roots inside one `@if` block.

## Fix

Split the `@if` into separate per-slot guards so each `@if` has exactly one root node:

```html
@if (layoutStore.isEditMode()) {
  <ng-container title-adornment>...</ng-container>
}
@if (layoutStore.isEditMode()) {
  <ng-container header-actions>...DONE...</ng-container>
} @else {
  <ng-container header-actions>...mode-toggle + edit-enter...</ng-container>
}
```
