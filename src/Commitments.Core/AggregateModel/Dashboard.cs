using System.Collections.Generic;


namespace Commitments.Core.AggregateModel;

public class Dashboard: BaseEntity
{
    public int DashboardId { get; set; }           
    public string Name { get; set; }        
    public int ProfileId { get; set; }
    public ICollection<DashboardCard> DashboardCards { get; set; } 
        = new HashSet<DashboardCard>();
}
