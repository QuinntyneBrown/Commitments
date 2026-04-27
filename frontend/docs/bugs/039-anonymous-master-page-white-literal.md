---
id: 039
title: anonymous-master-page :host hard-codes color #fff - last raw white literal in commitments-app/src
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
flow: dashboard-layout
severity: low
---

# anonymous-master-page :host hard-codes color #fff — last raw white literal in commitments-app/src

## Summary

`anonymous-master-page.component.scss` :host declares
`color: #fff`. The component is dormant (no other file
imports `AnonymousMasterPageComponent` per
`grep -rn "AnonymousMasterPage" projects` — the only match is
the component's own .ts).

After bug 038 deletion this is the last raw `color: #` /
`background: #` literal in
`projects/commitments-app/src/app/components` (excluding
`var(--cui-token, #fallback)` fallback pairs).

## Reproduction

`grep -rn "color: #\|background: #" projects/commitments-app/src \
  | grep -v "var(--"`
returns this single line.

## Fix outline (radically simple)

`color: #fff;` → `color: var(--cui-text-primary);` — done in
the same commit as the bug log so this file is closed in one
sweep.
