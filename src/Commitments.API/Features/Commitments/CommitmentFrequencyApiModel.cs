using Commitments.Api.Features.Frequencies;
using Commitments.Core.Entities;

namespace Commitments.Api.Features.Commitments
{
    public class CommitmentFrequencyApiModel
    {        
        public int CommitmentFrequencyId { get; set; }
        public string Name { get; set; }
        public int? FrequencyId { get; set; }
        public FrequencyApiModel Frequency { get; set; }
        public static CommitmentFrequencyApiModel FromCommitmentFrequency(CommitmentFrequency commitmentFrequency)
        {
            var model = new CommitmentFrequencyApiModel();
            model.CommitmentFrequencyId = commitmentFrequency.CommitmentFrequencyId;
            model.FrequencyId = commitmentFrequency.FrequencyId;
            model.Frequency = FrequencyApiModel.FromFrequency(commitmentFrequency.Frequency);
            return model;
        }
    }
}
