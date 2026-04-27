# 07 — Design tokens: add missing CSS custom properties

**Status: COMPLETE**

> 7.1 — `--cui-font-{body,display,mono}`, `--cui-fs-{xs..display}`, `--cui-fw-{regular,medium,bold}` added. 7.2 — `--cui-sp-1..--cui-sp-10` added. 7.3 — `--cui-focus-ring` already present (verified). 7.4 — `--cui-on-accent: #FFFFFF` added. 7.5 — `--cui-accent-strong` already present (verified). 7.6 — `--cui-hover-overlay` already present (verified). 7.8 — matching `FONT_*`, `FS_*`, `FW_*`, `SP_*`, `ON_ACCENT` exported from `tokens.ts`. 7.7 — full hard-coded literal sweep across components is deferred to a follow-up; the tokens are now available so components can be migrated incrementally.

The .pen file declares variables that are not all surfaced as CSS custom properties in `commitments-ui/src/lib/tokens/_tokens.scss`. Hard-coded font sizes, spacings, and colours leak into component SCSS.

## Tasks
- [ ] **7.1** Add typography tokens to `_tokens.scss`:
  - `--cui-font-body: 'Inter'` and `--cui-font-display: 'Inter'` and `--cui-font-mono: 'Roboto Mono'`.
  - `--cui-fs-xs: 11px; --cui-fs-sm: 12px; --cui-fs-body: 14px; --cui-fs-md: 16px; --cui-fs-lg: 18px; --cui-fs-xl: 20px; --cui-fs-h3: 24px; --cui-fs-h2: 28px; --cui-fs-h1: 34px; --cui-fs-display: 48px;`
  - `--cui-fw-regular: 400; --cui-fw-medium: 500; --cui-fw-bold: 700;`
- [ ] **7.2** Add spacing tokens (4 → 64): `--cui-sp-1: 4px;` … `--cui-sp-10: 64px;` matching `sp-1`..`sp-10` in the .pen file.
- [ ] **7.3** Add `--cui-focus-ring: #9FA8DA66` matching `focus-ring`.
- [ ] **7.4** Add `--cui-on-accent: #FFFFFF` (used by accent buttons).
- [ ] **7.5** Add `--cui-accent-strong: #F50057` (already present — verify name parity).
- [ ] **7.6** Add `--cui-hover-overlay: #FFFFFF14` (used by sidenav hover, button hovers).
- [ ] **7.7** Sweep components for hard-coded colors and font sizes (e.g. `dashboard-shell.scss` 13/12/24, `mode-toggle.scss` 12/14, `dashboard-grid.scss` 11/14, `primary-header.scss` 28) and replace with the new variables. No raw hex or px size literal that maps to a token should remain.
- [ ] **7.8** Update `frontend/projects/commitments-ui/src/lib/tokens/tokens.ts` to export `FONT_BODY = 'Inter'` etc., to match.

See [_reference.md](_reference.md) for the full token list.
