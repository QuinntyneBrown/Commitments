// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DashboardService.Core;
using DashboardService.Core.AggregateModel;
using Microsoft.EntityFrameworkCore;

namespace DashboardService.Infrastructure.Data;

public class DashboardServiceDbContext : DbContext, IDashboardServiceDbContext
{
    public DashboardServiceDbContext(DbContextOptions<DashboardServiceDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Dashboard");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DashboardServiceDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Card> Cards { get; private set; }
    public DbSet<CardLayout> CardLayouts { get; private set; }
    public DbSet<Dashboard> Dashboards { get; private set; }
    public DbSet<DashboardCard> DashboardCards { get; private set; }
    public DbSet<User> Users { get; private set; }
}