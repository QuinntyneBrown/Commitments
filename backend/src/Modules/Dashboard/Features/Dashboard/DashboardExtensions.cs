using Dashboard.Domain.DashboardAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Features.Dashboard;

public static class DashboardExtensions
{
    public static DashboardDto ToDto(this DashboardEntity dashboard)
    {
        return new DashboardDto
        {
            DashboardId = dashboard.DashboardId,
            Name = dashboard.Name,
            ProfileId = dashboard.ProfileId
        };
    }

    public static async Task<List<DashboardDto>> ToDtosAsync(this IQueryable<DashboardEntity> dashboards, CancellationToken cancellationToken)
    {
        return await dashboards.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}
