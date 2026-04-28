---
id: bug-069
title: Four commitments-ui primitives carry a static `class="..."` alongside an equivalent dynamic `[class]="..."` binding — drop the dead-weight static
status: Fixed
---

# Bug 069 — Drop redundant static class on four primitives

**Status**: Fixed

## Fix

Removes the redundant `class="<base>"` static attribute from four
templates whose dynamic `[class]` binding already emits the same
base class on every CD cycle:

- `delta-badge.component.html` (mat-chip)
- `snackbar.component.html` (section)
- `status-pill.component.html` (mat-chip)
- `dashboard-tile.component.html` (mat-card)

Same idiom as bug-063 (tile-shell) and bug-068 (metric-header):
the dynamic binding is the single source of truth for class.

Coverage:
- Two new specs (status-pill, delta-badge) assert the templates
  no longer carry the split form. The two demo primitives
  (snackbar, dashboard-tile) get the same fix without dedicated
  specs since they aren't consumed by the dashboard plugin or
  app.
- All 22 affected suites pass (159/159 — was 157/157 before).

## Description

Four commitments-ui primitives declare a static `class="..."`
attribute that exactly duplicates the base class their dynamic
`[class]` binding already emits:

| File                                            | Static               | Dynamic                                          |
| ----------------------------------------------- | -------------------- | ------------------------------------------------ |
| `delta-badge.component.html`    (mat-chip)      | `class="delta-badge"`    | `[class]="'delta-badge delta-badge--' + tone()"`    |
| `snackbar.component.html`       (section)       | `class="snackbar"`       | `[class]="'snackbar snackbar--' + tone()"`          |
| `status-pill.component.html`    (mat-chip)      | `class="status-pill"`    | `[class]="'status-pill status-pill--' + variant()"` |
| `dashboard-tile.component.html` (mat-card)      | `class="dashboard-tile"` | `[class]="'dashboard-tile dashboard-tile--' + accent()"` |

In each case the dynamic binding produces the base class on every
change-detection cycle, so the static is redundant — not load-
bearing as in bug-068 (`mat-typography` was being silently
dropped), just dead weight.

The fix mirrors bug-063 (tile-shell) and bug-068 (metric-header):
delete the static, leave the dynamic binding as the single source
of truth for class.

## Affected files

- `frontend/projects/commitments-ui/src/lib/delta-badge/delta-badge.component.html`
- `frontend/projects/commitments-ui/src/lib/snackbar/snackbar.component.html`
- `frontend/projects/commitments-ui/src/lib/status-pill/status-pill.component.html`
- `frontend/projects/commitments-ui/src/lib/dashboard-tile/dashboard-tile.component.html`

## Reproduction

```bash
grep -nE 'class="[^"]+"[^>]*\[class\]=' frontend/projects/commitments-ui
```

Four matches.

## Expected

Each template drops its static `class="..."` attribute when the
dynamic `[class]` binding already includes the same base class.

## Verification

- Unit: source-level template specs — none of the four templates
  matches the redundant `class="..." [class]="..."` pattern.
- Visual: identical (the dynamic binding output is unchanged).
- All affected suites continue to pass.
