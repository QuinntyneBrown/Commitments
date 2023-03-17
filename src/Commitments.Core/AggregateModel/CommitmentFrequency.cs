// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace Commitments.Core.AggregateModel;

public class CommitmentFrequency
{
    public int CommitmentFrequencyId { get; set; }
    public int? CommitmentId { get; set; }
    public int? FrequencyId { get; set; }
    public Commitment Commitment { get; set; }
    public Frequency Frequency { get; set; }
}

