using Commitments.Core.Entities;
using Newtonsoft.Json;

namespace Commitments.API.Features.DashboardCards
{
    public class DashboardCardApiModel
    {        
        public int DashboardCardId { get; set; }
        public int DashboardId { get; set; }
        public int CardId { get; set; }
        public OptionsApiModel Options { get; set; }

        public static DashboardCardApiModel FromDashboardCard(DashboardCard dashboardCard)
        {
            var model = new DashboardCardApiModel();
            model.DashboardCardId = dashboardCard.DashboardCardId;
            model.DashboardId = dashboardCard.DashboardId;
            model.CardId = dashboardCard.CardId;
            model.Options = JsonConvert.DeserializeObject<OptionsApiModel>(dashboardCard.Options);
            return model;
        }

        public class OptionsApiModel {
            public int Top { get; set; } = 1;
            public int Left { get; set; } = 1;
            public int Height { get; set; } = 1;
            public int Width { get; set; } = 1;
        }
    }
}
