# Live / Review Dashboard and Chart.js Alignment Plan

This document outlines the implementation work needed to align the dashboard with `docs/ui-design.pen`, with emphasis on live mode, review mode, and Chart.js line charts.

## Source Designs Reviewed

- `docs/ui-design.pen`
  - `jl9rS` - Live & Review Mode components and dashboard section.
  - `A1yim` - Mode toggle, live active.
  - `WFYHQ` - Mode toggle, review active.
  - `zNLnf` - Scrubber / timeline.
  - `nAfUX` - Review tile.
  - `o0BgI` - Real-time metric tile.
  - `9IpBQ` - Line chart tile.
  - `39pLD` / `yZqDW` - Live dashboard at LG / XL.
  - `TFRTa` / `wk1pH` - Review dashboard at LG / XL.
- `docs/exports`
  - Exported PNGs for the same components and dashboard frames.
- `docs/detailed-designs`
  - `01-live-goal-metrics` defines the first live metric tile, current progress endpoint, and SignalR push flow.
  - `02-review-goal-history` defines the first review tile, historical endpoint, date-window state, and scrub flow.
- `docs/specs/L1.md` and `docs/specs/L2.md`
  - Live mode: `L1-011`, `L2-023` through `L2-026`.
  - Review mode: `L1-012`, `L2-027` through `L2-031`.
  - Visual system and responsiveness: `L1-014`, `L1-015`, `L2-034`, `L2-036`.
  - Performance constraints: `L2-043` through `L2-045`.

## Current Architectural Target

The frontend should keep the application as a thin shell and place reusable behavior in libraries:

- `commitments-app` hosts routes, app-level providers, and the dashboard shell only.
- `dashboard-framework` owns mode state, dashboard layout state, Gridster integration, tile registration, plugin activation, and mode-aware tile context.
- `commitments-ui` owns reusable visual primitives derived from `ui-design.pen`.
- `commitments-dashboard-plugin` owns commitment-specific tiles, data adapters, and Chart.js dashboard visualizations.

Chart rendering should use the existing `chart.js` dependency directly unless a wrapper is intentionally introduced later. The final chart tiles should not be static SVG or hand-drawn DOM charts.

## What Needs To Be Implemented

### Design System Alignment

Move the reusable styling implied by `ui-design.pen` into `commitments-ui` and global theme tokens.

Required tokens:

- App background: `#121212`.
- Toolbar: `#1F2233`.
- Sidebar: `#181A24`.
- Tile surface: `#242424`.
- Raised panel / scrubber surface: `#1E1E1E`.
- Divider / tile border: `#3A3A3A`.
- Live accent: `#FF4081`.
- Review accent: `#9FA8DA`.
- Line-chart accent: `#42A5F5`.
- Success delta: `#66BB6A`.
- Primary text: `#FFFFFF`.
- Secondary text: `#B0B0B0`.
- Muted text: `#666666`.

Required reusable UI pieces:

- Mode toggle matching `A1yim` and `WFYHQ`.
- Icon button primitive for scrubber controls.
- Status pill for live/review badges.
- Review scrubber timeline shell matching `zNLnf`.
- Tile shell variants for metric, review, and chart tiles.
- Shared metric header, trend row, and delta badge pieces.

### Dashboard Framework

`dashboard-framework` needs first-class dashboard mode support:

- Define a `DashboardMode` type: `live | review`.
- Keep mode state in the framework, not inside individual tiles.
- Expose mode through tile context so plugins can render live or review variants.
- Support mode-specific tile manifests and Gridster layouts.
- Persist layout by mode, so live and review dashboards do not overwrite each other.
- Hide the add-tile FAB in review mode.
- Render the review scrubber in review mode only.
- Keep `commitments-app` as a small shell that wires providers and renders the framework shell.

### Live Mode

Live mode should match `Dashboard Live - LG` / `Dashboard Live - XL` and use plugin tiles that subscribe to current data.

Required behavior:

- Initial live tile load from `GET /api/goal-progress/current`.
- SignalR push updates when achievements change.
- Live badge styling and update animation.
- Gridster layout with live plugin tiles.
- Add-tile FAB visible.
- Live toggle state active.
- Chart tiles update from the latest live stream or polling/data refresh adapter.

### Review Mode

Review mode should match `Dashboard Review - LG` / `Dashboard Review - XL`.

Required behavior:

- Review toggle active.
- Add-tile FAB hidden.
- Review scrubber visible above the review grid.
- Selected date is shared through dashboard mode context.
- Review tiles fetch historical snapshots for the selected date.
- Scrubbing is debounced to at most 100 ms.
- Historical responses are cacheable by goal/date/window.
- Review tile displays the selected date, snapshot value, and delta versus today.

### Chart.js Line Charts

The line chart tile should implement `9IpBQ` with Chart.js.

Required visual behavior:

- Header: show-chart icon, title `Consistency Trend`, subtitle, and live/review status pill.
- Metric row: large current percentage, trend text, and peak/low summary.
- Plot: dark horizontal grid lines, blue smooth line, gradient area fill, point markers, highlighted current/selected point, and sparse x-axis labels.
- Height: stable tile height around 340 px at desktop sizes.
- Responsive: canvas fills the tile plot area without layout shifts.

Recommended Chart.js configuration:

- `type: 'line'`.
- `responsive: true`.
- `maintainAspectRatio: false`.
- `plugins.legend.display: false`.
- Tooltip styled to match the dark design.
- Category x-axis using preformatted labels.
- Hidden y-axis labels with visible grid lines.
- Dataset:
  - `borderColor: '#42A5F5'`.
  - `borderWidth: 2.5`.
  - `tension: 0.35`.
  - `fill: true`.
  - Canvas gradient from `#42A5F566` to transparent.
  - Normal point radius around 3 px.
  - Current / selected point radius around 7 px with dark border.

Recommended data contract:

```ts
export interface GoalTrendPointDto {
  date: string;
  completed: number;
  target: number;
  percentage: number;
}

export interface GoalTrendDto {
  goalId: string;
  mode: 'live' | 'review';
  asOf: string;
  windowDays: number;
  points: GoalTrendPointDto[];
  currentPercentage: number;
  peakPercentage: number;
  lowPercentage: number;
  deltaLabel: string;
}
```

## Detailed Designs To Add

The existing `01-live-goal-metrics` and `02-review-goal-history` designs are good starting vertical slices. The remaining work should be split into the following small detailed designs.

### 03 - UI Tokens And Shared Components

Entails:

- Extract dark theme tokens from `ui-design.pen`.
- Define `commitments-ui` APIs for mode toggle, status pill, icon button, tile shell, metric header, and delta badge.
- Document component inputs, outputs, keyboard behavior, and disabled states.
- Include screenshots/exports for `A1yim`, `WFYHQ`, `nAfUX`, `o0BgI`, and `9IpBQ`.

Acceptance:

- Storybook or isolated examples render the primitives against the dark theme.
- Components can be consumed by both framework shell and plugin tiles.

### 04 - Dashboard Mode Shell

Entails:

- Add mode state to `dashboard-framework`.
- Render the mode toggle in the dashboard header/content area according to the LG/XL designs.
- Persist selected mode, with live as the default.
- Expose selected mode through tile context.
- Hide review-only controls in live mode and live-only controls in review mode.

Acceptance:

- Toggling between live and review changes mode without route reload.
- Mode is observable by plugin tiles.
- The app shell remains a provider/bootstrap host.

### 05 - Gridster Layouts By Mode

Entails:

- Define live and review Gridster layouts separately.
- Map plugin tiles to mode-specific default positions.
- Ensure live dashboard frames align with `39pLD` and `yZqDW`.
- Ensure review dashboard frames align with `TFRTa` and `wk1pH`.
- Keep persisted live/review layouts isolated.

Acceptance:

- Live layout changes do not mutate the review layout.
- Review mode does not show the add-tile FAB.
- Breakpoints match the existing LG/XL design spacing and the L1/L2 responsive requirements.

### 06 - Review Scrubber Timeline

Entails:

- Implement `zNLnf` as a reusable component in `commitments-ui` or a framework-level review control.
- Add previous, play/pause, next, and jump-to-today controls.
- Add date tick labels and a draggable/range-based track.
- Publish selected date changes into dashboard review context.
- Clamp date selection to valid historical windows.

Acceptance:

- Scrub events update review context with debounce.
- Keyboard interaction works for range movement and buttons.
- The selected date label and highlighted tick update together.

### 07 - Live Real-Time Metric Tile

Entails:

- Extend `01-live-goal-metrics` to match `o0BgI`.
- Use `GoalProgressDto` for current completion data.
- Add a live metric summary, trend row, and 14-day bar visualization.
- Subscribe to SignalR updates.
- Keep the tile as a plugin registered from `commitments-dashboard-plugin`.

Acceptance:

- Initial render works from REST data.
- SignalR updates change the displayed value and live badge state.
- Bar chart data reflects the last 14 days and highlights today.

### 08 - Chart.js Line Chart Tile

Entails:

- Implement `9IpBQ` as a plugin tile using Chart.js.
- Add a chart adapter/service that creates, updates, and destroys the Chart.js instance.
- Add trend data endpoint or reuse an existing trend data source.
- Support live mode current-point highlighting.
- Support review mode selected-date highlighting if the tile is enabled for review.

Acceptance:

- The chart renders as a real canvas-based Chart.js line chart.
- It updates without recreating the whole Angular component.
- The tile remains responsive at Gridster desktop and tablet sizes.
- Tests verify the canvas exists and receives non-empty series data.

### 09 - Review Goal History Tile Polish

Entails:

- Extend `02-review-goal-history` to match `nAfUX`.
- Use selected date from review context.
- Show selected-date badge, historical value, snapshot label, and delta versus today.
- Reuse the shared review/status/delta primitives.
- Keep historical endpoint inputs clamped and cacheable.

Acceptance:

- Scrubbing changes the tile value.
- Delta label handles positive, negative, and equal cases.
- Empty historical days render an explicit zero/snapshot state without layout shifts.

### 10 - Mode-Aware Plugin Contracts

Entails:

- Extend tile registration metadata with supported modes.
- Allow a plugin to provide separate live/review components for the same tile concept.
- Pass mode, selected review date, and refresh hooks through tile context.
- Document how future tiles opt into live, review, or both modes.

Acceptance:

- Framework can filter tile catalog by mode.
- Plugin code does not import application shell types.
- Adding a new mode-aware tile requires only plugin registration and tile implementation.

### 11 - Backend Trend And Snapshot Endpoints

Entails:

- Keep `GET /api/goal-progress/current`.
- Keep `GET /api/goal-progress/at`.
- Add a trend endpoint if no existing endpoint can supply line chart data, for example:
  - `GET /api/goal-progress/trend?goalId={id}&windowDays=14`
  - `GET /api/goal-progress/trend?goalId={id}&asOf={date}&windowDays=14`
- Ensure current, historical, and trend queries use bounded count queries and appropriate indexes.
- Document cache headers for historical and trend requests.

Acceptance:

- Live data is fresh.
- Historical/trend requests are deterministic for the same date window.
- Query count remains bounded for dashboard tile loads.

### 12 - End-To-End And Visual Acceptance

Entails:

- Add Playwright Page Object Model coverage for live/review mode switching.
- Cover scrubber movement, jump-to-today, hidden review FAB, and mode-specific tile rendering.
- Cover Chart.js line chart rendering with a canvas existence and non-empty pixel/data check.
- Add viewport coverage for tablet, LG desktop, and XL desktop.

Acceptance:

- E2E tests pass locally.
- Tests assert behavior, not only text presence.
- Any visual snapshots are tied to the exported design frames and can tolerate dynamic chart data.

## Suggested Implementation Order

1. UI tokens and shared primitives.
2. Dashboard mode shell.
3. Mode-specific Gridster layouts.
4. Review scrubber timeline.
5. Live metric tile visual polish.
6. Chart.js line chart tile.
7. Review tile visual polish.
8. Mode-aware plugin contracts.
9. Backend trend endpoint, if needed.
10. E2E and visual acceptance coverage.

This order keeps each slice vertical enough to validate in the running app while avoiding a large dashboard rewrite before the shared mode and UI contracts are stable.
