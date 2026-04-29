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
| 33 | [Feature Library Host Pattern](33-feature-library-host-pattern/README.md) | Accepted | Shared kit: `WindowFeatureBridgeService`, `provideMockDashboardFramework()`, `playwright.<feature>-host.config.ts` template, POM base — owned once, applied by 34 — 41 |
| 34 | [Identity Feature Library](34-identity-feature-library/README.md) | Draft | Lift Login + My Profile + Profiles into `@commitments/identity-feature` + host on :4310 (L2-001..L2-004) |
| 35 | [Behaviours Feature Library](35-behaviours-feature-library/README.md) | Draft | Lift Behaviour Types + Behaviours into `@commitments/behaviours-feature` + host on :4320 (L2-005, L2-006) |
| 36 | [Frequencies Feature Library](36-frequencies-feature-library/README.md) | Draft | Lift Frequencies + Edit Frequency into `@commitments/frequencies-feature` + host on :4330 (L2-007, L2-008) |
| 37 | [Commitments Feature Library](37-commitments-feature-library/README.md) | Draft | Lift Commitments page into `@commitments/commitments-feature` + host on :4340 (L2-009..L2-011) |
| 38 | [Tracking Feature Library](38-tracking-feature-library/README.md) | Draft | Lift Activities + To-Dos into `@commitments/tracking-feature` + host on :4350 (L2-012, L2-016) |
| 39 | [Notes Feature Library](39-notes-feature-library/README.md) | Draft | Lift Notes + Edit Note + Tags + Notes-by-Tag into `@commitments/notes-feature` + host on :4360 (L2-014, L2-015, L2-041) |
| 40 | [Cards Feature Library](40-cards-feature-library/README.md) | Draft | Lift Cards + Card Layouts into `@commitments/cards-feature` + host on :4370 (L2-019, L2-022) |
| 41 | [Settings Feature Library](41-settings-feature-library/README.md) | Draft | Lift Settings into `@commitments/settings-feature` + host on :4380 (L2-035, L2-037) |

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

## Feature library extraction (slices 33 — 41)

Slices 33 — 41 form a self-contained arc that lifts every page out of `commitments-app` into a per-bounded-context Angular library, each with its own `commitments-<feature>-feature-host` Angular SPA driven by Playwright (POM specs). Source intent: keep each library radically simple; route all backend transport through `dashboard-framework`'s `DashboardBackendService`; mock that service in the host with a recording mock that pushes calls into a `window.__featureHarness` bridge so Playwright can assert the library uses the framework boundary correctly.

The pattern is the same one slices 25 — 32 prove for `commitments-dashboard-plugin-host` — but for **pages with routes** instead of dashboard tiles. Slice 33 owns the shared kit (bridge service, mock provider factory, Playwright config template, POM base class). Slices 34 — 41 each apply the kit to one bounded context.

Order:

1. **33 Feature Library Host Pattern** — pipeline foundation; ships `WindowFeatureBridgeService`, `provideMockDashboardFramework()`, the Playwright config template, and the boundary spec template. No feature is migrated.
2. **34 Identity** (Login + Profiles + My Profile) — must land before any other host can have a "logged-in" fixture profile to point at.
3. **35 Behaviours** → **36 Frequencies** — catalog libs that 37 / 38 reuse as peer typed deps.
4. **37 Commitments** → **38 Tracking** — core CRUD libs that depend on 35 + 36.
5. **39 Notes** — largest standalone lib (Note ⟷ Tag domain).
6. **40 Cards** — exports `EditCardDialog` consumed by 37.
7. **41 Settings** — last; depends on 34's `ProfileService` and is the smallest slice (one page, no service).

After slice 41 ships, `commitments-app/src/app/pages/` is empty and `commitments-app/src/app/app.routes.ts` is a flat list of `...identityRoutes, ...behavioursRoutes, ...` imports — every page is exercised in isolation by its host's Playwright suite **and** still composed inside the production app via the dashboard layout. Slice 33's boundary spec runs in every lib's CI, guaranteeing nothing in a lib bypasses `DashboardBackendService`.

## ag-grid → Angular Material migration

Deltas 18 — 24 form a self-contained refactor arc. The build stays green at every step because ag-grid stays installed until 24, and `<app-data-table>` co-exists with `<ag-grid-angular>` in 18 — 23. Order:

1. **18 Mat-Table Foundation** — ship `DataTableComponent` in `commitments-ui` (no consumer flips yet).
2. **19 Simple Catalog Mat-Tables** — flip Profiles → Behaviour Types → Frequencies → Behaviours → Cards → Card Layouts (one PR each in that order).
3. **20 Tracking Mat-Tables** — flip Activities → To-Dos → Commitments.
4. **21 Tags Mat-Table** — flip Tags (special: inline-edit cell pattern).
5. **22 Notes Mat-Table** — flip Notes (special: routerLink cell).
6. **23 Frequencies Editor Mat-Table** — flip the embedded grid in `FrequenciesEditorComponent`.
7. **24 Ag-Grid Removal** — gated on every consumer above being on `<app-data-table>`. Drops the npm deps, the four renderer wrappers, the two CSS imports, and adds the sentinel smoke spec.
