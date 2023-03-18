// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
using Commitments.Core.AggregateModel.ActivityAggregate;
using Commitments.Core.AggregateModel.BehaviourAggregate;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate;
using Commitments.Core.AggregateModel.CommitmentAggregate;
using Commitments.Core.AggregateModel.FrequencyAggregate;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Core.Interfaces;

public interface ICommitmentsDbContext : IDisposable
{
    DbSet<Activity> Activities { get; }
    DbSet<Behaviour> Behaviours { get; }
    DbSet<BehaviourType> BehaviourTypes { get; }
    DbSet<Commitment> Commitments { get; }
    DbSet<Frequency> Frequencies { get; }
    DbSet<FrequencyType> FrequencyTypes { get; }
    DbSet<Commitment> ProfileCommitments { get; }
    DbSet<Profile> Profiles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

