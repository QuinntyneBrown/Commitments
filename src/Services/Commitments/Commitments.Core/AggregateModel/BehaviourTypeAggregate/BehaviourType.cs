// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Commitments.Core.AggregateModel.BehaviourAggregate;

namespace Commitments.Core.AggregateModel.BehaviourTypeAggregate;

public class BehaviourType : BaseEntity
{
    public Guid BehaviourTypeId { get; set; }
    public string Name { get; set; }
    public ICollection<Behaviour> Behaviours { get; set; }
    = new HashSet<Behaviour>();
}

