using Commitments.API.Features.DashboardCards;
using Commitments.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Commitments.API.Features.Dashboards
{
    public class DashboardApiModel
    {        
        public int DashboardId { get; set; }
        public string Name { get; set; }
        public int ProfileId { get; set; }
        public ICollection<DashboardCardApiModel> DashboardCards { get; set; }
        = new HashSet<DashboardCardApiModel>();

        public static DashboardApiModel FromDashboard(Dashboard dashboard)
        {
            var model = new DashboardApiModel();
            model.DashboardId = dashboard.DashboardId;
            model.Name = dashboard.Name;
            model.ProfileId = dashboard.ProfileId;
            model.DashboardCards = dashboard.DashboardCards.Select(x => DashboardCardApiModel.FromDashboardCard(x)).ToList();
            return model;
        }
    }
}
