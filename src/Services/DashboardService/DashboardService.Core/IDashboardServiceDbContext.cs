// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DashboardService.Core.AggregateModel;

namespace DashboardService.Core;

public interface IDashboardServiceDbContext
{
    DbSet<Card> Cards { get; }
    DbSet<CardLayout> CardLayouts { get; }
    DbSet<Dashboard> Dashboards { get; }
    DbSet<DashboardCard> DashboardCards { get; }
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}