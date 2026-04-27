---
id: 038
title: styles.scss declares three legacy CSS vars that the design system has superseded
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: 8bfad02
flow: dashboard-layout
severity: low
---

# styles.scss declares three legacy CSS vars that the design system has superseded

## Summary

`projects/commitments-app/src/styles.scss` opens with:

```scss
:root {
  --primary-color: #3f51b5;
  --secondary-color: #3f51b5;
  --default-color: rgb(245, 245, 245);
}
```

The first two have **zero callers** in the workspace
(`grep -rn "var(--primary-color\|var(--secondary-color)"
projects` matches nothing). They're a leftover from before the
`--cui-*` palette landed.

The third (`--default-color`) is used by exactly two files:
`anonymous-master-page.component.scss` and
`add-dashboard-cards-dialog.scss`. Both call sites already
include a hardcoded fallback (`var(--default-color, rgb(241, 241, 241))`
/ `var(--default-color, rgb(245, 245, 245))`), so removing the
declaration here makes those sites resolve to the fallback —
no visible change because neither component is mounted by the
active routes.

## Reproduction

Open `styles.scss` and observe the three `--primary-color` /
`--secondary-color` / `--default-color` declarations next to
the new `var(--cui-bg)` / `var(--cui-text-primary)` body
binding — the file mixes two generations of design tokens.

## Fix outline (radically simple)

Delete the entire `:root { … }` block from `styles.scss`.

  - Active routes consume `--cui-*` tokens via `theme.scss` and
    `commitments-ui/src/lib/tokens/_tokens.scss`; both are
    imported above the `:root` block.
  - Dormant call sites of `--default-color` keep their inline
    `rgb(...)` fallback.

## Tests to add (failing first)

Visual / behavioural change is nil because the rendered routes
don't reference the legacy vars. Bug log + delete + verify the
existing dashboard / login suites stay green.
