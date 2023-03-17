using Commitments.Api.Features.Commitments;


namespace Commitments.Api.Features.Achievements;

public class AchievementDto
{        
    public int AchievementId { get; set; }
    public CommitmentDto Commitment { get; set; }
}
