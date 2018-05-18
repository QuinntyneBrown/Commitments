using Commitments.Core.Entities;

namespace Commitments.API.Features.Behaviours
{
    public class BehaviourApiModel
    {        
        public int BehaviourId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public static BehaviourApiModel FromBehaviour(Behaviour behaviour)
            => new BehaviourApiModel
            {
                BehaviourId = behaviour.BehaviourId,
                Name = behaviour.Name,
                Slug = behaviour.Slug,
                Description = behaviour.Description
            };
    }
}
