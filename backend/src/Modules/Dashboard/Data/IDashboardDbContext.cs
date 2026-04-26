using Dashboard.Domain.DashboardAggregate;
using Dashboard.Domain.DashboardCardAggregate;
using Dashboard.Domain.CardAggregate;
using Dashboard.Domain.CardLayoutAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data;

public interface IDashboardDbContext : IDisposable
{
    DbSet<DashboardEntity> Dashboards { get; }
    DbSet<DashboardCard> DashboardCards { get; }
    DbSet<Card> Cards { get; }
    DbSet<CardLayout> CardLayouts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
