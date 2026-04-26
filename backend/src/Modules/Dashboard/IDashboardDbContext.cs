using Dashboard.Core.Model.DashboardAggregate;
using Dashboard.Core.Model.DashboardCardAggregate;
using Dashboard.Core.Model.CardAggregate;
using Dashboard.Core.Model.CardLayoutAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Core;

public interface IDashboardDbContext : IDisposable
{
    DbSet<DashboardEntity> Dashboards { get; }
    DbSet<DashboardCard> DashboardCards { get; }
    DbSet<Card> Cards { get; }
    DbSet<CardLayout> CardLayouts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
