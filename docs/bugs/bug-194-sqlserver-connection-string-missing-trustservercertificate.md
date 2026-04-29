---
id: bug-194
title: SQL Server connection strings missing TrustServerCertificate=True — token and all DB operations fail with SSL cert chain error
status: Open
---

## Symptom

Any API endpoint that queries the database throws:

```
Microsoft.Data.SqlClient.SqlException: A connection was successfully established with the server,
but then an error occurred during the login process.
(provider: SSL Provider, error: 0 - The certificate chain was issued by an authority that is not trusted.)
```

This affects all modules (Commitments, Identity, Dashboard, DigitalAssets).

## Root cause

`Microsoft.Data.SqlClient` v4+ enforces SSL by default. SQLEXPRESS on dev machines uses a self-signed certificate that is not trusted by the Windows certificate store. Without `TrustServerCertificate=True` the driver refuses the connection.

The connection strings in both `appsettings.json` and `appsettings.Development.json` are missing this flag:

```
Data Source=.\SQLEXPRESS;Initial Catalog=Commitments;Integrated Security=SSPI;
```

## Fix

Add `TrustServerCertificate=True` to every connection string in `appsettings.Development.json`.
