# Backend / Frontend Interface Control Document

**Document ID:** ICD-BFF-001  
**Status:** Draft target contract  
**Last updated:** 2026-04-26  
**Scope:** Commitments Angular frontend, dashboard tile libraries, and .NET backend API/hub interface.

## 1. Purpose

This Interface Control Document defines the contract between the frontend and backend. It covers:

- REST endpoints used by the app shell, dashboard, and tiles.
- SignalR/WebSocket transport and the messages that must be published for live tiles.
- Shared DTO shapes, identifiers, date formats, headers, cache policy, and error handling.
- Known alignment items between the current implementation and the target contract.

The contract is based on `docs/specs/L1.md`, `docs/specs/L2.md`, `docs/detailed-designs/`, and the current frontend/backend source.

## 2. Interface Summary

| Interface | Protocol | Direction | Current state | Purpose |
|---|---|---:|---|---|
| REST API | HTTPS, JSON | Frontend -> Backend | Partially implemented | Commands, CRUD reads, dashboard bootstrap, tile data snapshots |
| SignalR hub | WebSocket preferred, SignalR fallback transports | Backend -> Frontend | Frontend client exists; backend hub still to implement | Live updates and cache invalidation for tiles |
| Tile context | Angular DI/signals | Framework -> Tile | Implemented in `dashboard-framework` | Pass mode, selected review date, refresh hooks, and tile instance operations |
| Local storage | Browser storage | Frontend only | Implemented | Mode, review date, and layout persistence |
| In-process event bus | .NET process memory | Backend module -> Backend module | Implemented | Backend module integration. Not a frontend contract. |

## 3. Global Contract Rules

### 3.1 API base path and versioning

Canonical backend routes shall be versioned:

```text
https://{host}/api/v1.0/{resource}
```

The existing frontend still contains several unversioned plural routes such as `/api/dashboards/currentProfile` and `/api/commitments/daily`. The target contract shall standardize on `/api/v1.0/...`. Transitional aliases may be kept during migration, but new tile endpoints shall use the versioned route.

### 3.2 Encoding and casing

- Request and response bodies use `application/json`.
- JSON field names use camelCase.
- `Guid` values are serialized as strings.
- Date-only values use `YYYY-MM-DD`.
- Instants use ISO 8601 UTC strings, for example `2026-04-26T18:43:02Z`.
- Currency and localized display strings shall not be returned unless the endpoint explicitly owns display formatting, such as `deltaLabel`.

### 3.3 Authentication and profile scoping

Every protected REST request shall include:

```http
Authorization: Bearer {jwt}
ProfileId: {profileGuid}
```

The backend reads `ProfileId` through `IHttpContextAccessor.GetProfileId()`. The frontend shall populate it from `currentProfileIdKey` after login and current-profile bootstrap.

SignalR hub connection shall authenticate using the same JWT. The existing frontend connects to:

```text
{baseUrl}/hub?token={jwt}
```

The backend hub implementation shall accept that query parameter for compatibility. A later frontend refactor may move to SignalR's `accessTokenFactory`, but that is not required for the initial hub implementation.

### 3.4 Common responses

| Status | Meaning |
|---:|---|
| 200 | Request succeeded. Current command handlers return 200 with response DTOs. |
| 400 | Validation or malformed input. Body is `ProblemDetails` or validation problem details. |
| 401 | Missing, expired, or invalid token. |
| 403 | Token user is not allowed to access the supplied `ProfileId`. |
| 404 | Entity not found or not owned by the active profile. |
| 500 | Unexpected server error. |

### 3.5 Cache policy

Live/current data shall not be cached:

```http
Cache-Control: no-store
```

Historical goal-progress snapshots and trends shall be cacheable for fully historical requests:

```http
Cache-Control: public, max-age=300
```

The current backend policy uses this rule when `asOf` is older than one minute. Requests with no `asOf`, or with `asOf` inside the last minute, shall use `no-store`.

## 4. Shared Types

```ts
type Guid = string;
type IsoDate = string;       // YYYY-MM-DD
type IsoDateTime = string;   // ISO 8601 UTC instant
type DashboardMode = 'live' | 'review';
```

### 4.1 Dashboard DTOs

```ts
interface DashboardDto {
  dashboardId: Guid;
  name: string;
  profileId: Guid | null;
  dashboardCards: DashboardCardDto[];
}

interface DashboardCardDto {
  dashboardCardId: Guid;
  dashboardId: Guid;
  cardId: Guid;
  cardLayoutId: Guid;
  card: CardDto;
  cardLayout: CardLayoutDto;
  options: Record<string, unknown>;
}

interface CardDto {
  cardId: Guid;
  name: string;
  description: string;
}

interface CardLayoutDto {
  cardLayoutId: Guid;
  name: string;
  description: string;
}
```

### 4.2 Goal progress DTOs

`goalId` is a frontend/tile term. In the current backend implementation it maps to `CommitmentId`.

```ts
interface GoalProgressDto {
  goalId: Guid;
  target: number;
  count: number;
  asOf: IsoDateTime;
}

interface Last14DayPointDto {
  date: IsoDate;
  completed: number;
  target: number;
  percent: number;
}

interface Last14DayResponse {
  points: Last14DayPointDto[];
}

interface GoalTrendPointDto {
  date: IsoDate;
  completed: number;
  target: number;
  percentage: number;
}

interface GoalTrendDto {
  goalId: Guid;
  mode: DashboardMode;
  asOf: IsoDate;
  windowDays: number;
  points: GoalTrendPointDto[];
  currentPercentage: number;
  peakPercentage: number;
  lowPercentage: number;
  deltaLabel: string;
}
```

## 5. REST API Contract

### 5.1 Authentication and profile bootstrap

| ID | Method | Route | Request | Response | Notes |
|---|---|---|---|---|---|
| REST-AUTH-001 | POST | `/api/users/token` | `{ username, password }` | `{ accessToken }` | Existing frontend route. Produces JWT persisted under `accessTokenKey`. |
| REST-PROFILE-001 | GET | `/api/v1.0/profile/{profileId}` | none | `{ profile }` | Implemented backend route uses singular controller route. |
| REST-PROFILE-002 | GET | `/api/profiles/current` or target `/api/v1.0/profile/current` | none | `{ profile }` | Required by frontend login bootstrap. Backend route shall be aligned. |

### 5.2 Dashboard management

| ID | Method | Route | Purpose |
|---|---|---|---|
| REST-DASH-001 | GET | `/api/v1.0/dashboard/currentProfile` | Load the active profile dashboard in one round trip with all `dashboardCards` and options. |
| REST-DASH-002 | GET | `/api/v1.0/dashboard/{dashboardId}` | Load one dashboard. |
| REST-DASH-003 | POST | `/api/v1.0/dashboard` | Create a dashboard. |
| REST-DASH-004 | PUT | `/api/v1.0/dashboard` | Update a dashboard. |
| REST-DASH-005 | DELETE | `/api/v1.0/dashboard/{dashboardId}` | Delete a dashboard. |
| REST-DASHCARD-001 | POST | `/api/v1.0/dashboardCard` | Add one card to a dashboard. |
| REST-DASHCARD-002 | POST | `/api/v1.0/dashboardCard/range` | Add multiple cards. |
| REST-DASHCARD-003 | PUT | `/api/v1.0/dashboardCard` | Persist card options/configuration. |
| REST-DASHCARD-004 | DELETE | `/api/v1.0/dashboardCard/{dashboardCardId}` | Remove one card from the dashboard. |
| REST-CARD-001 | GET | `/api/v1.0/card` | Read available card catalog records. |
| REST-CARDLAYOUT-001 | GET | `/api/v1.0/cardLayout` | Read available card layout records. |

Dashboard load performance requirement: the dashboard shell shall first call `REST-DASH-001`. Tile data requests may then run independently and in parallel.

### 5.3 Goal-progress tile endpoints

These endpoints are implemented under `backend/src/Modules/Commitments/Controllers/GoalProgressController.cs`.

| ID | Method | Route | Query | Response | Cache |
|---|---|---|---|---|---|
| REST-GOAL-001 | GET | `/api/v1.0/goal-progress/current` | `goalId: Guid` | `GoalProgressDto` | `no-store` |
| REST-GOAL-002 | GET | `/api/v1.0/goal-progress/at` | `goalId: Guid`, `asOf: IsoDateTime` | `GoalProgressDto` | `public, max-age=300` when historical |
| REST-GOAL-003 | GET | `/api/v1.0/goal-progress/last14` | `goalId: Guid` | `Last14DayResponse` | `no-store` |
| REST-GOAL-004 | GET | `/api/v1.0/goal-progress/trend` | `goalId: Guid`, `windowDays?: number`, `asOf?: IsoDateTime` | `GoalTrendDto` | historical rule |

Validation:

- `goalId` must be a non-empty `Guid`.
- `windowDays` shall be clamped to `[1, 365]`.
- `asOf` shall be parsed as ISO 8601. Invalid values return 400.
- Future `asOf` values shall be clamped to server `UtcNow`.
- Goals/commitments outside the active profile return 404.

Tile usage:

| Tile | REST calls |
|---|---|
| Live real-time metric | `current`, `last14` on mount |
| Review goal history | `at` for selected review date, `current` for delta vs today |
| Consistency trend chart | `trend`; live omits `asOf`, review includes selected date |

### 5.4 Target aggregate tile endpoints

The current plugin tiles for Daily Results, Weekly Focus, Monthly Progress, Outstanding To Dos, and Relations contain static markup. To make them data-backed, the backend shall expose the following target endpoints. These are not fully implemented today.

| ID | Method | Route | Query | Response |
|---|---|---|---|---|
| REST-TILE-001 | GET | `/api/v1.0/dashboard-tiles/daily-results` | `date?: IsoDate` | `DailyResultsSnapshotDto` |
| REST-TILE-002 | GET | `/api/v1.0/dashboard-tiles/weekly-focus` | `weekStart?: IsoDate` | `WeeklyFocusSnapshotDto` |
| REST-TILE-003 | GET | `/api/v1.0/dashboard-tiles/monthly-progress` | `month?: YYYY-MM` | `MonthlyProgressSnapshotDto` |
| REST-TILE-004 | GET | `/api/v1.0/dashboard-tiles/outstanding-todos` | none | `OutstandingTodosSnapshotDto` |
| REST-TILE-005 | GET | `/api/v1.0/dashboard-tiles/relations` | `asOf?: IsoDate` | `RelationsSummarySnapshotDto` |

```ts
interface DailyResultsSnapshotDto {
  date: IsoDate;
  completed: number;
  expected: number;
  percent: number;
  items: DailyResultItemDto[];
}

interface DailyResultItemDto {
  commitmentId: Guid;
  behaviourId: Guid;
  name: string;
  completed: boolean;
  completedCount: number;
  target: number;
  lastActivityAt: IsoDateTime | null;
}

interface WeeklyFocusSnapshotDto {
  weekStart: IsoDate;
  weekEnd: IsoDate;
  focuses: WeeklyFocusItemDto[];
}

interface WeeklyFocusItemDto {
  commitmentId: Guid;
  behaviourId: Guid;
  name: string;
  plannedCount: number;
  completedCount: number;
  target: number;
  status: 'onTrack' | 'behind' | 'complete';
}

interface MonthlyProgressSnapshotDto {
  month: string; // YYYY-MM
  completed: number;
  expected: number;
  percent: number;
  weeks: MonthlyProgressWeekDto[];
}

interface MonthlyProgressWeekDto {
  weekStart: IsoDate;
  completed: number;
  expected: number;
  percent: number;
}

interface OutstandingTodosSnapshotDto {
  count: number;
  items: OutstandingTodoDto[];
}

interface OutstandingTodoDto {
  toDoId: Guid;
  title: string;
  dueOn: IsoDate | null;
  isCompleted: boolean;
  priority: 'low' | 'normal' | 'high' | null;
}

interface RelationsSummarySnapshotDto {
  asOf: IsoDate;
  segments: RelationSegmentDto[];
}

interface RelationSegmentDto {
  label: string;
  count: number;
  percent: number;
}
```

## 6. SignalR / WebSocket Contract

### 6.1 Transport

The realtime interface shall use SignalR.

```text
Hub route: /hub
Preferred transport: WebSockets
Fallbacks: SignalR negotiated fallback transports
Server group: profile:{profileId}
```

On successful connection, the hub shall add the connection to the active profile group. All profile-scoped updates shall be sent only to that group.

The current frontend `HubClient` listens to a SignalR method named `message` and pushes the received value into `messages$`. The future backend may either:

1. Send all messages through `SendAsync("message", message)`, which works with the current client, or
2. Add typed frontend subscriptions for individual SignalR methods.

Until the frontend client is changed, the required implementation path is `SendAsync("message", message)`.

### 6.2 Message envelope

Realtime messages shall use this envelope:

```ts
interface RealtimeMessage<TEvent extends string, TPayload> {
  schemaVersion: 1;
  messageId: Guid;
  event: TEvent;
  profileId: Guid;
  occurredAt: IsoDateTime;
  correlationId: Guid | null;
  payload: TPayload;
}
```

For backward compatibility with current subscribers, the frontend `HubClient` may flatten selected payload fields when publishing to `messages$`. New tile code should prefer the envelope.

### 6.3 Required tile messages

#### WS-TILE-001: `goalProgressUpdated`

Sent after an activity/achievement changes progress for a commitment/goal. This is the minimum live update required by the live metric tile and can also trigger the consistency chart to refresh or patch its current point.

```ts
type GoalProgressUpdatedMessage =
  RealtimeMessage<'goalProgressUpdated', GoalProgressUpdatedPayload>;

interface GoalProgressUpdatedPayload {
  goalId: Guid;
  behaviourId: Guid | null;
  count: number;
  target: number;
  percent: number;
  asOf: IsoDateTime;
  date: IsoDate;
  reason: 'activityCreated' | 'activityUpdated' | 'activityDeleted' | 'commitmentUpdated';
  sourceActivityId: Guid | null;
}
```

Publication rules:

- Publish to `profile:{profileId}` only.
- Publish once after the database transaction commits.
- Include the updated count and target so the live metric tile does not need a follow-up HTTP request.
- Live metric tile shall ignore messages whose `payload.goalId` does not match its configured `goalId`.
- Consistency trend tile may patch the latest point when `mode = live`; otherwise it should call `requestRefresh()` or refetch `/trend`.

Compatibility shape currently accepted by `LiveGoalMetricsController`:

```json
{
  "event": "goalProgressUpdated",
  "goalId": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
  "count": 13,
  "asOf": "2026-04-26T18:43:02Z"
}
```

#### WS-TILE-002: `dashboardTileDataInvalidated`

Sent when a domain change affects one or more aggregate dashboard tiles but the server does not push a full snapshot. Tiles shall refetch their REST snapshot endpoint when they receive a matching invalidation.

```ts
type DashboardTileDataInvalidatedMessage =
  RealtimeMessage<'dashboardTileDataInvalidated', DashboardTileDataInvalidatedPayload>;

type DashboardTileDataset =
  | 'dailyResults'
  | 'weeklyFocus'
  | 'monthlyProgress'
  | 'outstandingTodos'
  | 'relations'
  | 'goalTrend';

interface DashboardTileDataInvalidatedPayload {
  datasets: DashboardTileDataset[];
  affectedGoalIds: Guid[];
  affectedCommitmentIds: Guid[];
  affectedToDoIds: Guid[];
  from: IsoDate | null;
  to: IsoDate | null;
  reason:
    | 'activityChanged'
    | 'commitmentChanged'
    | 'frequencyChanged'
    | 'toDoChanged'
    | 'profileChanged';
}
```

Publication examples:

- Activity created today: `datasets = ['dailyResults', 'weeklyFocus', 'monthlyProgress', 'relations', 'goalTrend']`.
- To-do completed: `datasets = ['outstandingTodos']`.
- Commitment frequency changed: `datasets = ['dailyResults', 'weeklyFocus', 'monthlyProgress', 'relations', 'goalTrend']`.

#### WS-TILE-003: `dailyResultsUpdated`

Optional full-snapshot push for the Daily Results tile. If implemented, it supersedes `dashboardTileDataInvalidated` for this tile.

```ts
type DailyResultsUpdatedMessage =
  RealtimeMessage<'dailyResultsUpdated', DailyResultsSnapshotDto>;
```

#### WS-TILE-004: `weeklyFocusUpdated`

Optional full-snapshot push for the Weekly Focus tile.

```ts
type WeeklyFocusUpdatedMessage =
  RealtimeMessage<'weeklyFocusUpdated', WeeklyFocusSnapshotDto>;
```

#### WS-TILE-005: `monthlyProgressUpdated`

Optional full-snapshot push for the Monthly Progress tile.

```ts
type MonthlyProgressUpdatedMessage =
  RealtimeMessage<'monthlyProgressUpdated', MonthlyProgressSnapshotDto>;
```

#### WS-TILE-006: `outstandingTodosUpdated`

Optional full-snapshot push for the Outstanding To Dos tile.

```ts
type OutstandingTodosUpdatedMessage =
  RealtimeMessage<'outstandingTodosUpdated', OutstandingTodosSnapshotDto>;
```

#### WS-TILE-007: `relationsSummaryUpdated`

Optional full-snapshot push for the Relations tile.

```ts
type RelationsSummaryUpdatedMessage =
  RealtimeMessage<'relationsSummaryUpdated', RelationsSummarySnapshotDto>;
```

### 6.4 Existing non-tile messages

The current frontend store already filters hub messages with these legacy `type` values:

```ts
type LegacyStoreMessage =
  | { type: '[Note] Saved'; payload: { note: unknown } }
  | { type: '[Note] Removed'; payload: { noteId: Guid } }
  | { type: '[Tag] Saved'; payload: { tag: unknown } }
  | { type: '[Tag] Removed'; payload: { tagId: Guid } };
```

If notes/tags continue to use realtime updates, these should be migrated to the envelope format:

- `noteSaved`
- `noteRemoved`
- `tagSaved`
- `tagRemoved`

The legacy `type` values may be emitted in parallel until existing subscribers are removed.

### 6.5 Client behavior

- The hub connection starts after authentication through `hubClientGuard`.
- Logout shall stop the hub connection.
- Tiles shall filter messages by `profileId` and by the relevant entity ID even though the server sends only to profile groups.
- Review-mode tiles generally shall not consume live update payloads directly. They may treat `dashboardTileDataInvalidated` as a reason to refresh today's comparison value.
- The client shall tolerate duplicate messages by using `messageId` for idempotence when a tile keeps local patch state.

## 7. Tile-Specific Interface Matrix

| Tile | Mode | Initial REST | Realtime messages | Review date handling |
|---|---|---|---|---|
| Daily Results | live | `daily-results?date=today` | `dailyResultsUpdated` or `dashboardTileDataInvalidated` | Not applicable |
| Weekly Focus | live | `weekly-focus?weekStart=currentWeek` | `weeklyFocusUpdated` or `dashboardTileDataInvalidated` | Not applicable |
| Monthly Progress | live | `monthly-progress?month=currentMonth` | `monthlyProgressUpdated` or `dashboardTileDataInvalidated` | Not applicable |
| Outstanding To Dos | live | `outstanding-todos` | `outstandingTodosUpdated` or `dashboardTileDataInvalidated` | Not applicable |
| Relations | live/review | `relations?asOf=today/selectedDate` | `relationsSummaryUpdated` or invalidation in live | `TileContext.selectedReviewDate` if review-enabled |
| Live real-time metric | live | `goal-progress/current`, `goal-progress/last14` | `goalProgressUpdated` | Not applicable |
| Review goal history | review | `goal-progress/at`, `goal-progress/current` | Optional invalidation to refresh today's comparison | Uses `TileContext.selectedReviewDate` at end of day |
| Consistency Trend | live/review | `goal-progress/trend` | `goalProgressUpdated` or invalidation in live | Adds `asOf` from `TileContext.selectedReviewDate` |

## 8. Frontend Tile Context Contract

The framework provides this context to each tile through `TILE_CONTEXT`:

```ts
interface TileContext {
  readonly tileId: string;
  readonly instanceId: string;
  readonly isEditMode: Signal<boolean>;
  readonly isMaximized: Signal<boolean>;
  readonly mode: Signal<DashboardMode>;
  readonly selectedReviewDate: Signal<IsoDate | null>;
  readonly refresh$: Observable<void>;
  requestRefresh(): void;
  remove(): void;
  maximize(): void;
  restore(): void;
  requestFocus(): void;
}
```

Backend dependencies shall not leak into tile context. Tile context is frontend-only orchestration.

## 9. Implementation Alignment Items

The following items were observed in the current source and should be reconciled as implementation proceeds:

1. Backend controllers are versioned and often singular, for example `/api/v1.0/dashboard`, while several frontend services use unversioned plural routes, for example `/api/dashboards`. The target contract is versioned.
2. The backend expects a `ProfileId` header for profile-scoped endpoints. The current `headers.interceptor.ts` only sets `Authorization`; it shall also set `ProfileId`.
3. The frontend has a SignalR client for `/hub`, but `Commitments.Api` does not currently register SignalR services or map `/hub`.
4. The current frontend hub client listens to the generic SignalR method `message`. The first backend hub implementation shall use `SendAsync("message", envelope)` unless the frontend client is updated in the same slice.
5. Requirements and detailed designs refer to goals and achievements. Current backend goal-progress code maps `goalId` to `CommitmentId` and counts `Activities`. The DTO name may stay `GoalProgressDto`, but implementation comments and tests should make that mapping explicit.
6. `goal-progress/current` and `at` currently return count `0` when the commitment is not found. The requirements call for 404 when the goal is not owned by the active profile; the backend should align with the requirement.
7. Aggregate tile endpoints for Daily Results, Weekly Focus, Monthly Progress, Outstanding To Dos, and Relations are target contracts. Current plugin tiles are static placeholders.

## 10. Acceptance Criteria For This ICD

- A developer can implement the backend hub without guessing event names or payload shapes.
- A tile author can identify which REST endpoint and realtime message each tile consumes.
- All profile-scoped requests and messages include clear auth/profile routing requirements.
- Existing code gaps are visible enough to drive follow-up implementation tickets.
