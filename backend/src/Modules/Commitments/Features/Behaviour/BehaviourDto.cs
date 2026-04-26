// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Features.BehaviourType;
using BehaviourEntity = Commitments.Domain.BehaviourAggregate.Behaviour;

namespace Commitments.Features.Behaviour;

public class BehaviourDto
{
    public Guid BehaviourId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public Guid BehaviourTypeId { get; set; }
    public bool IsDesired { get; set; }

    public BehaviourTypeDto BehaviourType { get; set; }
    public static BehaviourDto FromBehaviour(BehaviourEntity behaviour)
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