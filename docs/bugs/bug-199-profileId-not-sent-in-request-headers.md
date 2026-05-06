---
id: bug-199
title: ProfileId not included in token response and not sent in request headers — all profile-scoped endpoints throw FormatException (500)
status: Fixed
---

## Symptom

All API endpoints that call `GetProfileId()` return HTTP 500:

```
System.FormatException: Unrecognized Guid format.
   at System.Guid..ctor(String g)
   at Commitments.Shared.HttpContextAccessorExtensions.GetProfileId(...)
```

Affected endpoints include:
- `GET /api/v1.0/commitments/daily-results`
- `GET /api/v1.0/relations/summary`
- `GET /api/v1.0/behaviours/weekly-focus` (and others)

In the Angular app, all dashboard tiles that call these endpoints show empty state ("No data yet.") because the API calls fail with 500.

## Root Cause

`GetProfileId()` reads `Request.Headers["ProfileId"]` and does `new Guid(profileId)`. When the header is missing (empty string), `new Guid("")` throws `FormatException`.

The `ProfileId` header is never sent because:
1. The token endpoint (`POST /api/v1.0/users/token`) does not include `profileId` in the response
2. The login component (`LoginPageComponent.signIn()`) only stores `accessToken` in localStorage, ignoring `profileId`  
3. The `headerInterceptor` never adds a `ProfileId` header

## Fix

### Backend
Include the user's first `ProfileId` in the token response:
- `GetTokenByUsernameAndPasswordResponse`: add `ProfileId` field
- `GetTokenByUsernameAndPasswordRequestHandler`: query `Profiles` for the user and include the first profile's ID

### Frontend
1. `LoginPageComponent.signIn()`: store `profileId` from the response in localStorage as `'profileId'`
2. `headerInterceptor`: add `ProfileId: <stored-profileId>` header to every outgoing request
