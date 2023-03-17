// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;


namespace Commitments.Core.AggregateModel;

public class BehaviourType: BaseEntity
{
    public int BehaviourTypeId { get; set; }
    public string Name { get; set; }
    public ICollection<Behaviour> Behaviours { get; set; }
    = new HashSet<Behaviour>();
}

