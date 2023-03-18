// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.BehaviourAggregate;
using Commitments.Core.AggregateModel.ProfileAggregate;

namespace Commitments.Core.AggregateModel.ActivityAggregate;

public class Activity : BaseEntity
{
    public Guid ActivityId { get; set; }
    public Guid ProfileId { get; set; }
    public Guid BehaviourId { get; set; }
    public DateTime PerformedOn { get; set; }
    public string Description { get; set; }
    public Profile Profile { get; set; }
    public Behaviour Behaviour { get; set; }
}

