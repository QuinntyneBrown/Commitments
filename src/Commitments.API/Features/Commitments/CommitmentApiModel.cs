using Commitments.Api.Features.Behaviours;
using Commitments.Core.Entities;
using System.Collections.Generic;
using System.Linq;


namespace Commitments.Api.Features.Commitments;

public class CommitmentDto
{        
    public int CommitmentId { get; set; }
    public int BehaviourId { get; set; }
    public int ProfileId { get; set; }
    public BehaviourDto Behaviour { get; set; }
    public ICollection<CommitmentFrequencyDto> CommitmentFrequencies { get; set; }
    = new HashSet<CommitmentFrequencyDto>();

    public static CommitmentDto FromCommitment(Commitment commitment)
    {
        var model = new CommitmentDto();
        model.CommitmentId = commitment.CommitmentId;
        model.BehaviourId = commitment.BehaviourId;
        model.ProfileId = commitment.ProfileId;
        model.Behaviour = BehaviourDto.FromBehaviour(commitment.Behaviour);
        model.CommitmentFrequencies = commitment.CommitmentFrequencies
            .Select(x => CommitmentFrequencyDto.FromCommitmentFrequency(x)).ToList();
        return model;
    }
}
