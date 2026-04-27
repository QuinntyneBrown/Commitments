# 062 — Tablet e2e times out 44/57 at default worker count

## Status

OPEN — `npx playwright test --project=tablet` (no explicit `--workers`).

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

Cap dev workers at 4 — verified passing for the tablet suite, still
parallel enough to be fast on lg/xl. CI keeps `workers: 1`.

```ts
workers: process.env.CI ? 1 : 4,
```

## Resolution

- [ ] Failing run verified pre-fix (44/57 timeouts at default workers).
- [ ] Config updated.
- [ ] Tablet suite verified passing at the new cap.
