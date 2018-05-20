using Commitments.Core.Entities;

namespace Commitments.API.Features.Dashboards
{
    public class DashboardApiModel
    {        
        public int DashboardId { get; set; }
        public string Name { get; set; }

        public static DashboardApiModel FromDashboard(Dashboard dashboard)
        {
            var model = new DashboardApiModel();
            model.DashboardId = dashboard.DashboardId;
            model.Name = dashboard.Name;
            return model;
        }
    }
}
