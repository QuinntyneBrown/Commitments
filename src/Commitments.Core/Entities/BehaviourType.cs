using System.Collections.Generic;

namespace Commitments.Core.Entities
{
    public class BehaviourType
    {
        public int BehaviourTypeId { get; set; }
        public string Type { get; set; }
        public ICollection<Behaviour> Behaviours { get; set; }
        = new HashSet<Behaviour>();
    }
}
