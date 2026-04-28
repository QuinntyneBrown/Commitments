---
id: bug-171
title: icon-button hover uses literal rgba instead of --cui-hover-overlay token
status: Open
---

# Bug 171 — icon-button hover should reference `--cui-hover-overlay`

## Description

`icon-button.component.scss` line 19:

```scss
.icon-button:hover:not(:disabled) {
  background: rgba(255, 255, 255, 0.08);
  ...
}
```

The design system declares the equivalent value as a CSS
custom property in `_tokens.scss`:

```scss
--cui-hover-overlay: #FFFFFF14;
```

`#FFFFFF14` is white at 8% alpha — the same visual as
`rgba(255, 255, 255, 0.08)`. Using the token keeps the
icon-button's hover state in sync with any future overlay
tweak from the design system.

```scss
.icon-button:hover:not(:disabled) {
  background: var(--cui-hover-overlay, rgba(255, 255, 255, 0.08));
  ...
}
```

The pressed state on line 34 (`rgba(255, 255, 255, 0.12)`)
has no corresponding `--cui-pressed-overlay` token, so it
remains a literal — outside this bug's scope.

## Affected files

- `frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.scss`

## Reproduction

```bash
grep -n 'rgba(255' frontend/projects/commitments-ui/src/lib/icon-button/icon-button.component.scss
```

Returns the hover and pressed lines.

## Expected

The hover line uses `var(--cui-hover-overlay, rgba(255, 255, 255, 0.08))`.
A regression-guard spec asserts the token reference is
present.

## Verification

- New regression spec confirms the token is referenced.
- All other icon-button specs continue to pass.
