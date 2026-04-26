using Dashboard.Domain.CardAggregate;
using Dashboard.Domain.CardLayoutAggregate;
using Dashboard.Domain.DashboardAggregate;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Domain.DashboardCardAggregate;

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
