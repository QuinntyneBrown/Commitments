// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;


namespace Commitments.Core.AggregateModel;

public class Behaviour: BaseEntity
{
    public int BehaviourId { get; set; }           
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public int BehaviourTypeId { get; set; }
    public BehaviourType BehaviourType { get; set; }
    public ICollection<Commitment> Commitments { get; set; }
    = new HashSet<Commitment>();
}

