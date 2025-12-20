// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Commitments.Core.Model.BehaviourTypeAggregate;
using Commitments.Core.Model.CommitmentAggregate;

namespace Commitments.Core.Model.BehaviourAggregate;

public class Behaviour : BaseEntity
{
    public Guid BehaviourId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public Guid BehaviourTypeId { get; set; }
    public BehaviourType BehaviourType { get; set; }
    public ICollection<Commitment> Commitments { get; set; }
    = new HashSet<Commitment>();
}