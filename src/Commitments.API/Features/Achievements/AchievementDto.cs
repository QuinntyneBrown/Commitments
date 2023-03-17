// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Features.Commitments;


namespace Commitments.Api.Features.Achievements;

public class AchievementDto
{        
    public int AchievementId { get; set; }
    public CommitmentDto Commitment { get; set; }
}

