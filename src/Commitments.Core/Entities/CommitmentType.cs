using System.Collections.Generic;

namespace Commitments.Core.Entities
{
    public class CommitmentType
    {
        public int CommitmentTypeId { get; set; }
        public string Type { get; set; }
        public ICollection<Commitment> Commitments { get; set; }
        = new HashSet<Commitment>();
    }
}
