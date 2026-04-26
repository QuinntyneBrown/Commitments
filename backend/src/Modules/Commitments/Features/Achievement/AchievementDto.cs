// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.CommitmentAggregate;
using Commitments.Features.Commitment;
using System;

namespace Commitments.Features.Achievement;

public class AchievementDto
{
    public Guid AchievementId { get; set; }
    public CommitmentDto Commitment { get; set; }
}