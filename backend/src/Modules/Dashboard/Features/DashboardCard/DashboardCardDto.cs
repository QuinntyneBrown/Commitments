using Dashboard.Api.Features.Card;
using Dashboard.Api.Features.CardLayout;
using Newtonsoft.Json.Linq;

namespace Dashboard.Api.Features.DashboardCard;

public class DashboardCardDto
{
    public Guid DashboardCardId { get; set; }
    public Guid DashboardId { get; set; }
    public Guid CardId { get; set; }
    public Guid CardLayoutId { get; set; }
    public CardDto Card { get; set; }
    public CardLayoutDto CardLayout { get; set; }
    public JObject Options { get; set; }
}
