# 09a — Login page detailed audit

Closes the audit half of `09-login-page-audit.md` (task 9.1). Implementation
work to bring the page into alignment is still **open** — see the action
items at the bottom.

Comparison sources
- Design: `Login — S/M/LG/XL` (`YfR0i`, `oC1bM`, `8xz6c`, `rKU7X`) + components
  `Input/Filled` (`noogT`), `Input/Focused` (`AXJ5A`), `Btn/Raised/Primary`
  (`AgzGC`).
- Implementation: `frontend/projects/commitments-app/src/app/pages/login/login-page/login-page.component.{ts,html,scss}`,
  global tokens at `frontend/projects/commitments-ui/src/lib/tokens/_tokens.scss`,
  global Material theme at `frontend/projects/commitments-app/src/styles/theme.scss`.

## Design summary (per breakpoint)

All breakpoints share: full-viewport `$bg` (#121212), card centered both axes,
`$surface` (#1E1E1E) fill, radius **8**, vertical layout, content order
`Commitments` heading → `Login` subtitle → username input → password input →
submit button. Heading uses Inter 700 white; subtitle uses Inter 500
`$text-secondary`. Card shadow is an outer drop shadow at `#000000B3`.

| Breakpoint | Frame | Page padding | Card width | Card padding | Card gap | Heading size | Subtitle size | Shadow (offset / blur) |
|---|---|---|---|---|---|---|---|---|
| S (360)    | `YfR0i` | 24 | `fill_container` (≈312) | 24 | 16 | 28 | 16 | y=8 / 24 |
| M (768)    | `oC1bM` | 48 | 440 | 32 | 20 | 34 | 18 | y=8 / 24 |
| LG (1280)  | `8xz6c` | 64 | 480 | 40 | 24 | 34 | 20 | y=8 / 24 |
| XL (1920)  | `rKU7X` | 80 | 520 | 48 | 28 | 48 | 20 | y=12 / 32 |

Inputs (LG reference, all sizes share styling): `Input/Filled` — fill `$surface-2`
(#242424), radius 4, padding [8,12], 1-px bottom stroke `$divider` (#3A3A3A),
label 12 / regular `$text-secondary`, value 16 / regular `$text-primary`.
Focused state (`Input/Focused`): label 12 / 500 `$primary` (#9FA8DA), bottom
stroke 2 px `$primary`. Submit button is `Btn/Raised/Primary` — fill
`$primary`, label `$on-primary` (#1A1B23) at 14 / 500 with letter-spacing 0.6,
height 36, radius 4, horizontal padding 16, outer shadow y=1 / 3 / `#00000066`.
The label is rendered uppercase ("SUBMIT") in every breakpoint frame.

## Implementation findings

### Page background and centering — ✅
- `:host` is `display: flex; min-height: 100vh; align-items/justify-content: center`.
- Body background is `var(--cui-bg)` = #121212 (matches `$bg`).

### Page padding — ⚠️
- `:host` has no padding. At S (360 px viewport) the card's `width: 100%` fills
  the viewport edge-to-edge, whereas the design reserves a 24-px gutter. Add a
  responsive padding on `:host` (24 → 48 → 64 → 80 across S/M/LG/XL).

### Card geometry — ❌
- `mat-card { max-width: 480px; width: 100%; height: 416px; padding: 40px;
  border-radius: var(--cui-radius-md); background: var(--cui-surface); box-shadow: var(--cui-shadow-floating); }`
- Card width and padding match LG only — no M/XL variants and no S override.
  Need width breakpoints at 320/440/480/520 (or `fill_container` style at S).
- `height: 416px` is hardcoded. The design lets the card grow with content
  (vertical layout, fit-content). Remove the explicit height.
- `--cui-shadow-floating` = `0 6px 12px rgba(0,0,0,0.6)`; design shadow is
  `0 8px 24px #000000B3` (LG) and `0 12px 32px #000000B3` (XL). Either add a
  shadow token that matches `loginCard` or apply the values inline. (Note: this
  shadow mismatch may be a generic token issue worth raising under task 07.)

### Vertical rhythm inside the card — ❌
- Spacing inside the card is currently driven by ad-hoc rules: `h2 { margin-bottom: 30px }`,
  `form { margin: 20px 0 0 0 }`, `button { margin-top: 40px }`, with no rule
  between the two `mat-form-field`s.
- Design uses uniform vertical `gap` of 16/20/24/28 (S/M/LG/XL). Replace
  margin-based spacing with a flex column + `gap`, parameterised per
  breakpoint.

### Heading "Commitments" — ❌
- `h2` uses `font-weight: 600` and inherits Material's default h2 size.
- Design: 28/34/34/48 (S/M/LG/XL), weight **700**, fill `$text-primary`.
- Pin font-size + weight per breakpoint; ensure colour is `var(--cui-text-primary)`.

### Subtitle "Login" — ⚠️
- Rendered via `<mat-card-title>` with `color: var(--cui-text-secondary)`.
- Colour matches. Size/weight inherits Material defaults; design wants
  16/18/20/20 (S/M/LG/XL) at weight 500 in Inter. Pin them.

### Inputs — ❌
- Template uses `<mat-form-field>` + `<input matInput …>` with no `appearance`
  override. The shared theme is built from `mat.m2-define-light-theme(...)` so
  inputs render as a light-mode Material legacy form field on a dark card —
  visually wrong.
- Design's `Input/Filled` is closer to Material `appearance="fill"` with a
  flat 1-px bottom stroke. Two paths:
  1. Add `appearance="fill"` and override the relevant Material CSS variables
     (`--mdc-filled-text-field-container-color: var(--cui-surface-2)`,
     `--mdc-filled-text-field-active-indicator-color: var(--cui-divider)`,
     `--mdc-filled-text-field-focus-active-indicator-color: var(--cui-primary)`,
     label colours → `--cui-text-secondary` / `--cui-primary`, input text →
     `--cui-text-primary`, radius 4 px, vertical padding 8 px).
  2. Replace `<mat-form-field>` with a custom input-field component that
     mirrors the `Input/Filled` token set.
- Design's labels are static "Username" / "Password" *above* the value; the
  current code passes them as `placeholder` rather than `<mat-label>`, so they
  disappear on focus instead of floating up. Switch to `<mat-label>`.

### Submit button — ❌
- `<button mat-raised-button color="primary">` resolves to Material indigo
  palette (~`#3F51B5`), not `$primary` (#9FA8DA), and the label is rendered as
  "Submit" (not uppercased), with default Material height/radius/letter-spacing.
- To match `Btn/Raised/Primary`:
  - background `var(--cui-primary)`, label `var(--cui-on-primary)` (#1A1B23)
  - font 14 / 500 Inter, `letter-spacing: 0.6px`, `text-transform: uppercase`
  - height 36, radius 4, horizontal padding 16
  - shadow `0 1px 3px rgba(0,0,0,0.4)`
  - keep `width: 100%`
- The translation key in the template ("Submit") should remain — apply
  uppercase via CSS so other locales aren't shouted at unless that's intended;
  if uppercase is part of the design language, document it.

### Responsive behaviour — ❌
- Component currently has no `@media` rules; only `max-width` keeps it bounded.
- Add rules so card width / page padding / card padding / inner gap / heading
  size / subtitle size all step through the four design breakpoints. Suggested
  breakpoints based on the design frame widths: S `<480`, M `480–959`, LG
  `960–1599`, XL `≥1600` (or align with whatever convention 05 settles on).

### Copy — ✅
- Heading "Commitments" and subtitle "Login" match. Submit label "Submit"
  matches modulo casing (see button item).

### Accessibility / behaviour notes (not a design deviation, but worth flagging)
- `usernameNativeElement?.focus()` runs in `ngAfterContentInit` — fine, but
  consider `autofocus` on the username `<input>` for the same effect without
  reaching into the DOM via `ElementRef`.
- The inputs use `placeholder` in lieu of `<mat-label>`; with the styling
  fixes above, switching to `<mat-label>` will also fix the screen-reader
  label.

## Action items (open — to be picked up after design alignment 01–08 lands)

- [ ] **9.1.1** Replace card `height: 416px` with content-driven height; switch
  to `display: flex; flex-direction: column; gap: …` to drive inner spacing.
- [ ] **9.1.2** Add responsive rules for page padding, card width, card
  padding, inner gap, heading size, subtitle size at S/M/LG/XL per the table
  above.
- [ ] **9.1.3** Pin heading and subtitle typography (Inter, 700/500, sizes
  per breakpoint, fills `--cui-text-primary` / `--cui-text-secondary`).
- [ ] **9.1.4** Restyle `<mat-form-field>` to the `Input/Filled` token set
  (or replace with a custom component) and convert `placeholder=` to
  `<mat-label>`.
- [ ] **9.1.5** Restyle the submit button to match `Btn/Raised/Primary`
  (palette, height, radius, letter-spacing, uppercase, shadow).
- [ ] **9.1.6** Replace `--cui-shadow-floating` with — or add — a shadow token
  that matches the `loginCard` design shadow (LG `0 8px 24px #000000B3`,
  XL `0 12px 32px #000000B3`). Coordinate with task 07 if other surfaces use
  the same token.
- [ ] **9.1.7** Visually verify each breakpoint against frames `YfR0i`,
  `oC1bM`, `8xz6c`, `rKU7X` (screenshot or `mcp__pencil__get_screenshot`).

See [_reference.md](_reference.md) for tokens and node IDs.
