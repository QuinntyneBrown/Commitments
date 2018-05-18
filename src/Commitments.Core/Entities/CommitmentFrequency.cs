using System.Collections.Generic;

namespace Commitments.Core.Entities
{
    public class CommitmentFrequency
    {
        public int CommitmentFrequencyId { get; set; }           
		public int Frequency { get; set; }
        public bool IsDesirable { get; set; }
        public int FrequencyTypeId { get; set; }
        public int CommitmentId { get; set; }
        public Commitment Commitment { get; set; }
        public FrequencyType FrequencyType { get; set; }
    }
}
