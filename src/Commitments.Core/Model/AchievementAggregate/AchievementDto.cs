// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.CommitmentAggregate;
using System;

namespace Commitments.Core.AggregateModel.AchievementAggregate;

public class AchievementDto
{
    public Guid AchievementId { get; set; }
    public CommitmentDto Commitment { get; set; }
}