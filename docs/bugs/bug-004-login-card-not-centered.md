# Bug 004 — Login card is not vertically centered in the viewport

**Status**: Fixed

## Description

The login card's vertical center is ~24px below the viewport center. The Playwright test checks that `Math.abs(cardCenter - viewportHeight / 2) < 20` and fails with `received: 23.99`.

## Root Cause

`:host` uses `min-height: 100vh` with `padding: 24px` but defaults to `box-sizing: content-box`. This makes the host element `100vh + 48px` tall (padding is added on top of the min-height content box). Flexbox centers the card within the *element*, not the viewport, so the card center sits at `(100vh + 48px) / 2 ≈ 400 + 24 = 424px` from the top instead of 400px.

## Affected file

`frontend/projects/commitments-app/src/app/pages/login/login-page/login-page.component.scss`

## Fix

Add `box-sizing: border-box` to `:host` so the 24px padding is included in the 100vh height, centering the card exactly within the viewport.
