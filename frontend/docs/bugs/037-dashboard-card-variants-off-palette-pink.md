---
id: 037
title: poster + to-do dashboard-card variants use Material's #E91E63 pink + #fff white instead of --cui tokens
status: fixed
discovered: 2026-04-27
fixed: 2026-04-27
fixed_in: b1e9b5a
flow: dashboard-layout
severity: low
---

# poster + to-do dashboard-card variants use Material's #E91E63 pink + #fff white instead of --cui tokens

## Summary

Two dashboard-card variants in
`projects/commitments-app/src/app/components/`:

  - `poster-dashboard-card.component.scss`
  - `to-do-dashboard-card.component.scss`

Both declare:

```scss
:host {
  background-color: #E91E63;   /* Material pink */
  color: #fff;
}
```

The colours are off-palette: `#E91E63` is Material's "Pink 500"
while the design's accent is `#FF4081` (`--cui-accent`); `#fff`
already matches `--cui-text-primary`.

Same dormant-but-preventative shape as bug 036 — these two
cards aren't currently rendered by the active routes, but
sweeping them onto the palette removes drift the moment they
mount.

## Reproduction

`grep -rln "background-color: #E91E63" projects/commitments-app/src/app/components`
returns the two files.

## Fix outline (radically simple)

Two literal-to-token swaps per file (4 swaps total):

  - `background-color: #E91E63;` → `background-color: var(--cui-accent);`
  - `color: #fff;`               → `color: var(--cui-text-primary);`

## Tests to add (failing first)

Same shape as bug 036: not currently mounted; treat as
preventative migration. Bug log + sweep + verify the existing
dashboard / login suites stay green.
