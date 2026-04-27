# Commitments, Behaviours, Activities, and Frequencies

This is the core tracking system. Build the catalog first, then record activity against it.

## Data Model

```text
Behaviour Type -> Behaviour
Frequency Type -> Frequency
Behaviour + Frequency -> Commitment
Behaviour performed at time -> Activity
Activity updates progress -> Dashboard metrics
```

## Behaviour Types

Behaviour types group behaviours so lists are easier to scan.

Examples:

- Health
- Work
- Learning
- Relationships
- Avoidance

Use Behaviour Types when:

- You have many behaviours.
- You want dashboard relations or balance metrics to be meaningful.
- You need a clean catalog for future commitments.

Basic workflow:

1. Open Behaviour Types.
2. Add a type.
3. Edit the name if needed.
4. Delete only when no behaviours depend on it.

## Behaviours

A behaviour is a reusable action.

Examples:

- Drink 2L water.
- Walk 30 minutes.
- Write project notes.
- Avoid late snacks.
- Review commitments.

Basic workflow:

1. Open Behaviours.
2. Add a behaviour.
3. Assign its behaviour type.
4. Save.
5. Use it later in commitments and activities.

Deletion may be blocked if a behaviour is referenced by commitments or activity history. Prefer editing names for corrections and deleting only unused entries.

## Frequency Types

Frequency types describe the family of a frequency rule.

Examples:

- Daily
- Weekly
- Monthly
- Custom

Use them to keep frequency lists clear and selectable.

## Frequencies

A frequency describes how often a behaviour is expected.

Examples:

- Daily.
- Three times per week.
- Weekdays.
- Once per month.

Frequencies can also mark whether the behaviour is desirable. A desirable frequency means you want to do the behaviour. An undesirable frequency means you want to avoid the behaviour.

Basic workflow:

1. Open Frequencies.
2. Add a frequency.
3. Choose its frequency type.
4. Set whether it is desirable.
5. Save.

## Commitments

A commitment pairs one behaviour with expected frequency rules. It is the thing dashboard metrics evaluate.

Use commitments for durable expectations:

- Walk 30 minutes daily.
- Write notes three times per week.
- Avoid late snacks daily.
- Review open to-dos every weekday.

Basic workflow:

1. Open Commitments.
2. Add a commitment.
3. Select one behaviour.
4. Add one or more frequencies.
5. Add pre-conditions if needed.
6. Save.

Pre-conditions are useful when a commitment only applies under specific circumstances. Keep them short and testable.

## Activities

An activity records that a behaviour happened at a specific time.

Basic workflow:

1. Open Activities.
2. Add an activity.
3. Select the behaviour performed.
4. Confirm or edit the performed date and time.
5. Add a description if useful.
6. Save.

Activities are the primary input for progress metrics. Recording an activity can update Live Goal Metrics and other live dashboard tiles.

## Recommended Daily Tracking Routine

1. Start on Dashboard Live mode.
2. Record each activity soon after it happens.
3. Watch the relevant live tile update.
4. Complete any related to-do.
5. At the end of the day, use Review mode to confirm the final snapshot.

## Maintenance Routine

Weekly:

1. Review behaviours for duplicates.
2. Review frequencies for stale expectations.
3. Remove or edit commitments that no longer apply.
4. Use Review mode to compare before and after changes.

Monthly:

1. Review long-running commitments.
2. Check whether activity history supports the dashboard story.
3. Archive or delete unused catalog entries where safe.

