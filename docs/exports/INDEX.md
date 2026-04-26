# Commitments — UI Design Exports

Source: `docs/ui-design.pen` (open with Pencil to edit). All screens are dark-theme,
mobile-first Angular Material, designed at four breakpoints: **S** 360px,
**M** 768px, **LG** 1280px, **XL** 1920px.

## Design system

| File | Description |
|------|-------------|
| `lPXSZ.png` | Color tokens, typography scale, all reusable Material components |
| `Kkip3.png` | Interactive states gallery (button / FAB / input / snackbar / sidenav states) |
| `bNt3E.png` | All 12 dialog variants on a dimmed backdrop (desktop modal layout) |
| `Inepq.png` | Mobile dialog (full-screen sheet treatment) |

## Pages — by breakpoint

| Page | S (360) | M (768) | LG (1280) | XL (1920) |
|------|---------|---------|-----------|-----------|
| Login              | YfR0i | oC1bM | 8xz6c | rKU7X |
| Dashboard          | NytWS | TJhVe | OxYKj | AHHfN |
| Commitments        | veeVo | g4y1n | LoEfJ | Xf885 |
| Activities         | e8X0x | GNdzf | NjIvH | yLHzb |
| Behaviours         | JwFU1 | 6VQvr | AoRHW | Enxxv |
| Behaviour Types    | grCl3 | 3Fmas | bZ6Ke | LY7Q8 |
| Cards              | vjlOq | zNAGk | B3j0Q | u5AnV |
| Card Layouts       | Uv97L | EyxpP | w5Uic | gZUYf |
| Frequencies        | IIyoV | RvG9f | Py1WO | suspl |
| Profiles           | hZK8t | TYXSz | D5BoP | STz7s |
| Tags               | UjNH4 | 7xzPF | vSOFn | 5gY2O |
| To-Dos             | sm1pF | kVqBq | JX39X | gOrad |
| My Profile         | fTAgD | p1r3S | uWpHH | SP1Cu |
| Settings           | NWfaQ | lbWi2 | OrYaE | kAmkK |
| Edit Note          | 4DHR0 | njo1f | ncGaY | 7qEi0 |
| Edit Frequency     | fWdQN | u8pHa | 8qC93 | 70FCr |
| Notes by Tag       | 2k0we | f6Xe5 | 9rtee | lXQ17 |

## Live & Review mode (new)

| File | Description |
|------|-------------|
| `jl9rS.png`  | Section overview — toggle, scrubber, tile components together |
| `A1yim.png`  | Mode-Toggle (Live active, accent pulse dot) |
| `WFYHQ.png`  | Mode-Toggle (Review active, indigo + history icon) |
| `zNLnf.png`  | Scrubber/Timeline — date label, prev/play/next, jump-to-today, marker on track |
| `nAfUX.png`  | Review-Tile — historical metric with date badge + delta-vs-today indicator |
| `o0BgI.png`  | Real-Time Metric Tile — chart.js style 14-day bar chart, today bar in accent gradient with glow, LIVE badge |
| `9IpBQ.png`  | Line-Chart-Tile — chart.js style smooth line + gradient area fill, 14 data dots, today point glows, axis labels |
| `39pLD.png`  | Dashboard **Live** — LG (1280) — chart tile + standard tiles + FAB |
| `TFRTa.png`  | Dashboard **Review** — LG (1280) — scrubber + 6 review tiles, no FAB |
| `yZqDW.png`  | Dashboard **Live** — XL (1920) |
| `wk1pH.png`  | Dashboard **Review** — XL (1920) |

The mode toggle sits in the page header (top-right). Switching to Review mode swaps:
- Live tiles → Review-Tiles (date badge, delta vs today, primary border)
- Real-time chart hides; scrubber appears at the top of the grid
- FAB hides (read-only history)
- "Jump to today" returns to Live

## Reusable component IDs (in the .pen file)

Buttons: `AgzGC` Raised/Primary · `K0jVB` Raised/Accent · `45CHX` Stroked ·
`CHuxA` Basic · `S0h2c` Warn · `iiDSE` Disabled

Icon buttons / FABs: `vl4SK` Icon · `TEvaI` Icon/Accent · `3j8nV` Icon/Warn ·
`e9DYo` FAB · `o7y10` FAB Mini · `43ADd` FAB Extended

Inputs: `noogT` Filled · `AXJ5A` Focused · `gxr1w` Error · `I6uHn` Disabled

Shell: `xGF9T` Toolbar · `ROIa0` Primary Header · `1WUcZ` Sidenav ·
`ZOXJ0` SidenavItem · `XFQ3C` SidenavItem/Active · `DtboV` SidenavItem/Hover

Surfaces: `IX2FV` Mat-Card · `0UH2Q` Dashboard Tile · `2GdJ8` Table Header ·
`nGMBl` Table Row

Inputs/Feedback: `GR3ut` Chip · `tlI4V` Chip/Selected · `GDVHN` Radio ·
`OhR7i` Radio/Selected · `KZYdg` Snackbar · `USNXD` Snackbar/Error ·
`8LaDm` Snackbar/Success · `XqnnL` Mat-Dialog

Live/Review: `A1yim` Mode-Toggle (Live) · `WFYHQ` Mode-Toggle (Review) ·
`zNLnf` Scrubber · `nAfUX` Review-Tile · `o0BgI` Real-Time Metric Tile (bar) ·
`9IpBQ` Line-Chart-Tile
