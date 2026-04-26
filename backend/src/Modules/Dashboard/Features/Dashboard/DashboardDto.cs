using Dashboard.Api.Features.DashboardCard;

namespace Dashboard.Api.Features.Dashboard;

public class DashboardDto
{
    public Guid DashboardId { get; set; }
    public string Name { get; set; }
    public Guid? ProfileId { get; set; }
    public List<DashboardCardDto> DashboardCards { get; set; }
}
