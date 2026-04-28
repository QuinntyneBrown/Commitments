---
id: bug-059
title: Tile-shell's legacy `status` input + `__status` SCSS rules are dead code after the named-slot migration
status: Fixed
---

# Bug 059 — Remove dead tile-shell `status` input

**Status**: Fixed

## Fix

Three deletions in `tile-shell`:
- `readonly status = input('');` removed from
  `tile-shell.component.ts`.
- `@if (status())` branch removed from
  `tile-shell.component.html`.
- `.tile-shell__status` and
  `.tile-shell--chart .tile-shell__status` rules removed from
  `tile-shell.component.scss`.

Net: -21 lines, -1 input on the public API. All seven plugin
tiles continue to project their pills via `[tile-status]`
ng-content (no behaviour change for any consumer).

Coverage:
- Component-level spec asserts the instance no longer exposes a
  `status` field.
- SCSS-source spec asserts `.tile-shell__status` is gone.
- All 21 affected suites pass (144/144 — was 142/142 before).

## Description

Bug-018 introduced a named projection slot
(`<ng-content select="[tile-status]">`) inside the tile-shell
header. Bugs 057 and 058 then migrated all seven dashboard-plugin
tiles (daily-results, weekly-focus, monthly-progress,
outstanding-todos, relations, consistency-trend, goal-metrics)
to project their `<cui-status-pill>` through that slot.

The pre-existing `status` text input on `TileShellComponent` and
its associated template branch / SCSS rules are now unused —
zero call-sites declare a `status="…"` attribute on
`<commitments-tile-shell>`. The dead code looks like:

```ts
readonly status = input('');
```

```html
@if (status()) {
  <span class="tile-shell__status">{{ status() }}</span>
}
```

```scss
.tile-shell__status { … }
.tile-shell--chart .tile-shell__status { … }
```

Removing the dead input + template branch + two SCSS rules:
- shrinks the component's public API surface by one input
- removes a "two ways to show status" choice (junior dev only
  sees the `[tile-status]` projection pattern)
- drops 12 SCSS lines and 4 template lines

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.ts`
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.html`
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`

## Reproduction

Search the codebase:

```bash
grep -r 'commitments-tile-shell[^>]*\bstatus="' frontend/
# (no match — no consumer declares the input)
```

The component nonetheless still ships the input and its rendering
chrome.

## Expected

- `TileShellComponent` no longer declares `readonly status = input('');`.
- Template no longer contains `@if (status())` or
  `.tile-shell__status` span.
- SCSS no longer declares `.tile-shell__status` or
  `.tile-shell--chart .tile-shell__status` rules.

## Verification

- Unit: source-level checks against the three files asserting the
  identifiers are absent.
- All 21 affected suites continue to pass (no behaviour change
  for any consumer — they all use `[tile-status]` projection).
