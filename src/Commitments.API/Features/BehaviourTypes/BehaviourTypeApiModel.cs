using Commitments.Core.Entities;

namespace Commitments.API.Features.BehaviourTypes
{
    public class BehaviourTypeApiModel
    {        
        public int BehaviourTypeId { get; set; }
        public string Name { get; set; }

        public static BehaviourTypeApiModel FromBehaviourType(BehaviourType behaviourType)
        {
            var model = new BehaviourTypeApiModel();
            model.BehaviourTypeId = behaviourType.BehaviourTypeId;
            model.Name = behaviourType.Name;
            return model;
        }
    }
}
