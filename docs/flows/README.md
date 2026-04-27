# Flows

Each subfolder describes one major user-facing sequence in the Commitments app. The descriptions are written so a developer (or an LLM) can translate them directly into a Playwright e2e test.

## Conventions

Each `README.md` in a flow folder has the same shape:

1. **Summary** — one paragraph: what the user is trying to accomplish.
2. **Surface area** — pages, routes, modules, and integration events touched.
3. **Preconditions** — what state must exist before the flow can be exercised.
4. **Steps** — numbered user actions with the exact UI affordance to interact with and the assertion that proves the step worked.
5. **Selectors** — every `data-testid`, role, label, or placeholder a Playwright author can lean on. If a stable selector does not exist, that is called out explicitly so the author can either add one or fall back to a less-stable selector.
6. **Edge cases** — variations a thorough test should cover.

## Selector philosophy

- Prefer `data-testid` (already pervasive on the dashboard shell and tiles).
- For pages that have no test ids (most catalog/CRUD pages still on `ag-grid` + Material), prefer accessible labels (`getByRole`, `getByLabel`) and visible text. Authors should add a `data-testid` whenever a flow needs one and update the matching flow doc.

## Index

| Flow | Folder |
| --- | --- |
| Authentication (sign in / sign out) | [`authentication/`](authentication/README.md) |
| Profile switching | [`profile-switching/`](profile-switching/README.md) |
| Application shell & navigation | [`application-shell-navigation/`](application-shell-navigation/README.md) |
| Behaviour catalog management | [`behaviour-management/`](behaviour-management/README.md) |
| Frequency catalog management | [`frequency-management/`](frequency-management/README.md) |
| Commitment tracking | [`commitment-tracking/`](commitment-tracking/README.md) |
| Activity recording | [`activity-recording/`](activity-recording/README.md) |
| Note management | [`note-management/`](note-management/README.md) |
| To-Do management | [`todo-management/`](todo-management/README.md) |
| Tag management | [`tag-management/`](tag-management/README.md) |
| Dashboard layout | [`dashboard-layout/`](dashboard-layout/README.md) |
| Dashboard modes (live / review) | [`dashboard-modes/`](dashboard-modes/README.md) |
| Consistency Trend chart | [`consistency-trend-chart/`](consistency-trend-chart/README.md) |
