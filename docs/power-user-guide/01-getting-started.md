# Getting Started

This chapter covers launching the app, signing in, selecting the right profile, and preparing a clean workspace.

## Run Locally

From the repository root, run the backend:

```powershell
cd backend
dotnet restore
dotnet build
dotnet run --project src/Commitments.Api
```

To apply migrations and seed reference data at startup:

```powershell
cd backend
dotnet run --project src/Commitments.Api -- migratedb seeddb
```

Run the Angular frontend in another terminal:

```powershell
cd frontend
npm install
npm start
```

The frontend uses the backend base URL configured in the Angular app configuration. In the current app configuration that URL is `http://localhost:52748/`.

## Sign In

1. Open the frontend in a browser.
2. Enter your username and password.
3. Submit the form.
4. Confirm that you land on the dashboard or the destination you attempted to open before login.

After sign-in, the app stores the session token and active profile id in browser storage. Protected API calls include the session token and active profile id so the backend can scope data to the correct profile.

## Pick the Right Profile

A profile is the boundary for commitments, activities, notes, to-dos, dashboard cards, and assets. If the data on a page looks empty or unexpected, check the active profile first.

Recommended profile workflow:

1. Open the profile menu or Profiles page.
2. Confirm your current profile name.
3. Create a profile if this is a new workspace.
4. Keep separate profiles for meaningfully separate contexts, such as personal, coaching, test data, or demos.

## First-Time Setup Checklist

Set up the data in this order:

1. Behaviour Types
2. Behaviours
3. Frequency Types
4. Frequencies
5. Commitments
6. Dashboard cards and tiles
7. Notes and tags
8. To-dos

This order prevents selection lists from being empty when you create commitments.

## Session Safety

- Use Logout when you are finished on a shared machine.
- If you switch profiles, refresh the dashboard after the switch so live connections and cached dashboard data align with the new profile.
- If the browser has stale data after a backend restart, sign out and sign back in.

## Data Hygiene

Good Commitments data is specific and reusable:

- Prefer "Walk 30 minutes" over "Exercise".
- Prefer "Read technical book" over "Learn".
- Create one behaviour per trackable action.
- Use commitments to express expectations, not one-off tasks.
- Use to-dos for one-off tasks.
- Use notes for context, decisions, retrospectives, and plans.

