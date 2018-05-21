using System.ComponentModel.DataAnnotations.Schema;

namespace Commitments.Core.Entities
{
    public class CommitmentPreCondition
    {
        public int CommitmentPreConditionId { get; set; }           
        public string Name { get; set; }  
        [ForeignKey("Commitment")]
        public int CommitmentId { get; set; }
        public Commitment Commitment { get; set; }
    }
}
