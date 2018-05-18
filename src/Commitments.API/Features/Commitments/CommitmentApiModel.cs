using Commitments.Core.Entities;

namespace Commitments.API.Features.Commitments
{
    public class CommitmentApiModel
    {        
        public int CommitmentId { get; set; }
        public int BehaviourId { get; set; }
        public int ProfileId { get; set; }

        public static CommitmentApiModel FromCommitment(Commitment commitment)
        {
            var model = new CommitmentApiModel();
            model.CommitmentId = commitment.CommitmentId;
            model.BehaviourId = commitment.BehaviourId;
            model.ProfileId = commitment.ProfileId;
            return model;
        }
    }
}
