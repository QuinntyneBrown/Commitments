# Commitments Power User Guide

This guide is for users who run Commitments as a daily operating system for goals, behaviours, activities, notes, to-dos, and dashboard review. It focuses on practical workflows: what to set up first, where to go, what each dashboard mode means, and how to audit historical changes.

The guide follows the application capabilities described in `docs/specs`, the current Angular dashboard shell, and the existing Commitments domain model.

## Guide Map

1. [Getting Started](01-getting-started.md)
2. [Navigation and Workspace](02-navigation-and-workspace.md)
3. [Dashboard Live Metrics](03-dashboard-live-metrics.md)
4. [Review Mode and History Scrubbing](04-review-mode-and-history-scrubbing.md)
5. [Commitments, Behaviours, Activities, and Frequencies](05-commitments-behaviours-activities-frequencies.md)
6. [To-Dos, Notes, and Tags](06-to-dos-notes-tags.md)
7. [Profiles, Settings, and Assets](07-profiles-settings-and-assets.md)
8. [Dashboard Cards and Layouts](08-dashboard-cards-and-layouts.md)
9. [Troubleshooting and Power Workflows](09-troubleshooting-and-power-workflows.md)

## Mental Model

Commitments works best when you treat it as a chain:

```text
Profile -> Behaviour catalog -> Frequency catalog -> Commitment -> Activity -> Dashboard metric -> Review history
```

- A **profile** owns all of the data you see.
- A **behaviour** is something you do or avoid, such as "Drink water" or "Skip late snacks".
- A **frequency** describes the expected cadence, such as daily, weekly, or three times per week.
- A **commitment** pairs one behaviour with one or more frequencies and optional pre-conditions.
- An **activity** records that a behaviour happened at a specific time.
- Dashboard **live mode** shows current progress and listens for streamed updates.
- Dashboard **review mode** lets you scrub through historical dates and compare metrics over time.

## Fast Start Workflow

1. Sign in.
2. Confirm the active profile in the toolbar.
3. Create or verify behaviour types.
4. Create behaviours.
5. Create frequency types and frequencies.
6. Create commitments that combine behaviours and frequencies.
7. Record activities as work happens.
8. Add dashboard tiles for the metrics you care about.
9. Keep the dashboard in Live mode during the day.
10. Switch to Review mode to scrub historical metrics and diagnose changes.

## Core Destinations

The authenticated app shell exposes these primary destinations when the full workspace shell is enabled:

| Destination | Use it for |
|---|---|
| Dashboard | Live metrics, review mode, dashboard cards, tile layout |
| Activities | Recording behaviour activity history |
| Behaviours | Managing reusable behaviours |
| Behaviour Types | Grouping behaviours |
| Commitments | Building and maintaining commitments |
| Cards | Managing dashboard card catalog entries |
| Card Layouts | Managing dashboard card layout definitions |
| Frequencies | Managing frequency rules |
| Notes | Writing rich-text notes |
| Profiles | Creating and managing profiles |
| To Do's | Tracking outstanding tasks |
| Login / Logout | Starting or ending a session |

## Power User Principles

- Keep catalog data clean. Good behaviours and frequencies make commitments easier to maintain.
- Record activities as soon as possible. Live metrics are only as useful as the events feeding them.
- Use Live mode for operations and Review mode for analysis.
- Scrub historical dashboard metrics after changing commitments, frequencies, or activity history.
- Keep one dashboard layout for daily monitoring and use review tiles for diagnostic work.
- Use the "+" FAB to add tiles and enter edit mode (from Live) to rearrange or remove them.
- Use tags on notes consistently so reviews and retrospectives are easy to navigate.

