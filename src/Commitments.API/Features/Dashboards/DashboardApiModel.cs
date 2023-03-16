using Commitments.Api.Features.DashboardCards;
using Commitments.Core.Entities;
using System.Collections.Generic;
using System.Linq;


namespace Commitments.Api.Features.Dashboards;

public class DashboardApiModel
{        
    public int DashboardId { get; set; }
    public string Name { get; set; }
    public int ProfileId { get; set; }
    public ICollection<DashboardCardApiModel> DashboardCards { get; set; }
    = new HashSet<DashboardCardApiModel>();

    public static DashboardApiModel FromDashboard(Dashboard dashboard)
        => new DashboardApiModel
        {
            DashboardId = dashboard.DashboardId,
            Name = dashboard.Name,
            ProfileId = dashboard.ProfileId,
            DashboardCards = dashboard.DashboardCards.Select(x => DashboardCardApiModel.FromDashboardCard(x)).ToList()
        };
}
