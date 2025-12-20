// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CommitmentAggregate;
using System;

namespace Commitments.Core.Model.AchievementAggregate;

public class AchievementDto
{
    public Guid AchievementId { get; set; }
    public CommitmentDto Commitment { get; set; }
}