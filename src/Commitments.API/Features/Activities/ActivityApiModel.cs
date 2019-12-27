using Commitments.Api.Features.Behaviours;
using Commitments.Core.Entities;
using System;

namespace Commitments.Api.Features.Activities
{
    public class ActivityApiModel
    {        
        public int ActivityId { get; set; }        
        public int ProfileId { get; set; }
        public int BehaviourId { get; set; }
        public DateTime PerformedOn { get; set; }
        public string Description { get; set; }
        public BehaviourApiModel Behaviour { get; set; }
        public static ActivityApiModel FromActivity(Activity activity)
            => new ActivityApiModel
            {
                ActivityId = activity.ActivityId,
                ProfileId = activity.ProfileId,
                BehaviourId = activity.BehaviourId,
                PerformedOn = activity.PerformedOn,
                Description = activity.Description,
                Behaviour = BehaviourApiModel.FromBehaviour(activity.Behaviour)
            };
    }
}
