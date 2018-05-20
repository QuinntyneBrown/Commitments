using System;

namespace Commitments.Core.Entities
{
    public class Activity: BaseEntity
    {
        public int ActivityId { get; set; }           
		public int ProfileId { get; set; }
        public int BehaviourId { get; set; }
        public DateTime PerformedOn { get; set; }
        public string Description { get; set; }
        public Profile Profile { get; set; }
        public Behaviour Behaviour { get; set; }
    }
}
