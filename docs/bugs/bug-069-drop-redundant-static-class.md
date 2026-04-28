---
id: bug-069
title: Four commitments-ui primitives carry a static `class="..."` alongside an equivalent dynamic `[class]="..."` binding — drop the dead-weight static
status: Open
---

# Bug 069 — Drop redundant static class on four primitives

**Status**: Open

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
