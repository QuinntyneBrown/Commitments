// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Domain.FrequencyAggregate;
using Commitments.Domain.FrequencyTypeAggregate;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Data;

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
