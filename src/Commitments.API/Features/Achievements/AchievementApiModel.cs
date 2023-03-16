using Commitments.Api.Features.Commitments;


namespace Commitments.Api.Features.Achievements;

public class AchievementApiModel
{        
    public int AchievementId { get; set; }
    public CommitmentApiModel Commitment { get; set; }
}
