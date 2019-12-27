using Commitments.Core.Entities;
using Newtonsoft.Json;

namespace Commitments.Api.Features.DashboardCards
{
    public class DashboardCardApiModel
    {        
        public int DashboardCardId { get; set; }
        public int DashboardId { get; set; }
        public int? CardId { get; set; }
        public int? CardLayoutId { get; set; }
        public OptionsApiModel Options { get; set; }

        public static DashboardCardApiModel FromDashboardCard(DashboardCard dashboardCard)
            => new DashboardCardApiModel
            {
                DashboardCardId = dashboardCard.DashboardCardId,
                DashboardId = dashboardCard.DashboardId,
                CardId = dashboardCard.CardId,
                CardLayoutId = dashboardCard.CardLayoutId,
                Options = JsonConvert.DeserializeObject<OptionsApiModel>(dashboardCard.Options)
            };

        public class OptionsApiModel {
            public int Top { get; set; } = 1;
            public int Left { get; set; } = 1;
            public int Height { get; set; } = 1;
            public int Width { get; set; } = 1;
        }
    }
}
