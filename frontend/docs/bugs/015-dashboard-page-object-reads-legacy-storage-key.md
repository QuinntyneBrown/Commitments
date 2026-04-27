---
id: 015
title: dashboard.page.ts persistedTileCount() reads the legacy 'commitments.dashboard.layout.v1' key but the layout service now writes to 'commitments.layout.live'
status: open
discovered: 2026-04-26
flow: dashboard-layout
severity: medium
---

# dashboard.page.ts persistedTileCount() reads the legacy storage key

## Summary

`projects/commitments-app/e2e/pages/dashboard.page.ts` declares:

```ts
const LAYOUT_STORAGE_KEY = 'commitments.dashboard.layout.v1';
```

…and `persistedTileCount()` reads `localStorage[LAYOUT_STORAGE_KEY]`.

The runtime `LayoutPersistenceService`
(`projects/dashboard-framework/src/lib/dashboard/layout-persistence.service.ts`)
writes to **mode-specific** keys instead:

```ts
LIVE_LAYOUT_STORAGE_KEY   = 'commitments.layout.live'
REVIEW_LAYOUT_STORAGE_KEY = 'commitments.layout.review'
```

So the legacy `commitments.dashboard.layout.v1` key is never
written. `persistedTileCount()` always returns `0`, which makes
the following pre-existing tests fail intermittently or always:

- `dashboard shell › adds a plugin tile and persists the
  dashboard layout`
- `dashboard shell › removes a tile in edit mode`

(The "resets the dashboard to the default plugin layout" and
"toggles edit layout mode" tests don't rely on
`persistedTileCount` and stay green.)

This is **not** introduced by bug-014's fix — verified by
`git stash` of bug-014 and re-running: same failure mode.

## Reproduction

1. `npm run e2e -- --project=lg-desktop --grep "adds a plugin tile"`.
2. Inspect failure message: `Expected: 6, Received: 0`.

After the test runs, in the same browser context:

```js
localStorage.getItem('commitments.dashboard.layout.v1')
// -> null
localStorage.getItem('commitments.layout.live')
// -> '{"schemaVersion":1,"savedAt":...,"items":[…6 entries…]}'
```

## Fix outline (radically simple)

Update `projects/commitments-app/e2e/pages/dashboard.page.ts`:

  - Drop the local `LAYOUT_STORAGE_KEY` const.
  - Import `LIVE_LAYOUT_STORAGE_KEY` from
    `@commitments/dashboard-framework`.
  - Use it in both `goto()` (the addInitScript that clears
    storage in beforeEach) and `persistedTileCount()`.

That keeps the page object and the runtime service single-sourced
on the same key. No production-code change.

## Tests to add (failing first)

The two failing tests above already serve as the failing assertion
— once the page object reads the right key, they pass.
