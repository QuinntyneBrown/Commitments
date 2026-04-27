# 08 — Typography consistency

**Status: ACCEPTED**

**Design** uses Inter for everything. **Implementation** mixes:
- `styles.scss:11` — `font-family: Inter, Roboto, 'Helvetica Neue', sans-serif` ✓.
- `primary-header.component.scss:32` — `font: 500 28px/1.2 Roboto, Arial, sans-serif` ✗ (Roboto first).
- Material theme defaults (`mat.m2-define-typography-config()` in `theme.scss`) use Roboto, which leaks into mat-toolbar / mat-button / mat-button-toggle / mat-card text everywhere.

## Tasks
- [ ] **8.1** Set `Inter` first in `PrimaryHeaderComponent` styles.
- [ ] **8.2** Override the Material typography config in `theme.scss` to use Inter, e.g. `mat.m2-define-typography-config($font-family: 'Inter')`. Verify all mat-* components inherit the right family afterwards.
- [ ] **8.3** Verify `Mode-Toggle` segments render as Inter / 12 / 500 / letterSpacing 0.6 / uppercase to match `A1yim`/`WFYHQ` (current SCSS sets 12 / 600 / 0.6 / uppercase — change weight 600 → 500).
- [ ] **8.4** Make sure `Inter` is loaded in `index.html` as a webfont — the project currently relies on the system-installed `Inter`, which silently falls back to Roboto on machines without it. Add a `<link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;700&display=swap" />` (or self-host).
- [ ] **8.5** Make sure `Material Symbols Rounded` is loaded — design uses it throughout (current Material setup loads `Material Icons` only).

See [_reference.md](_reference.md) for tokens and node IDs.
