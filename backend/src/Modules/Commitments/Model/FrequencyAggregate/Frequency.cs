// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;

namespace Commitments.Core.Model.FrequencyAggregate;

public class BaseFrequency : BaseEntity
{
    public Guid Frequency { get; set; }
}

public class Frequency : BaseFrequency
{
    public Guid FrequencyId { get; set; }
    public Guid FrequencyTypeId { get; set; }
    public bool IsDesirable { get; set; }
    public FrequencyType FrequencyType { get; set; }
    public ICollection<CommitmentFrequency> CommitmentFrequencies { get; set; }
    = new HashSet<CommitmentFrequency>();
}