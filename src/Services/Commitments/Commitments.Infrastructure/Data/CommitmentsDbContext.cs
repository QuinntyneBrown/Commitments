// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Core.AggregateModel;
using Commitments.Core.AggregateModel.ActivityAggregate;
using Commitments.Core.AggregateModel.BehaviourAggregate;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate;
using Commitments.Core.AggregateModel.CommitmentAggregate;
using Commitments.Core.AggregateModel.FrequencyAggregate;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;


namespace Commitments.Infrastructure.Data;

public class CommitmentsDbContext : DbContext, ICommitmentsDbContext
{
    public CommitmentsDbContext(DbContextOptions options)
        : base(options) {
        SavingChanges += OnSavingChanges;
    }

    public DbSet<Activity> Activities { get; private set; }
    public DbSet<Behaviour> Behaviours { get; private set; }
    public DbSet<BehaviourType> BehaviourTypes { get; private set; }
    public DbSet<Commitment> Commitments { get; private set; }
    public DbSet<Frequency> Frequencies { get; private set; }
    public DbSet<FrequencyType> FrequencyTypes { get; private set; }
    public DbSet<Profile> Profiles { get; private set; }
    public DbSet<Commitment> ProfileCommitments { get; private set; }

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

