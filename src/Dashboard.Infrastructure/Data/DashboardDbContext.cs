using Commitments.Shared;
using Dashboard.Core;
using Dashboard.Core.Model.DashboardAggregate;
using Dashboard.Core.Model.DashboardCardAggregate;
using Dashboard.Core.Model.CardAggregate;
using Dashboard.Core.Model.CardLayoutAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Infrastructure.Data;

public class DashboardDbContext : BaseDbContext, IDashboardDbContext
{
    public DashboardDbContext(DbContextOptions<DashboardDbContext> options)
        : base(options)
    {
    }

    public DbSet<DashboardEntity> Dashboards { get; private set; }
    public DbSet<DashboardCard> DashboardCards { get; private set; }
    public DbSet<Card> Cards { get; private set; }
    public DbSet<CardLayout> CardLayouts { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Dashboard");

        modelBuilder.Entity<DashboardCard>()
            .HasOne(dc => dc.Dashboard)
            .WithMany(d => d.DashboardCards)
            .HasForeignKey(dc => dc.DashboardId);

        modelBuilder.Entity<DashboardCard>()
            .HasOne(dc => dc.Card)
            .WithMany()
            .HasForeignKey(dc => dc.CardId);

        modelBuilder.Entity<DashboardCard>()
            .HasOne(dc => dc.CardLayout)
            .WithMany()
            .HasForeignKey(dc => dc.CardLayoutId);

        base.OnModelCreating(modelBuilder);
    }
}
