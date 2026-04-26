// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.ActivityAggregate;
using Commitments.Features.Behaviour;
using System;
using ActivityEntity = Commitments.Domain.ActivityAggregate.Activity;


namespace Commitments.Features.Activity;

public class ActivityDto
{
    public Guid ActivityId { get; set; }
    public Guid ProfileId { get; set; }
    public Guid BehaviourId { get; set; }
    public DateTime PerformedOn { get; set; }
    public string Description { get; set; }
    public BehaviourDto Behaviour { get; set; }
    public static ActivityDto FromActivity(ActivityEntity activity)
        => new ActivityDto
        {
            ActivityId = activity.ActivityId,
            ProfileId = activity.ProfileId,
            BehaviourId = activity.BehaviourId,
            PerformedOn = activity.PerformedOn,
            Description = activity.Description,
            Behaviour = BehaviourDto.FromBehaviour(activity.Behaviour)
        };
}