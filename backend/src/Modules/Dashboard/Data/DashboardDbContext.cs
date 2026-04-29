using Commitments.Shared;
using Dashboard.Data;
using Dashboard.Domain.DashboardAggregate;
using Dashboard.Domain.DashboardCardAggregate;
using Dashboard.Domain.CardAggregate;
using Dashboard.Domain.CardLayoutAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;

namespace Dashboard.Data;

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

        modelBuilder.Entity<DashboardEntity>()
            .HasKey(d => d.DashboardId);

        modelBuilder.Entity<Card>()
            .HasKey(c => c.CardId);

        modelBuilder.Entity<CardLayout>()
            .HasKey(cl => cl.CardLayoutId);

        modelBuilder.Entity<DashboardCard>()
            .HasKey(dc => dc.DashboardCardId);

        var jobjectConverter = new ValueConverter<JObject, string>(
            v => v.ToString(),
            v => JObject.Parse(v));

        modelBuilder.Entity<DashboardCard>()
            .Property(dc => dc.Options)
            .HasConversion(jobjectConverter);

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
