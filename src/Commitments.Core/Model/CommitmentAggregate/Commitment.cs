// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.BehaviourAggregate;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Commitments.Core.Model.CommitmentAggregate;

public class Commitment : BaseEntity
{
    public Guid CommitmentId { get; set; }

    [ForeignKey("Behaviour")]
    public Guid BehaviourId { get; set; }

    public Guid ProfileId { get; set; }

    public ICollection<CommitmentFrequency> CommitmentFrequencies { get; set; }
        = new HashSet<CommitmentFrequency>();

    public Behaviour Behaviour { get; set; }

    public ICollection<CommitmentPreCondition> CommitmentPreConditions { get; set; }
        = new HashSet<CommitmentPreCondition>();
}