# 057 — Login card sits ~24px below the viewport vertical center

## Status

OPEN — flow: `docs/flows/authentication/README.md` step 1.

## Symptom

`login.spec.ts:29` — `card is vertically centered in the viewport`:

```
Expected: < 20
Received:   23.9921875
```

At lg-desktop (1280×800) the card's vertical center is at ~424px while the
viewport center is 400px — off by exactly the `:host` `padding-top` (24px).

## Root cause

`login-page.component.scss`:

```scss
:host {
  display: flex;
  width: 100%;
  min-height: 100vh;
  align-items: center;
  justify-content: center;
  padding: 24px;
  background: var(--cui-bg);
}
```

With the browser default `box-sizing: content-box`, `min-height: 100vh`
applies to the **content area**, so the host's actual rendered height is
`100vh + 48px` = 848px. `align-items: center` then centers the card within
the 800px content area (top = 24 + 376 = 400 + 24 padding offset = 424).

## Fix

Add `box-sizing: border-box` to `:host`. With border-box, `min-height: 100vh`
includes the padding — the host renders at 800px, the content area is 752px,
and the card center lands at the true viewport center (400px).

## Resolution

- [ ] Failing test verified pre-fix.
- [ ] `:host` updated to `box-sizing: border-box`.
- [ ] Test verified passing post-fix.
