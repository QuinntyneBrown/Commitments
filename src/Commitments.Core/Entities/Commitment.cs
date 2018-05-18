using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commitments.Core.Entities
{
    public class Commitment
    {
        public int CommitmentId { get; set; }           
		public string Name { get; set; }
        [ForeignKey("Behaviour")]
        public int BehaviourId { get; set; }
        [ForeignKey("Profile")]
        public int ProfileId { get; set; }
        [ForeignKey("CommitmentFrequency")]
        public int CommitmentFrequencyId { get; set; }
        public int CommitmentFailFrequencyId { get; set; }
        public CommitmentFrequency CommitmentFrequency { get; set; }
        public CommitmentFailFrequency CommitmentFailFrequency { get; set; }
        public Behaviour Behaviour { get; set; }
        public Profile Profile { get; set; }
        public ICollection<CommitmentPreCondition> CommitmentPreConditions { get; set; } 
            = new HashSet<CommitmentPreCondition>();
    }
}
