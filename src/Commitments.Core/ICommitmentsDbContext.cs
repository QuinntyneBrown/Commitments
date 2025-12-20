// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
using Commitments.Core.AggregateModel.ActivityAggregate;
using Commitments.Core.AggregateModel.BehaviourAggregate;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate;
using Commitments.Core.AggregateModel.CommitmentAggregate;
using Commitments.Core.AggregateModel.CardAggregate;
using Commitments.Core.AggregateModel.CardLayoutAggregate;
using Commitments.Core.AggregateModel.DashboardAggregate;
using Commitments.Core.AggregateModel.DashboardCardAggregate;
using Commitments.Core.AggregateModel.FrequencyAggregate;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate;
using Commitments.Core.AggregateModel.UserAggregate;
using Commitments.Core.AggregateModel.DigitalAssetAggregate;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;


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
    DbSet<Card> Cards { get; }
    DbSet<CardLayout> CardLayouts { get; }
    DbSet<Dashboard> Dashboards { get; }
    DbSet<DashboardCard> DashboardCards { get; }
    DbSet<Commitment> ProfileCommitments { get; }
    DbSet<Profile> Profiles { get; }
    DbSet<User> Users { get; }
    DbSet<DigitalAsset> DigitalAssets { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}