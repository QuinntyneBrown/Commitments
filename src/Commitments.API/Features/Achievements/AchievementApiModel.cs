using Commitments.API.Features.Commitments;

namespace Commitments.API.Features.Achievements
{
    public class AchievementApiModel
    {        
        public int AchievementId { get; set; }
        public CommitmentApiModel Commitment { get; set; }
        
    }
}
