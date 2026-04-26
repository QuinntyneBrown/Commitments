using Dashboard.Features.DashboardCard;

namespace Dashboard.Features.Dashboard;

public class DashboardDto
{
    public Guid DashboardId { get; set; }
    public string Name { get; set; }
    public Guid? ProfileId { get; set; }
    public List<DashboardCardDto> DashboardCards { get; set; }
}
