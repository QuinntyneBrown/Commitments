// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.ActivityAggregate;
using Commitments.Core.Model.BehaviourAggregate;
using Commitments.Core.Model.BehaviourTypeAggregate;
using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Core;

public interface ICommitmentsDbContext : IDisposable
{
    DbSet<Activity> Activities { get; }
    DbSet<Behaviour> Behaviours { get; }
    DbSet<BehaviourType> BehaviourTypes { get; }
    DbSet<Commitment> Commitments { get; }
    DbSet<CommitmentFrequency> CommitmentFrequencies { get; }
    DbSet<Frequency> Frequencies { get; }
    DbSet<FrequencyType> FrequencyTypes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
