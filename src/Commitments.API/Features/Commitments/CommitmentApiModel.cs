using Commitments.Core.Entities;

namespace Commitments.API.Features.Commitments
{
    public class CommitmentApiModel
    {        
        public int CommitmentId { get; set; }
        public string Name { get; set; }

        public static CommitmentApiModel FromCommitment(Commitment commitment)
        {
            var model = new CommitmentApiModel();
            model.CommitmentId = commitment.CommitmentId;
            model.Name = commitment.Name;
            return model;
        }
    }
}
