
namespace Commitments.Core.AggregateModel;

public class CommitmentFrequency
{
    public int CommitmentFrequencyId { get; set; }
    public int? CommitmentId { get; set; }
    public int? FrequencyId { get; set; }
    public Commitment Commitment { get; set; }
    public Frequency Frequency { get; set; }
}
