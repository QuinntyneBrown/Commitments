// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Api.Features.Commitment;
using System;

namespace Commitments.Api.Features.Achievement;

public class AchievementDto
{
    public Guid AchievementId { get; set; }
    public CommitmentDto Commitment { get; set; }
}