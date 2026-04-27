# Reference: Design Tokens and Key Node IDs

## Design tokens (.pen variables)

```
Color:
  bg:              #121212
  surface:         #1E1E1E
  surface-2:       #242424
  surface-3:       #2C2C2C
  surface-4:       #333333
  toolbar:         #1F2233   (header-h: 80, toolbar-h: 64)
  sidenav:         #181A24   (sidenav-w: 250)
  divider:         #3A3A3A
  divider-soft:    #2A2A2A
  primary:         #9FA8DA
  primary-dim:     #3F51B5
  primary-strong:  #7986CB
  accent:          #FF4081
  accent-strong:   #F50057
  on-primary:      #1A1B23
  on-accent:       #FFFFFF
  text-primary:    #FFFFFF
  text-secondary:  #B0B0B0
  text-disabled:   #666666
  text-on-primary: #FFFFFF
  hover-overlay:   #FFFFFF14
  focus-ring:      #9FA8DA66
  info:            #42A5F5
  success:         #66BB6A
  warn:            #F44336
  warning:         #FFA726

Typography:
  font-body:    Inter
  font-display: Inter
  font-mono:    Roboto Mono
  fs-xs/sm/body/md/lg/xl: 11/12/14/16/18/20
  fs-h3/h2/h1/display:    24/28/34/48
  fw-regular/medium/bold: 400/500/700

Spacing:
  sp-1..sp-10: 4, 8, 12, 16, 20, 24, 32, 40, 48, 64

Radius:
  r-sm/md/lg/xl/full: 4, 8, 12, 16, 9999
```

## Key design node IDs

| Concept | ID | Notes |
|---|---|---|
| App-Shell | `8028h` | Toolbar + body (sidenav + content) |
| Mat-Toolbar | `xGF9T` | Hamburger + brand + spacer + profile + avatar |
| Mat-PrimaryHeader | `ROIa0` | h2 page title row, `$primary-dim` background |
| Mat-Sidenav | `1WUcZ` | 250-wide left nav |
| Mat-SidenavItem | `ZOXJ0` | Default state |
| Mat-SidenavItem/Active | `XFQ3C` | 3-px left border, primary fill tint |
| Mat-SidenavItem/Hover | `DtboV` | `$hover-overlay` fill |
| Dashboard-Tile | `0UH2Q` | 320×200 tile body |
| Dashboard-Tile/Editable | `KB9Mx` | 320×200 tile w/ 2px `$accent` stroke + DRAG handle, close button, resize handle |
| FAB/Accent | `e9DYo` | 56×56 pink FAB |
| Mode-Toggle | `A1yim` | Live-active default |
| Mode-Toggle/Review-Active | `WFYHQ` | Review-active variant |
| Add Tile dialog | `AhkGr` | 560×620 modal (tile-grid, inside frame `a2Cjz`) |
| Dashboard — LG (1280) | `OxYKj` | Reference shell+dashboard frame |
| Dashboard Live — LG (1280) | `39pLD` | Live variant w/ Mode-Toggle and FAB |
| Dashboard Review — LG (1280) | `TFRTa` | Review variant (no FAB) |
| Dashboard — LG (1280) — Edit Mode | `fJpM0` | Edit-mode variant: `$accent-strong` PrimaryHeader, EDIT MODE pill, DONE button, editable tiles |
| Dashboard — LG (1280) — Add Tile Dialog | `a2Cjz` | Edit-mode + add-tile-dialog open (backdrop blur 6, `#000000B3`) |
| Login — LG (1280) | `8xz6c` | Login screen reference |
