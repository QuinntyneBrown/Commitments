using System.Collections.Generic;

namespace Commitments.Core.Entities
{
    public class CommitmentFrequency
    {
        public int CommitmentFrequencyId { get; set; }           
		public string Name { get; set; }
        public int? FrequencyTypeId { get; set; }
        public FrequencyType FrequencyType { get; set; }
        public ICollection<Commitment> Commitments { get; set; }
            = new HashSet<Commitment>();
    }
}
