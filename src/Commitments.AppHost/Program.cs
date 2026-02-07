// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");

var sqlServer = builder.AddSqlServer("sql");

var commitmentsDb = sqlServer.AddDatabase("CommitmentsDb");
var dashboardDb = sqlServer.AddDatabase("DashboardDb");
var identityDb = sqlServer.AddDatabase("IdentityDb");
var digitalAssetsDb = sqlServer.AddDatabase("DigitalAssetsDb");

var commitmentsApi = builder.AddProject<Projects.Commitments_Api>("commitments-api")
    .WithReference(redis)
    .WithReference(commitmentsDb)
    .WaitFor(redis)
    .WaitFor(commitmentsDb);

var dashboardApi = builder.AddProject<Projects.Dashboard_Api>("dashboard-api")
    .WithReference(redis)
    .WithReference(dashboardDb)
    .WaitFor(redis)
    .WaitFor(dashboardDb);

var identityApi = builder.AddProject<Projects.Identity_Api>("identity-api")
    .WithReference(redis)
    .WithReference(identityDb)
    .WaitFor(redis)
    .WaitFor(identityDb);

var digitalAssetsApi = builder.AddProject<Projects.DigitalAssets_Api>("digitalassets-api")
    .WithReference(redis)
    .WithReference(digitalAssetsDb)
    .WaitFor(redis)
    .WaitFor(digitalAssetsDb);

builder.Build().Run();
