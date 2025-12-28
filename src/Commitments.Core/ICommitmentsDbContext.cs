// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model;
using Commitments.Core.Model.ActivityAggregate;
using Commitments.Core.Model.BehaviourAggregate;
using Commitments.Core.Model.BehaviourTypeAggregate;
using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.CardAggregate;
using Commitments.Core.Model.CardLayoutAggregate;
using Commitments.Core.Model.DashboardAggregate;
using Commitments.Core.Model.DashboardCardAggregate;
using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;
using Commitments.Core.Model.UserAggregate;
using Commitments.Core.Model.DigitalAssetAggregate;
using Commitments.Core.Model.ProfileAggregate;
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