# 062 — Tablet e2e times out 44/57 at default worker count

## Status

FIXED — `workers: 2` in dev verified across all three viewports (57/57 each).

## Symptom

```
44 failed
13 passed (6.3m)
```

Every dashboard.spec.ts test that depends on `await dashboard.goto()`
times out at ~40s. Running the same suite with `--workers=2` or
`--workers=4` passes 26/26.

`lg-desktop` and `xl-desktop` projects pass cleanly at the same default
worker count.

## Root cause

`playwright.config.ts:11` reads:

```ts
workers: process.env.CI ? 1 : undefined,
```

`undefined` lets Playwright pick `cpus / 2` (typically 6+ on dev
machines). The single Angular dev-server (`http://127.0.0.1:4200`) gets
overwhelmed when 6 tablet-viewport browsers initialise, and `goto()`
fails to reach `dashboard-shell` visible state inside the per-test
timeout.

Tablet viewport (768×1024) appears more sensitive than lg/xl because
the layout reflows the topbar / hides the sidenav, so the per-page
boot is slightly heavier.

## Fix

Cap dev workers at 2 — `4` was enough for tablet but xl still
flaked (12/57 timeouts). `2` is the safe ceiling across all three
viewports while staying parallel. CI keeps `workers: 1`.

```ts
workers: process.env.CI ? 1 : 2,
```

## Resolution

- [x] Failing run verified pre-fix (44/57 timeouts at default workers).
- [x] Config updated to `workers: 2` with an inline comment.
- [x] All three viewports verified passing at the new cap (tablet 57/57,
      lg 57/57, xl 57/57).
