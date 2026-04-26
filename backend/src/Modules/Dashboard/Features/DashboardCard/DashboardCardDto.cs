using Dashboard.Features.Card;
using Dashboard.Features.CardLayout;
using Newtonsoft.Json.Linq;

namespace Dashboard.Features.DashboardCard;

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
