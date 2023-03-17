using System.Collections.Generic;


namespace Commitments.Core.AggregateModel;

public class BehaviourType: BaseEntity
{
    public int BehaviourTypeId { get; set; }
    public string Name { get; set; }
    public ICollection<Behaviour> Behaviours { get; set; }
    = new HashSet<Behaviour>();
}
