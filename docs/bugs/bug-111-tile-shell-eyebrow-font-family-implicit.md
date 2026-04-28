---
id: bug-111
title: tile-shell eyebrow doesn't explicitly set --cui-font-display, relies on body cascade
status: Open
---

# Bug 111 — eyebrow font-family

**Status**: Open

## Description

`tile-shell.component.scss`'s `.tile-shell__title` rule
explicitly references the design font token:

```scss
.tile-shell__title {
  font-family: var(--cui-font-display, 'Inter');
  font-size: 16px;
  font-weight: 500;
}
```

But the eyebrow rule a few lines up declares no `font-family`:

```scss
.tile-shell__eyebrow {
  margin: 2px 0 0;
  color: var(--cui-text-secondary, #B0B0B0);
  font-size: 11px;
  font-weight: 400;
}
```

It currently picks up Inter via the body cascade
(`commitments-app/src/styles.scss` sets the page font), but if
any intermediate component or wrapper changes `font-family`, the
eyebrow drifts away from the design font silently.

Every `ui-design.pen`'s eyebrow text node specifies
`fontFamily: "Inter"` — pinning it explicitly via the
`--cui-font-display` token matches both the title and the
designs.

## Affected files

- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss`
- `frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.spec.ts`

## Reproduction

```bash
grep -A 4 '\.tile-shell__eyebrow' frontend/projects/commitments-ui/src/lib/tile-shell/tile-shell.component.scss
```

The block has no `font-family` declaration.

## Expected

```scss
.tile-shell__eyebrow {
  margin: 2px 0 0;
  color: var(--cui-text-secondary, #B0B0B0);
  font-family: var(--cui-font-display, 'Inter');
  font-size: 11px;
  font-weight: 400;
}
```

## Verification

- Unit (CSS source): `.tile-shell__eyebrow` block contains
  `font-family: var(--cui-font-display`.
- All existing tile-shell specs continue to pass.
