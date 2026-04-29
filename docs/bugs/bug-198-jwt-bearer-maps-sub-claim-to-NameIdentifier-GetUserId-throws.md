---
id: bug-198
title: JWT Bearer maps "sub" → ClaimTypes.NameIdentifier — GetUserId() can't find "sub" claim and throws FormatException, causing 500 on all authenticated endpoints
status: Open
---

## Symptom

Every authenticated API call (e.g. `GET /api/v1.0/profiles/current`) returns HTTP 500:

```
System.FormatException: Unrecognized Guid format.
   at System.Guid..ctor(String g)
   at Commitments.Shared.HttpContextAccessorExtensions.GetUserId(...)
```

The `jwtInterceptor` in the Angular app treats this 500 as a "failed" response and on 401 redirects to `/login`. The profile call triggers a redirect that kicks authenticated users back to the login page immediately.

## Root cause

`JwtAccessTokenIssuer.Issue()` adds the claim with key `JwtRegisteredClaimNames.Sub` (`"sub"`). The ASP.NET Core JWT Bearer middleware, by default, **maps inbound JWT claims** — it converts `"sub"` → `ClaimTypes.NameIdentifier` (`http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier`).

`GetUserId()` then calls `User.FindFirst("sub")` which returns `null` (the claim is stored under a different key). `sub ?? string.Empty` produces `""`, and `new Guid("")` throws `FormatException`.

## Fix

Set `MapInboundClaims = false` in the JWT Bearer options so the `"sub"` claim name is preserved verbatim in the claims identity.

```csharp
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false;  // add this
    options.TokenValidationParameters = new TokenValidationParameters { ... };
});
```
