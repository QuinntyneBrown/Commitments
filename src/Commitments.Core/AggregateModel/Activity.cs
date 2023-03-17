// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;


namespace Commitments.Core.AggregateModel;

public class Activity: BaseEntity
{
    public int ActivityId { get; set; }           
    public int ProfileId { get; set; }
    public int BehaviourId { get; set; }
    public DateTime PerformedOn { get; set; }
    public string Description { get; set; }
    public Profile Profile { get; set; }
    public Behaviour Behaviour { get; set; }
}

