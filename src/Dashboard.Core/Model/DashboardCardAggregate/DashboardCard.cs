using Dashboard.Core.Model.CardAggregate;
using Dashboard.Core.Model.CardLayoutAggregate;
using Dashboard.Core.Model.DashboardAggregate;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Core.Model.DashboardCardAggregate;

public class DashboardCard
{
    public Guid DashboardCardId { get; set; }
    [ForeignKey("Dashboard")]
    public Guid DashboardId { get; set; }
    [ForeignKey("Card")]
    public Guid CardId { get; set; }
    [ForeignKey("CardLayout")]
    public Guid CardLayoutId { get; set; }
    public DashboardEntity Dashboard { get; set; }
    public Card Card { get; set; }
    public CardLayout CardLayout { get; set; }
    public JObject Options { get; set; }
}
