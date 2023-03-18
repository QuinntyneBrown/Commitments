// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.BehaviourAggregate;
using System;


namespace Commitments.Core.AggregateModel.ActivityAggregate;

public class ActivityDto
{
    public Guid ActivityId { get; set; }
    public Guid ProfileId { get; set; }
    public Guid BehaviourId { get; set; }
    public DateTime PerformedOn { get; set; }
    public string Description { get; set; }
    public BehaviourDto Behaviour { get; set; }
    public static ActivityDto FromActivity(Activity activity)
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

