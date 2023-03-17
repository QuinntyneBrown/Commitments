using Commitments.Api.Features.Behaviours;
using Commitments.Core.AggregateModel;
using System;


namespace Commitments.Api.Features.Activities;

public class ActivityDto
{        
    public int ActivityId { get; set; }        
    public int ProfileId { get; set; }
    public int BehaviourId { get; set; }
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
