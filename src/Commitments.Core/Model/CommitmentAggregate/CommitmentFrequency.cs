// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Commitments.Core.Model.FrequencyAggregate;

namespace Commitments.Core.Model.CommitmentAggregate;

public class CommitmentFrequency
{
    public Guid CommitmentFrequencyId { get; set; }
    public Guid? CommitmentId { get; set; }
    public Guid? FrequencyId { get; set; }
    public Commitment Commitment { get; set; }
    public Frequency Frequency { get; set; }
}