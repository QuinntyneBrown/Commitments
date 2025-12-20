// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.BehaviourTypeAggregate;

namespace Commitments.Core.Model.BehaviourTypeAggregate;

public class BehaviourTypeDto
{
    public Guid BehaviourTypeId { get; set; }
    public string Name { get; set; }

    public static BehaviourTypeDto FromBehaviourType(BehaviourType behaviourType)
    {
        var model = new BehaviourTypeDto();
        model.BehaviourTypeId = behaviourType.BehaviourTypeId;
        model.Name = behaviourType.Name;
        return model;
    }
}