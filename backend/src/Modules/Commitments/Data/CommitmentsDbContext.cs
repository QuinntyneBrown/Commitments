// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Data;
using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Domain.FrequencyAggregate;
using Commitments.Domain.FrequencyTypeAggregate;
using Commitments.Shared;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Data;

public class CommitmentsDbContext : BaseDbContext, ICommitmentsDbContext
{
    public CommitmentsDbContext(DbContextOptions<CommitmentsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Activity> Activities { get; private set; }
    public DbSet<Behaviour> Behaviours { get; private set; }
    public DbSet<BehaviourType> BehaviourTypes { get; private set; }
    public DbSet<Commitment> Commitments { get; private set; }
    public DbSet<CommitmentFrequency> CommitmentFrequencies { get; private set; }
    public DbSet<Frequency> Frequencies { get; private set; }
    public DbSet<FrequencyType> FrequencyTypes { get; private set; }

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
