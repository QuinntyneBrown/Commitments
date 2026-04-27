---
id: bug-012
title: Edit-mode-enter button is icon-only; design specifies labeled stroked button
status: In Progress
---

# Bug 012 — Edit-mode-enter button missing text label and stroke border

**Status**: In Progress

## Description

The `edit-mode-enter` button (pencil icon in the dashboard PrimaryHeader when in live mode)
renders as an icon-only round button.

The design (`ui-design.pen` → `Dashboard — LG (1280)` → `lgHd` → `editLG`) specifies
a small stroked button with:
- `edit` icon (16px) + **"EDIT"** text label
- Semi-transparent white background (`rgba(255,255,255,0.13)`)
- White border (`1px rgba(255,255,255,0.33)`)
- `$r-sm` corner radius (4px)
- Padding `8px 14px`, gap `6px`

## Affected files

- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.html`
- `frontend/projects/dashboard-framework/src/lib/dashboard/dashboard-shell/dashboard-shell.component.scss`
- `frontend/projects/commitments-app/e2e/dashboard.spec.ts` (existing token test needs updating)

## Fix

1. Add `"EDIT"` text label span inside the `edit-mode-enter` button.
2. Update `.dashboard-shell__edit-icon` → `.dashboard-shell__edit` SCSS to use
   semi-transparent white fill, border, and `$r-sm` radius — removing the current
   round icon-button shape.
3. Update the `topbar action buttons` e2e test to assert the new background token value.
