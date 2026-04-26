using Dashboard.Core.Model.DashboardCardAggregate;

namespace Dashboard.Core.Model.DashboardAggregate;

public class DashboardEntity
{
    public DashboardEntity(string name, Guid? profileId = null)
    {
        Name = name;
        ProfileId = profileId;
        DashboardCards = new List<DashboardCard>();
    }

    public Guid DashboardId { get; set; }
    public string Name { get; set; }
    public Guid? ProfileId { get; set; }
    public List<DashboardCard> DashboardCards { get; set; }
}
