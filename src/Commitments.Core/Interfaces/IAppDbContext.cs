// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Core.Interfaces;

public interface ICommimentsDbContext: IDisposable
{
    DbSet<Activity> Activities { get; }
    DbSet<Behaviour> Behaviours { get; }
    DbSet<BehaviourType> BehaviourTypes { get; }
    DbSet<Card> Cards { get; }
    DbSet<CardLayout> CardLayouts { get; }
    DbSet<Commitment> Commitments { get; }
    DbSet<Dashboard> Dashboards { get; }
    DbSet<DashboardCard> DashboardCards { get; }
    DbSet<DigitalAsset> DigitalAssets { get; }
    DbSet<Frequency> Frequencies { get; }
    DbSet<FrequencyType> FrequencyTypes { get; }
    DbSet<Note> Notes { get; }
    DbSet<PodCast> PodCasts { get; }
    DbSet<Profile> Profiles { get; }
    DbSet<Commitment> ProfileCommitments { get; }
    DbSet<Tag> Tags { get; }
    DbSet<ToDo> ToDos { get; }
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

