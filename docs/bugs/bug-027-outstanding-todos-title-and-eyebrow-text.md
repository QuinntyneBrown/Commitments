---
id: bug-027
title: Outstanding To-Dos — title and eyebrow strings differ from the design
status: Open
---

# Bug 027 — Outstanding To-Dos title + eyebrow text drift

**Status**: Open

## Description

`docs/tiles/outstanding-todos-tile/ui-design.pen` (frame `otTitle`)
shows the title `"Outstanding Todos"` and the eyebrow `"Tasks"`. The
implementation in `outstanding-todos-tile.component.html` instead
sets:

- `title="Outstanding To‑Dos"` — the dash between `To` and `Dos` is
  the Unicode non-breaking hyphen (`U+2011`), and the word is
  hyphenated where the design has none.
- `eyebrow="Queue"` — the design says `"Tasks"`.

The component class name (`OutstandingTodosTileComponent`), tile id
(`commitments.outstanding-todos`), file path, and design frame name
all use `Todos` as a single word. Only the visible title in the
template introduces the non-breaking hyphen.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/outstanding-todos-tile/outstanding-todos-tile.component.html`

## Reproduction

1. Render the Outstanding To-Dos tile.
2. Header reads `Outstanding To‑Dos` over `QUEUE`.
3. Compare to the .pen — header reads `Outstanding Todos` over
   `Tasks`.

## Expected

- `title="Outstanding Todos"`
- `eyebrow="Tasks"`

## Verification

- Unit: source-level template check —
  `outstanding-todos-tile.component.html` declares
  `title="Outstanding Todos"` and `eyebrow="Tasks"`, and contains no
  `U+2011` non-breaking hyphen.
- Visual: screenshot the rendered tile vs the .pen.
