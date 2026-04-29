# Detailed Designs — Index

Each entry below is a **small, vertically-sliced delta** that takes the named page from its current state in the repo to a fully working end-to-end implementation (UI route → API → DB → realtime where applicable). Each slice is sized for a single ATDD task, traceable to one or more L1/L2 requirements in `docs/specs/`.

Conventions used by every design document:

- **Delta scope** lists what already exists and what is missing — only the missing parts are designed.
- **Vertical slice** = controller action + MediatR request/handler + FluentValidation validator + DTO + EF mapping + Angular service method + page/dialog wiring + ATDD spec, in that order.
- **CRUD entry surface** is called out per page (table+FAB, dialog, dedicated edit route) so reviewers can confirm the UX before code is cut.
- All API URLs in this index are written as `api/v1.0/<resource>` to reflect the `Asp.Versioning.Mvc` route prefix; the Angular services hit `api/<resource>` and rely on the default-version fallback (per L2-052).

| # | Feature | Status | Delta in one line |
|---|---------|--------|-------------------|
| 01 | [Login](01-login/README.md) | Implemented | Add `POST /api/v1.0/users/token` + bootstrap `ProfileId` after login (L2-001..L2-003) |
| 02 | [Profiles](02-profiles/README.md) | Implemented | Wire route to existing `ProfilesPageComponent`, add `GET /current` and `POST /avatar` (L2-003, L2-004) |
| 03 | [My Profile](03-my-profile/README.md) | Implemented | Self-service avatar + display-name slice on top of `Profile` (L2-004) |
| 04 | [Settings](04-settings/README.md) | Implemented | Read-only profile/version panel — no new entity (L2-035, L2-037) |
| 05 | [Behaviour Types](05-behaviour-types/README.md) | Implemented | Wire route + ATDD on existing CRUD; add referential-integrity check (L2-006) |
| 06 | [Behaviours](06-behaviours/README.md) | Implemented | Wire route + ATDD; reject delete when referenced by commitment (L2-005) |
| 07 | [Frequencies](07-frequencies/README.md) | Implemented | Wire route + ATDD; render `IsDesirable` warn chip (L2-007) |
| 08 | [Edit Frequency](08-edit-frequency/README.md) | Implemented | Promote dialog to dedicated route for deep-link from frequency editor (L2-007) |
| 09 | [Commitments](09-commitments/README.md) | Implemented | Wire route + 5-row pagination + ATDD on composition dialog (L2-009..L2-011) |
| 10 | [Activities](10-activities/README.md) | Implemented | Wire route + ATDD on Add Activity dialog with `PerformedOn` defaulting to now (L2-012) |
| 11 | [To-Dos](11-to-dos/README.md) | Implemented | Add full CRUD module — currently only outstanding-count exists (L2-016) |
| 12 | [Notes](12-notes/README.md) | Implemented | Add `Note` aggregate, controller, list endpoints — entirely missing backend-side (L2-014) |
| 13 | [Edit Note](13-edit-note/README.md) | Implemented | Add slug-based load + Quill save round-trip + sanitisation (L2-014, L2-041) |
| 14 | [Tags](14-tags/README.md) | Implemented | Add `Tag` aggregate + controller — currently missing backend-side (L2-015) |
| 15 | [Notes by Tag](15-notes-by-tag/README.md) | Implemented | Add `Note ⟷ Tag` join + `GET /api/notes/tag/{slug}` (L2-015) |
| 16 | [Cards](16-cards/README.md) | Implemented | Wire catalog page to existing `CardController` with edit dialog (L2-019, L2-022) |
| 17 | [Card Layouts](17-card-layouts/README.md) | Implemented | Wire catalog page to existing `CardLayoutController` with edit dialog (L2-022) |
| 18 | [Mat-Table Foundation](18-mat-table-foundation/README.md) | Draft | Introduce shared `<app-data-table>` in `commitments-ui` + action-cell `<ng-template>` pattern (no L1/L2 surface change) |
| 19 | [Simple Catalog Mat-Tables](19-simple-catalog-mat-tables/README.md) | Draft | Migrate Profiles, Behaviour Types, Behaviours, Frequencies, Cards, Card Layouts pages off ag-grid (L2-003, L2-005, L2-006, L2-007, L2-019, L2-022) |
| 20 | [Tracking Mat-Tables](20-tracking-mat-tables/README.md) | Draft | Migrate Commitments, To-Dos, Activities pages off ag-grid (nested-path + date columns) (L2-009..L2-012, L2-016) |
| 21 | [Tags Mat-Table](21-tags-mat-table/README.md) | Draft | Migrate Tags page; replace ag-grid `editable: true` with `<input matInput>` blur/Enter pattern (L2-015) |
| 22 | [Notes Mat-Table](22-notes-mat-table/README.md) | Draft | Migrate Notes page; replace `onCellClicked` with native `<a [routerLink]>` (L2-014, L2-041) |
| 23 | [Frequencies Editor Mat-Table](23-frequencies-editor-mat-table/README.md) | Draft | Migrate the embedded grid inside `FrequenciesEditorComponent` (L2-007, L2-008) |
| 24 | [Ag-Grid Removal](24-ag-grid-removal/README.md) | Draft | Drop `ag-grid-angular` + `ag-grid-community` deps, delete renderer wrappers + CSS imports, smoke spec — depends on 18-23 |
| 25 | [Plugin Host Playwright Config](25-plugin-host-playwright-config/README.md) | Draft | Add `playwright.host.config.ts` + `e2e:host` scripts + smoke spec for `commitments-dashboard-plugin-host` on :4300 |
| 26 | [Tile Harness Route](26-tile-harness-route/README.md) | Draft | Add `/tile/:tileId` route in host that renders any tile with `TILE_CONTEXT` from query params (no dashboard chrome) |
| 27 | [Window Bridge](27-window-bridge/README.md) | Draft | Expose `window.__pluginHarness` snapshot via `WindowBridgeService` so Playwright can read tileId/mode/asOf and append http/chart records |
| 28 | [HTTP Recorder & Stubs](28-http-recorder-and-stubs/README.md) | Draft | Host `HttpInterceptor` records every outgoing request to bridge; `stubJson()` Playwright helper for fixtures via `page.route` |
| 29 | [Chart Recorder](29-chart-recorder/README.md) | Draft | `CHART_RECORDER` injection token in plugin's `ChartJsLineAdapter`; host writes serialised chart calls to bridge |
| 30 | [Metric-Tile Acceptance Suite](30-metric-tile-acceptance/README.md) | Draft | Per-tile POMs + specs for Daily Results, Weekly Focus, Outstanding Todos, Relations (live mode) |
| 31 | [Chart-Tile Acceptance Suite](31-chart-tile-acceptance/README.md) | Draft | Per-tile POMs + specs for Consistency Trend (and any other chart tiles) — DOM + chart bridge + HTTP assertions |
| 32 | [Review-Mode Acceptance Suite](32-review-mode-acceptance/README.md) | Draft | Per-tile review-mode specs asserting `asOf` propagates to HTTP and chart `pointRadius` highlights the asOf index |

**Status legend:** Draft → In Review → Approved → Implemented.

## Implementation order

The deltas above are independent slices wherever possible, but a sensible order for ATDD work is:

1. **01 Login** + **02 Profiles** + **03 My Profile** — without auth and a `ProfileId` header, no other page can be ATDD-tested end to end.
2. **05 Behaviour Types** → **06 Behaviours** → **07 Frequencies** → **08 Edit Frequency** — catalogs that downstream features depend on.
3. **09 Commitments** → **10 Activities** → **11 To-Dos** — the core tracking surfaces.
4. **12 Notes** → **13 Edit Note** → **14 Tags** → **15 Notes by Tag** — the notes module (largest backend delta).
5. **16 Cards** → **17 Card Layouts** — dashboard catalog management.
6. **04 Settings** — last; depends on every other page being routable so it can deep-link to them.

## Plugin acceptance testing via host (slices 25 — 32)

Slices 25 — 32 form a self-contained arc that stands up Playwright acceptance tests for `commitments-dashboard-plugin` tiles, exercised inside `commitments-dashboard-plugin-host` so the dashboard-framework chrome and the real backend are bypassed. Source intent: [`docs/commitments-dashboard-plugin-acceptance-testing-via-host.md`](../commitments-dashboard-plugin-acceptance-testing-via-host.md) and [`docs/pllug in testing.drawio`](../pllug%20in%20testing.drawio).

Order:

1. **25 Plugin Host Playwright Config** — pipeline foundation; smoke spec proves it boots.
2. **26 Tile Harness Route** — `/tile/:tileId` mounts a single tile bare, providing `TILE_CONTEXT` from URL.
3. **27 Window Bridge** — `window.__pluginHarness` snapshot owned by `WindowBridgeService`.
4. **28 HTTP Recorder & Stubs** — interceptor records outgoing calls; `page.route` stubs responses.
5. **29 Chart Recorder** — `CHART_RECORDER` token in plugin chart adapter; host bridge captures chart.js calls.
6. **30 Metric-Tile Acceptance** — first concrete tile specs (Daily Results, Weekly Focus, Outstanding Todos, Relations).
7. **31 Chart-Tile Acceptance** — Consistency Trend (and any other chart tiles).
8. **32 Review-Mode Acceptance** — every tile re-tested with `mode=review&asOf=...` URL.

Slice 29 contains the **only** plugin-side code change in the arc (~6-line delta to `ChartJsLineAdapter`). Every other change is host-only and additive.

## ag-grid → Angular Material migration

Deltas 18 — 24 form a self-contained refactor arc. The build stays green at every step because ag-grid stays installed until 24, and `<app-data-table>` co-exists with `<ag-grid-angular>` in 18 — 23. Order:

1. **18 Mat-Table Foundation** — ship `DataTableComponent` in `commitments-ui` (no consumer flips yet).
2. **19 Simple Catalog Mat-Tables** — flip Profiles → Behaviour Types → Frequencies → Behaviours → Cards → Card Layouts (one PR each in that order).
3. **20 Tracking Mat-Tables** — flip Activities → To-Dos → Commitments.
4. **21 Tags Mat-Table** — flip Tags (special: inline-edit cell pattern).
5. **22 Notes Mat-Table** — flip Notes (special: routerLink cell).
6. **23 Frequencies Editor Mat-Table** — flip the embedded grid in `FrequenciesEditorComponent`.
7. **24 Ag-Grid Removal** — gated on every consumer above being on `<app-data-table>`. Drops the npm deps, the four renderer wrappers, the two CSS imports, and adds the sentinel smoke spec.
