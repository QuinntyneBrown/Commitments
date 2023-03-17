// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;


namespace Commitments.Core.AggregateModel;

public class BaseFrequency: BaseEntity {
    public int Frequency { get; set; }
}

public class Frequency: BaseFrequency
{
    public int FrequencyId { get; set; }
    public int FrequencyTypeId { get; set; }
    public bool IsDesirable { get; set; }
    public FrequencyType FrequencyType { get; set; }
    public ICollection<CommitmentFrequency> CommitmentFrequencies { get; set; }
    = new HashSet<CommitmentFrequency>();
}

