# Bug 005 — Login card shadow token doesn't match design spec

**Status**: Fixed

## Description

The login card's box-shadow renders as `rgba(0, 0, 0, 0.7) 0px 8px 24px 0px` but the design spec requires `rgba(0, 0, 0, 0.6) 0px 6px 12px`.

## Root Cause

`--cui-shadow-login-card` in `_tokens.scss` is set to `0 8px 24px #000000B3` (`#B3` = 70% opacity, spread = 24px). The design calls for `0 6px 12px rgba(0, 0, 0, 0.6)` (60% opacity, 12px blur).

## Affected file

`frontend/projects/commitments-ui/src/lib/tokens/_tokens.scss`

## Fix

Change `--cui-shadow-login-card: 0 8px 24px #000000B3;` to `--cui-shadow-login-card: 0 6px 12px rgba(0, 0, 0, 0.6);`.
