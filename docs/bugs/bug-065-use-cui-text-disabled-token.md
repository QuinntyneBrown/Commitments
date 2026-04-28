---
id: bug-065
title: `metric-header__sub-caption` and `monthly-progress.bar-labels` use literal `#666666` instead of the existing `--cui-text-disabled` token
status: Open
---

# Bug 065 — Replace literal #666666 with `--cui-text-disabled` token

**Status**: Open

## Description

`commitments-ui/src/lib/tokens/_tokens.scss` already declares the
muted text colour as a CSS custom property:

```scss
--cui-text-disabled: #666666;
```

Two consumers use the literal hex instead of the token:

- `metric-header.component.scss` (added in bug-032 for the
  `__sub-caption` rule):
  ```scss
  .metric-header__sub-caption {
    color: #666666;
    font-size: 11px;
  }
  ```
- `monthly-progress-tile.component.scss` (added in bug-024 for
  `.bar-labels`):
  ```scss
  .bar-labels {
    …
    color: #666666;
    …
  }
  ```

Both bug-024 and bug-032 audited "literal vs token" and deferred
to "promote when a second consumer adopts the same value". With
both rules now stable, plus the existing `delta-badge--neutral`
already using `var(--cui-text-disabled)`, the third consumer
threshold is met — promote both to the token.

## Affected files

- `frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.scss`
- `frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss`

## Reproduction

```bash
grep -n '#666666' frontend/projects/commitments-ui/src/lib/metric-header/metric-header.component.scss \
                  frontend/projects/commitments-dashboard-plugin/src/lib/tiles/monthly-progress-tile/monthly-progress-tile.component.scss
```

Two literal references.

## Expected

- `.metric-header__sub-caption { color: var(--cui-text-disabled, #666666); }`
- `.bar-labels { color: var(--cui-text-disabled, #666666); }` (with the literal as fallback for environments that don't load the token sheet).

## Verification

- Unit: source-level SCSS spec — `metric-header__sub-caption`
  rule references `--cui-text-disabled`; `bar-labels` rule
  references `--cui-text-disabled`.
- Visual: no change (the variable resolves to the same `#666666`).
- All 22 affected suites continue to pass.
