using Commitments.API.Features.Behaviours;
using Commitments.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Commitments.API.Features.Commitments
{
    public class CommitmentApiModel
    {        
        public int CommitmentId { get; set; }
        public int BehaviourId { get; set; }
        public int ProfileId { get; set; }
        public BehaviourApiModel Behaviour { get; set; }
        public ICollection<CommitmentFrequencyApiModel> CommitmentFrequencies { get; set; }
        = new HashSet<CommitmentFrequencyApiModel>();

        public static CommitmentApiModel FromCommitment(Commitment commitment)
        {
            var model = new CommitmentApiModel();
            model.CommitmentId = commitment.CommitmentId;
            model.BehaviourId = commitment.BehaviourId;
            model.ProfileId = commitment.ProfileId;
            model.Behaviour = BehaviourApiModel.FromBehaviour(commitment.Behaviour);
            model.CommitmentFrequencies = commitment.CommitmentFrequencies
                .Select(x => CommitmentFrequencyApiModel.FromCommitmentFrequency(x)).ToList();
            return model;
        }
    }
}
