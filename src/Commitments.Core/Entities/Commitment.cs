namespace Commitments.Core.Entities
{
    public class Commitment: BaseEntity
    {
        public int CommitmentId { get; set; }           
		public string Name { get; set; }
        public string Description { get; set; }
        public  int? CommitmentFrequencyId { get; set; }
        public int? CommitmentTypeId { get; set; }
    }
}
