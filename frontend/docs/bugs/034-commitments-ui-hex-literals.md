---
id: 034
title: commitments-ui (icon-button + primary-header) hard-codes hex/rgb literals for colours that have --cui-* tokens
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 7ad98c7
flow: dashboard-layout
severity: low
---

# commitments-ui (icon-button + primary-header) hard-codes hex/rgb literals for colours that have --cui-* tokens

## Summary

Two shared UI components leak raw colour literals despite the
design palette already declaring matching tokens.

`icon-button.component.scss` — three `#FFFFFF` literals on
hover, pressed and focus-visible states. The default colour
already uses `var(--cui-text-secondary, #B0B0B0)`; the
hover/pressed/focus brighter state should join the same token
family as `var(--cui-text-primary)`.

`primary-header.component.scss` — `color: #fff` for the heading
and `background-color: rgb(63, 81, 181)` for the bar (the
indigo `--cui-primary-dim` from `theme.scss`).

These are the last raw literals in `commitments-ui/src` per
`grep -rn` (excluding `var(--token, #fallback)` fallback pairs
which are intentional).

## Reproduction

`grep -rn "color: #\|background-color: rgb(" projects/commitments-ui/src`
matches:
- `icon-button.component.scss:10, 14, 25`
- `primary-header.component.scss:3, 5`

## Fix outline (radically simple)

Five literal-to-token swaps across two files:

  - `icon-button.component.scss` — replace every standalone `#FFFFFF`
    on hover/pressed/focus rules with `var(--cui-text-primary)`.
  - `primary-header.component.scss` — `color: var(--cui-text-primary);`
    and `background-color: var(--cui-primary-dim);`.

The `rgba(255, 255, 255, 0.08)` / `0.12)` translucency layers on
`.icon-button:hover` / `.icon-button--pressed` are intentionally
left as literals — same translucency-over-surface pattern noted
in bugs 030–032.

## Tests to add (failing first)

Visual change is limited to the icon-button hover/focus states and
the primary header (which is registered in the lib but currently
not mounted in any rendered route at the default layout). The
swap is value-preserving for the white literals (token resolves
to `#FFFFFF`) and brings the `rgb(63, 81, 181)` onto the
`--cui-primary-dim` (`#3F51B5`) token. No e2e regression net
needed — bug log + fix in one commit pair.
