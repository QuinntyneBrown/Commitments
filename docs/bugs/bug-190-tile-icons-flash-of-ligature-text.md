---
id: bug-190
title: Tile icons flash literal ligature text on first paint because Material Symbols Rounded is loaded with display=swap
status: Fixed
---

# Bug 190 — Tile icons render literal ligature text until icon font loads

**Status**: Fixed

## Fix

`commitments-dashboard-plugin-host/src/index.html` and
`commitments-app/src/index.html`:

```html
- <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
- <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded:opsz,wght,FILL,GRAD@20..48,400..700,0..1,-50..200&display=swap" rel="stylesheet">
+ <link rel="preconnect" href="https://fonts.googleapis.com">
+ <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
+ <link href="https://fonts.googleapis.com/icon?family=Material+Icons&display=block" rel="stylesheet">
+ <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded:opsz,wght,FILL,GRAD@20..48,400..700,0..1,-50..200&display=block" rel="stylesheet">
```

Inter (the body text font) keeps `display=swap`, which is the
correct choice for text — fallback text is shown immediately so
content isn't invisible during the swap period.

527/527 workspace tests green.

## Description

Running `npm run e2e:host:headed` (or `npm start:host` and visiting
the dashboard plugin host directly), every tile renders broken for
~100–500ms after first paint: the literal ligature trigger text
("checklist", "drag_indicator", "history", "edit", "close",
"trending_up") appears in place of each icon, blowing out the
header layout, before the icon font finishes downloading and the
glyphs swap in.

The cause is the `display=swap` parameter on the Material Symbols
Rounded link in `index.html`:

```html
<link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded:...&display=swap" rel="stylesheet">
```

`font-display: swap` tells the browser to render fallback text
immediately and swap to the loaded font when ready. That is the
correct policy for **text** fonts — users would rather see the
copy in Helvetica for 200ms than stare at a blank rectangle. But
for **icon** fonts, the "fallback text" is the ligature trigger
itself — strings like `"checklist"` and `"drag_indicator"` — which
is visually broken content, not a graceful degradation:

`tile-shell.component.html:7`:
```html
<span class="material-symbols-rounded tile-shell__icon">{{ icon() }}</span>
```

`{{ icon() }}` is a string like `"checklist"`. The Material
Symbols font has a `liga` feature that maps that string to a
glyph. Until the font loads, the browser has no ligature table,
so it renders the raw 9-character string in the user's fallback
sans-serif. The icon span is sized for a 20×20 glyph
(`.tile-shell__icon { width: 20px; height: 20px }`), so the long
fallback string overflows or wraps, breaking the header row's
layout until the swap happens.

The same flash affects every other `material-symbols-rounded`
site: the dashboard layout sidenav, the dashboard grid drag
indicators and close buttons, the dashboard shell edit/done/add
controls, the review scrubber, the add-tile dialog, and the
delta-badge directional arrows.

There are also no `<link rel="preconnect">` hints, so the browser
doesn't open a TCP/TLS connection to `fonts.googleapis.com` and
`fonts.gstatic.com` until the HTML parser reaches the `<link
rel="stylesheet">` tags and then the parsed CSS triggers the woff2
fetch. That serializes ~150ms of avoidable network latency on
cold loads.

## Affected files

- `frontend/projects/commitments-dashboard-plugin-host/src/index.html`
- `frontend/projects/commitments-app/src/index.html`

## Reproduction

1. `cd frontend && npm run e2e:host:headed`
2. Watch any tile harness route load (e.g.
   `/tile/commitments.daily-results`).
3. On the first cold load (no font cache), the tile shell header
   briefly shows the literal ligature text (e.g.
   `"checklist"`) where the icon should be. The header row width
   blows out until the font finishes downloading and the text
   swaps to the glyph.

A targeted grep also confirms the precondition before the fix:

```bash
grep -n 'display=swap' frontend/projects/commitments-dashboard-plugin-host/src/index.html
grep -n 'display=swap' frontend/projects/commitments-app/src/index.html
```

Returns one match each (the Material Symbols Rounded link).

## Expected

Icon glyphs render immediately, or close to immediately, on first
paint — without the literal ligature text appearing first.

The fix is two-sided:

- **`font-display: block`** on the icon font (via the `&display=block`
  URL parameter). The browser renders invisible text during a
  short block period (~3s max) instead of the literal ligature
  string, then paints the glyph as soon as the font arrives. For
  icon fonts this is the right tradeoff: a brief invisible icon is
  preferable to a flash of broken layout, because the fallback
  text is meaningless to the user. (The same change applies to
  the legacy Material Icons URL used by `<mat-icon>`.)
- **`<link rel="preconnect">`** to `fonts.googleapis.com` (CSS
  origin) and `fonts.gstatic.com` (woff2 origin, with
  `crossorigin` because fonts are CORS resources). This opens
  the DNS/TCP/TLS handshake in parallel with HTML parsing,
  shaving the connection setup off the critical path so the font
  arrives sooner and the block period is rarely visible.

Inter is left on `display=swap` because it is a text font: a
short flash of fallback Helvetica is acceptable; invisible body
copy is not.

## Verification

- New regression specs
  (`projects/commitments-dashboard-plugin-host/src/app/index-html-fonts.spec.ts`
  and `projects/commitments-app/src/app/index-html-fonts.spec.ts`)
  assert each index.html: preconnects to both font origins, and
  the Material Symbols Rounded + Material Icons links use
  `display=block` (not `swap`).
- 527/527 workspace tests pass.
