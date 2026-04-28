# Commitments — Glossary

Definitions for every domain term used in the Commitments app, together with the app capabilities associated with each term.

---

## Achievement

A metric that measures progress toward a commitment goal. Achievements are computed from activity history and surfaced as milestone indicators.

**Associated capabilities:** Goal Metrics tile, goal progress queries, achievement management CRUD (`/achievements`).

---

## Activity

A record that a behaviour was performed at a specific point in time. Every time a user logs that they completed a behaviour, an Activity is created.

**Associated capabilities:** Activity recording flow (`/activities`), Edit Activity dialog, real-time goal progress recalculation via `ActivityRecordedEvent`, Daily Results tile.

---

## Behaviour

A reusable, named action that a user wants to track — for example "Drink 2 L of water" or "Read for 30 minutes". Behaviours are the atomic building block of commitments.

**Associated capabilities:** Behaviour catalog management (`/behaviours`), Edit Behaviour dialog, behaviour type assignment, activity recording against a behaviour.

---

## Behaviour Type

A category that groups related behaviours — for example "Health", "Learning", or "Work". Behaviour types allow filtering and distribution analysis across commitments.

**Associated capabilities:** Behaviour Type catalog management (`/behaviour-types`), Edit Behaviour Type dialog, Relations tile (category distribution chart).

---

## Card

An entry in the dashboard card catalog. A card defines a reusable component type — its name, description, and which frontend tile component it maps to. Cards are the definitions; Dashboard Cards are the instances.

**Associated capabilities:** Card catalog management (`/cards`), Edit Card dialog, Add Dashboard Cards dialog.

---

## Card Layout

A layout definition that specifies the grid geometry (position, width, height) and responsive breakpoint behaviour for a card when placed on a dashboard.

**Associated capabilities:** Card Layout management (`/card-layouts`), Edit Card Layout dialog, dashboard grid rendering.

---

## Commitment

The core aggregate of the application. A commitment pairs one or more behaviours with one or more frequencies to express an intention — for example "Drink 2 L of water, daily". Commitments drive goal progress tracking and activity recording.

**Associated capabilities:** Commitment tracking (`/commitments`), Edit Commitment dialog, commitment frequency linking, pre-condition configuration, goal progress calculation, `CommitmentChangedEvent`.

---

## Commitment Frequency

A join between a Commitment and a Frequency. Because a single commitment can target multiple cadences (e.g. "3× per week" and "daily"), this entity captures each pairing explicitly.

**Associated capabilities:** Managed transparently through the Edit Commitment dialog; drives the frequency-aware goal progress calculation.

---

## Commitment Pre-Condition

An optional constraint attached to a commitment that must be satisfied before the commitment is counted as active — for example "only on weekdays".

**Associated capabilities:** Configurable in the Edit Commitment dialog; evaluated during goal progress calculation.

---

## Dashboard

A personalised, grid-based workspace that displays a collection of tiles. Each user profile can have its own dashboard layout.

**Associated capabilities:** Dashboard management, Edit/Live/Review modes, layout persistence, Add Dashboard Cards dialog, dashboard card configuration.

---

## Dashboard Card

An instance of a Card placed on a specific dashboard. Stores the position, size, and any configuration specific to that placement.

**Associated capabilities:** Add/remove tiles in Edit Mode, Dashboard Card Configuration dialog, layout persistence.

---

## Dashboard Mode

Controls the temporal context of all tiles on the dashboard simultaneously.

- **Live Mode** — tiles stream real-time data; indicated by a LIVE badge. Data arrives via SignalR.
- **Review Mode** — tiles render historical data for a user-selected date/time; indicated by a REVIEW badge.
- **Edit Mode** — allows adding, removing, reordering, and resizing tiles.

**Associated capabilities:** Mode toggle in the dashboard toolbar, `DashboardModeService`, `TileContext` framework, snapshot endpoints.

---

## Digital Asset

A file or image uploaded and stored by the application, scoped to a profile. Used to attach images to notes, behaviours, or other entities.

**Associated capabilities:** Digital asset upload/retrieval, profile-scoped asset library.

---

## Frequency

A descriptor for how often a behaviour should be performed — for example "Daily", "3× per week", or "Weekly". Frequencies are reusable catalog entries.

**Associated capabilities:** Frequency catalog management (`/frequencies`), Edit Frequency dialog, commitment-frequency linking, goal progress weighting.

---

## Frequency Type

A grouping category for frequencies — for example "Daily Family" or "Weekly Family". Allows filtering and organising the frequency catalog.

**Associated capabilities:** Frequency Type catalog management (managed alongside frequencies), commitment configuration.

---

## Goal Progress

A real-time snapshot of how close a commitment is to its target for the current period. Recalculated whenever an activity is recorded or a commitment changes.

**Associated capabilities:** Goal Metrics tile, `GoalProgressController`, `GoalProgressUpdatedRealtimeNotifier` (SignalR push), `GoalProgressUpdated` integration event.

---

## Monthly Progress

An aggregated view of activity completion grouped into four buckets across a calendar month. Used to identify monthly patterns and consistency.

**Associated capabilities:** Monthly Progress tile, `MonthlyProgressController`.

---

## Note

A rich-text entry with a title and one or more tags. Notes provide a free-form journalling layer alongside structured commitment tracking.

**Associated capabilities:** Note management (`/notes`), Note tile, tag assignment, note resolver, `NoteSavedEvent` / `NoteRemovedEvent`, real-time note/tag envelope via SignalR.

---

## Outstanding To-Dos

The subset of To-Do items that have not yet been completed. Surfaced as a dedicated dashboard tile for at-a-glance task awareness.

**Associated capabilities:** Outstanding To-Dos tile, filtered query on the To-Do endpoint.

---

## Profile

A tenant context owned by a user. One user account can maintain multiple profiles (e.g. personal vs. work), each with its own commitments, activities, notes, and dashboard layout.

**Associated capabilities:** Profile management (`/profiles`, `/my-profile`), Create Profile dialog, profile switching, `ProfileCreatedEvent` / `ProfileDeletedEvent`, per-profile data isolation.

---

## Relations

A metric that shows the distribution of activity across behaviour categories. Answers the question "where am I spending my behavioural effort?"

**Associated capabilities:** Relations tile, `RelationsController`, category breakdown chart.

---

## Streak

A counter that tracks consecutive periods (days, weeks) in which a commitment was fulfilled without a gap.

**Associated capabilities:** Streak tile.

---

## Tag

A short label applied to one or more notes for filtering and organisation.

**Associated capabilities:** Tag management (`/tags`), tag assignment in the Note editor, tags resolver, `TagSavedEvent` / `TagRemovedEvent`, real-time tag envelope via SignalR.

---

## Tile

A self-contained widget rendered inside the dashboard. Each tile type binds to a specific data provider and renders a focused view of one metric or feature. Tiles are mode-aware — they automatically switch their data source between live and historical when the dashboard mode changes.

**Tile types:**
| Tile | What it shows |
|---|---|
| Daily Results | Activities completed today |
| Weekly Focus | Aggregated weekly commitment progress |
| Monthly Progress | Four-bucket monthly activity breakdown |
| Goal Metrics | Per-commitment progress toward targets |
| Relations | Behaviour category distribution |
| Outstanding To-Dos | Incomplete to-do items |
| Notes | Recent notes |
| Streak | Consecutive fulfilment streak |
| Poster | Customisable motivational poster |

**Associated capabilities:** Tile Registry, `TileContext` framework, Add Dashboard Cards dialog, Dashboard Edit Mode.

---

## To-Do

A discrete tracked task that can be created, updated, completed, or removed. To-Dos live alongside commitments as a lightweight task layer.

**Associated capabilities:** To-Do management (`/to-dos`), Edit To-Do dialog, Outstanding To-Dos tile, `ToDoChangedEvent`.

---

## User

An authentication identity with a username and password. A user owns one or more profiles.

**Associated capabilities:** Login flow (`/login`), `AuthService`, `UserCreatedEvent`, seed user provisioning.

---

## Weekly Focus

An aggregated summary of activity completion across the current week, highlighting which commitments received attention and which were neglected.

**Associated capabilities:** Weekly Focus tile, `WeeklyFocusController`.
