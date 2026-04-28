---
id: bug-179
title: _tokens.scss carries 16 legacy non-prefixed token duplicates that nothing references
status: Fixed
---

# Bug 179 — Drop legacy non-`--cui-`-prefixed token duplicates from `_tokens.scss`

**Status**: Fixed

## Fix

16 dead CSS custom property declarations removed from
`_tokens.scss`. The token file shrinks from 93 → 79 lines.
379/379 workspace tests green.

## Description

`_tokens.scss` declares CSS custom properties twice for many
design-system values — once with the legacy non-prefixed
form (`--bg-app: #121212`) and once with the canonical
`--cui-` prefix (`--cui-bg: #121212`). Cross-repo grep
confirms **none of the legacy non-prefixed forms are used by
any consumer**. They're dead duplicates.

The 16 dead legacy tokens (each has a `--cui-X` sibling that
IS used):

```
--bg-app           --cui-bg
--bg-toolbar       --cui-toolbar
--bg-sidebar       --cui-sidenav
--surface-tile     --cui-surface-2
--surface-raised   --cui-surface
--divider          --cui-divider
--accent-live      --cui-accent
--accent-review    --cui-primary
--accent-chart     --cui-info
--accent-success   --cui-success
--text-primary     --cui-text-primary
--text-secondary   --cui-text-secondary
--text-muted       --cui-text-disabled
--radius-tile      --cui-radius-lg
--space-tile-pad   (no equivalent — value 20px is referenced
                   only via the unrelated `--cui-sp-5: 20px`
                   token, never directly)
--shadow-tile      --cui-shadow-raised (similar concept)
```

Drop the legacy declarations. `_tokens.scss` shrinks; nothing
else changes.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tokens/_tokens.scss`

## Reproduction

```bash
grep -rn 'var(--bg-app\|var(--bg-toolbar\|var(--surface-tile\|var(--text-primary\)\|var(--accent-' frontend/projects --include='*.scss'
```

Returns no matches outside `_tokens.scss` itself.

## Expected

The legacy non-prefixed declarations are removed. A
regression-guard spec asserts `_tokens.scss` no longer
declares any of them.

## Verification

- New regression spec confirms removal.
- All other tests continue to pass.
