// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
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
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace Commitments.Infrastructure.Data;

public class CommitmentsDbContext : DbContext, ICommitmentsDbContext
{
    public CommitmentsDbContext(DbContextOptions options)
        : base(options)
    {
        SavingChanges += OnSavingChanges;
    }

    public DbSet<Activity> Activities { get; private set; }
    public DbSet<Behaviour> Behaviours { get; private set; }
    public DbSet<BehaviourType> BehaviourTypes { get; private set; }
    public DbSet<Commitment> Commitments { get; private set; }
    public DbSet<CommitmentFrequency> CommitmentFrequencies { get; private set; }
    public DbSet<Frequency> Frequencies { get; private set; }
    public DbSet<FrequencyType> FrequencyTypes { get; private set; }
    public DbSet<Card> Cards { get; private set; }
    public DbSet<CardLayout> CardLayouts { get; private set; }
    public DbSet<Dashboard> Dashboards { get; private set; }
    public DbSet<DashboardCard> DashboardCards { get; private set; }
    public DbSet<Profile> Profiles { get; private set; }
    public DbSet<Commitment> ProfileCommitments { get; private set; }
    public DbSet<User> Users { get; private set; }
    public DbSet<DigitalAsset> DigitalAssets { get; private set; }

    private void OnSavingChanges(object sender, SavingChangesEventArgs e)
    {
        foreach (var entity in ChangeTracker.Entries()
            .Where(e => e.Entity is ILoggable && e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity as ILoggable))
        {
            var isNew = entity.CreatedOn == default;
            entity.CreatedOn = isNew ? DateTime.UtcNow : entity.CreatedOn;
            entity.LastModifiedOn = DateTime.UtcNow;
        }

        foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
        {
            item.State = EntityState.Modified;
            item.CurrentValues["IsDeleted"] = true;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Commitments");

        modelBuilder.Entity<Activity>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Behaviour>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<BehaviourType>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Commitment>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Frequency>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<FrequencyType>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Profile>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<User>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<CommitmentFrequency>()
            .HasOne(nt => nt.Commitment)
            .WithMany(n => n.CommitmentFrequencies)
            .HasForeignKey(nt => nt.CommitmentId);

        modelBuilder.Entity<CommitmentFrequency>()
            .HasOne(nt => nt.Frequency)
            .WithMany(t => t.CommitmentFrequencies)
            .HasForeignKey(nt => nt.FrequencyId);

        base.OnModelCreating(modelBuilder);
    }
}