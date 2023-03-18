// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.BehaviourAggregate;
using System.Collections.Generic;
using System.Linq;


namespace Commitments.Core.AggregateModel.CommitmentAggregate;

public class CommitmentDto
{
    public Guid CommitmentId { get; set; }
    public Guid BehaviourId { get; set; }
    public Guid ProfileId { get; set; }
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

