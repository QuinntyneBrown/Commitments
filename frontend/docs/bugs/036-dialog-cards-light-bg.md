---
id: 036
title: 16 dialog/card components hard-code "background-color: #fff" on :host - bright white over dark theme
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 006c295
flow: dashboard-layout
severity: medium
---

# 16 dialog/card components hard-code "background-color: #fff" on :host — bright white over dark theme

## Summary

`grep -rl "background-color: #fff" projects/commitments-app/src/app/components`
matches 16 files: every dialog (`add-dashboard-cards-dialog`,
`edit-commitment-dialog`, `edit-frequency-dialog`, …, plus
`quill-text-editor`) and two dashboard-card variants
(`daily-results-dashboard-card`, `dashboard-card`,
`relations-results-dashboard-card`).

Each declares `:host { background-color: #fff }`. None of them
is currently triggered by the rendered routes, so the bug is
**dormant** — but the second any of these dialogs opens (or any
of the legacy `dashboard-card` components mounts) over the dark
`--cui-bg`, the contents will flash bright white against the
surrounding dark surface.

## Reproduction

`grep -rln "background-color: #fff" projects/commitments-app/src/app/components`
returns 16 files; visually nothing today because no route
mounts them.

## Fix outline (radically simple)

Sweep `replace_all` across the 16 files: change
`background-color: #fff;` → `background-color: var(--cui-surface);`.
Each file has exactly one occurrence of the literal so a single
`sed` pass is safe. No HTML / TS change.

## Tests to add (failing first)

The dialogs aren't currently mounted by any route - no e2e
regression net is reachable without first wiring up the
dialog-trigger flows. Treat the swap as a
preventative palette migration: bug log + sweep + verify the
existing tile / login suites stay green.
