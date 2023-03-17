using Commitments.Api.Features.BehaviourTypes;
using Commitments.Core.Entities;


namespace Commitments.Api.Features.Behaviours;

public class BehaviourDto
{        
    public int BehaviourId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public int BehaviourTypeId { get; set; }
    public bool IsDesired { get; set; }

    public BehaviourTypeDto BehaviourType { get; set; }
    public static BehaviourDto FromBehaviour(Behaviour behaviour)
        => new BehaviourDto
        {
            BehaviourId = behaviour.BehaviourId,
            Name = behaviour.Name,
            Slug = behaviour.Slug,                
            Description = behaviour.Description,
            BehaviourTypeId = behaviour.BehaviourTypeId,
            BehaviourType = BehaviourTypeDto.FromBehaviourType(behaviour.BehaviourType)
        };
}
