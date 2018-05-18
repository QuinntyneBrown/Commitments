using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commitments.Core.Entities
{
    public class Commitment: BaseEntity
    {
        public int CommitmentId { get; set; }           
        [ForeignKey("Behaviour")]
        public int BehaviourId { get; set; }
        [ForeignKey("Profile")]
        public int ProfileId { get; set; }
        public ICollection<CommitmentFrequency> CommitmentFrequencies { get; set; } 
            = new HashSet<CommitmentFrequency>();
        public Behaviour Behaviour { get; set; }
        public Profile Profile { get; set; }
        public ICollection<CommitmentPreCondition> CommitmentPreConditions { get; set; } 
            = new HashSet<CommitmentPreCondition>();
    }
}
