// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations.Schema;


namespace Commitments.Core.Model.CommitmentAggregate;

public class CommitmentPreCondition
{
    public Guid CommitmentPreConditionId { get; set; }

    public string Name { get; set; }

    [ForeignKey("Commitment")]
    public Guid CommitmentId { get; set; }

    public Commitment Commitment { get; set; }
}