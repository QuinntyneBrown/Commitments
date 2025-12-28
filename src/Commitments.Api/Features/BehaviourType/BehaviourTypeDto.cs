// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.BehaviourTypeAggregate;
using BehaviourTypeEntity = Commitments.Core.Model.BehaviourTypeAggregate.BehaviourType;

namespace Commitments.Api.Features.BehaviourType;

public class BehaviourTypeDto
{
    public Guid BehaviourTypeId { get; set; }
    public string Name { get; set; }

    public static BehaviourTypeDto FromBehaviourType(BehaviourTypeEntity behaviourType)
    {
        var model = new BehaviourTypeDto();
        model.BehaviourTypeId = behaviourType.BehaviourTypeId;
        model.Name = behaviourType.Name;
        return model;
    }
}