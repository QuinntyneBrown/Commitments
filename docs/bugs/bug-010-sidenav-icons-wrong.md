# Bug 010 — Sidenav nav-item icons don't match the design

**Status**: Fixed

## Description

Five sidenav nav items use incorrect Material Symbols icons. The design
(`ui-design.pen` — LG and XL dashboard frames) specifies:

| Label | Design icon | Implemented icon |
|---|---|---|
| Activities | `directions_run` | `event_available` |
| Behaviours | `psychology` | `repeat` |
| Notes | `sticky_note_2` | `description` |
| Profiles | `group` | `person` |
| To Do's | `checklist` | `format_list_bulleted` |

## Affected files

- `frontend/projects/commitments-app/src/app/components/dashboard-layout/dashboard-layout.component.ts`

## Fix

Update the five `icon` values in `navItems` to match the design.
