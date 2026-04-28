---
id: bug-066
title: `relations__row` and `focus-list li` use literal `#2A2A2A` instead of the existing `--cui-divider-soft` token
status: Open
---

# Bug 066 — Replace literal #2A2A2A with `--cui-divider-soft` token

**Status**: Fixed

## Fix

`var(--cui-divider-soft, #2A2A2A)` replaces the literal `#2A2A2A`
in both row-divider rules:
- `relations-tile.component.scss` `.relations__row`
- `weekly-focus-tile.component.scss` `.focus-list li`

`#2A2A2A` is preserved as the fallback; the token resolves to the
same value via `_tokens.scss`.

Coverage:
- Two new specs assert each rule references
  `--cui-divider-soft`.
- Existing bug-023 / bug-028 regexes loosened to accept either
  the literal hex or the token form (parallel to bug-062's
  earlier loosening pattern).
- All 22 affected suites pass (154/154 — was 152/152 before).

## Description

`commitments-ui/src/lib/tokens/_tokens.scss` declares:

```scss
--cui-divider-soft: #2A2A2A;
```

Two row-divider rules hard-code the literal:

- `relations-tile.component.scss` (added in bug-028):
  ```scss
  .relations__row {
    …
    border-bottom: 1px solid #2A2A2A;
  }
  ```
- `weekly-focus-tile.component.scss` (added in bug-023):
  ```scss
  .focus-list li {
    …
    border-bottom: 1px solid #2A2A2A;
  }
  ```

Same pattern as bug-065's `--cui-text-disabled` consolidation:
two consumers, an existing token, no design change — promote.

## Affected files

- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/relations-tile/relations-tile.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/weekly-focus-tile/weekly-focus-tile.component.scss`

## Reproduction

```bash
grep -rn '#2A2A2A' frontend/projects/commitments-dashboard-plugin/src/lib/tiles
```

Two literal references.

## Expected

- `.relations__row { border-bottom: 1px solid var(--cui-divider-soft, #2A2A2A); }`
- `.focus-list li { border-bottom: 1px solid var(--cui-divider-soft, #2A2A2A); }`

## Verification

- Unit: source-level SCSS spec for each rule references
  `--cui-divider-soft`.
- Visual: no change.
- All 22 affected suites continue to pass.
