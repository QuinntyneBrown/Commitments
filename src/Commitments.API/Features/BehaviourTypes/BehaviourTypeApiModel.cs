using Commitments.Core.Entities;


namespace Commitments.Api.Features.BehaviourTypes;

public class BehaviourTypeDto
{        
    public int BehaviourTypeId { get; set; }
    public string Name { get; set; }

    public static BehaviourTypeDto FromBehaviourType(BehaviourType behaviourType)
    {
        var model = new BehaviourTypeDto();
        model.BehaviourTypeId = behaviourType.BehaviourTypeId;
        model.Name = behaviourType.Name;
        return model;
    }
}
