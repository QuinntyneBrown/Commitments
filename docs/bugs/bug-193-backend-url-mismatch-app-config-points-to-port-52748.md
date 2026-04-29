---
id: bug-193
title: DASHBOARD_BACKEND_BASE_URL in app.config.ts points to port 52748 which is not running — backend is on 63714 (HTTP) / 63713 (HTTPS)
status: Fixed
---

## Symptom

Login always fails silently — POST to `http://localhost:52748/api/v1.0/users/token` returns a network error (connection refused) so the user is never authenticated.

## Root cause

`frontend/projects/commitments-app/src/app/app.config.ts` supplies:

```ts
{ provide: DASHBOARD_BACKEND_BASE_URL, useValue: 'http://localhost:52748/' }
```

But `backend/src/Commitments.Api/Properties/launchSettings.json` shows the API runs on:

```
https://localhost:63713;http://localhost:63714
```

Port 52748 is not bound anywhere.

## Fix

Update `DASHBOARD_BACKEND_BASE_URL` to `http://localhost:63714/`.
