using Commitments.Core.Entities;

namespace Commitments.API.Features.CommitmentFrequencies
{
    public class CommitmentFrequencyApiModel
    {        
        public int CommitmentFrequencyId { get; set; }
        public int CommitmentId { get; set; }
        public int Frequency { get; set; }
        public bool IsDesirable { get; set; }
        public int FrequencyTypeId { get; set; }

        public static CommitmentFrequencyApiModel FromCommitmentFrequency(CommitmentFrequency commitmentFrequency)
            => new CommitmentFrequencyApiModel
            {
                CommitmentFrequencyId = commitmentFrequency.CommitmentFrequencyId,
                CommitmentId = commitmentFrequency.CommitmentId,
                Frequency = commitmentFrequency.Frequency,
                IsDesirable = commitmentFrequency.IsDesirable,
                FrequencyTypeId = commitmentFrequency.FrequencyTypeId
            };
    }
}
