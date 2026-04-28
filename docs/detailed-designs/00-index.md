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
| 04 | [Settings](04-settings/README.md) | Draft | Read-only profile/version panel — no new entity (L2-035, L2-037) |
| 05 | [Behaviour Types](05-behaviour-types/README.md) | Draft | Wire route + ATDD on existing CRUD; add referential-integrity check (L2-006) |
| 06 | [Behaviours](06-behaviours/README.md) | Draft | Wire route + ATDD; reject delete when referenced by commitment (L2-005) |
| 07 | [Frequencies](07-frequencies/README.md) | Draft | Wire route + ATDD; render `IsDesirable` warn chip (L2-007) |
| 08 | [Edit Frequency](08-edit-frequency/README.md) | Draft | Promote dialog to dedicated route for deep-link from frequency editor (L2-007) |
| 09 | [Commitments](09-commitments/README.md) | Draft | Wire route + 5-row pagination + ATDD on composition dialog (L2-009..L2-011) |
| 10 | [Activities](10-activities/README.md) | Draft | Wire route + ATDD on Add Activity dialog with `PerformedOn` defaulting to now (L2-012) |
| 11 | [To-Dos](11-to-dos/README.md) | Draft | Add full CRUD module — currently only outstanding-count exists (L2-016) |
| 12 | [Notes](12-notes/README.md) | Draft | Add `Note` aggregate, controller, list endpoints — entirely missing backend-side (L2-014) |
| 13 | [Edit Note](13-edit-note/README.md) | Draft | Add slug-based load + Quill save round-trip + sanitisation (L2-014, L2-041) |
| 14 | [Tags](14-tags/README.md) | Draft | Add `Tag` aggregate + controller — currently missing backend-side (L2-015) |
| 15 | [Notes by Tag](15-notes-by-tag/README.md) | Draft | Add `Note ⟷ Tag` join + `GET /api/notes/tag/{slug}` (L2-015) |
| 16 | [Cards](16-cards/README.md) | Draft | Wire catalog page to existing `CardController` with edit dialog (L2-019, L2-022) |
| 17 | [Card Layouts](17-card-layouts/README.md) | Draft | Wire catalog page to existing `CardLayoutController` with edit dialog (L2-022) |

**Status legend:** Draft → In Review → Approved → Implemented.

## Implementation order

The deltas above are independent slices wherever possible, but a sensible order for ATDD work is:

1. **01 Login** + **02 Profiles** + **03 My Profile** — without auth and a `ProfileId` header, no other page can be ATDD-tested end to end.
2. **05 Behaviour Types** → **06 Behaviours** → **07 Frequencies** → **08 Edit Frequency** — catalogs that downstream features depend on.
3. **09 Commitments** → **10 Activities** → **11 To-Dos** — the core tracking surfaces.
4. **12 Notes** → **13 Edit Note** → **14 Tags** → **15 Notes by Tag** — the notes module (largest backend delta).
5. **16 Cards** → **17 Card Layouts** — dashboard catalog management.
6. **04 Settings** — last; depends on every other page being routable so it can deep-link to them.
