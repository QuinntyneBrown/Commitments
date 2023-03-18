// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.ActivityAggregate;
using Commitments.Core.AggregateModel.BehaviourAggregate;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate;
using Commitments.Core.AggregateModel.CardAggregate;
using Commitments.Core.AggregateModel.CardLayoutAggregate;
using Commitments.Core.AggregateModel.CommitmentAggregate;
using Commitments.Core.AggregateModel.DashboardAggregate;
using Commitments.Core.AggregateModel.DashboardCardAggregate;
using Commitments.Core.AggregateModel.DigitalAssetAggregate;
using Commitments.Core.AggregateModel.FrequencyAggregate;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate;
using Commitments.Core.AggregateModel.NoteAggregate;
using Commitments.Core.AggregateModel.ProfileAggregate;
using Commitments.Core.AggregateModel.TagAggregate;
using Commitments.Core.AggregateModel.ToDoAggregate;
using Commitments.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Commitments.Infrastructure.Data;

public class CommitmentsDbContext : DbContext, ICommimentsDbContext
{
    public CommitmentsDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Activity> Activities { get; private set; }
    public DbSet<Behaviour> Behaviours { get; private set; }
    public DbSet<BehaviourType> BehaviourTypes { get; private set; }
    public DbSet<Card> Cards { get; private set; }
    public DbSet<CardLayout> CardLayouts { get; private set; }
    public DbSet<Commitment> Commitments { get; private set; }
    public DbSet<Dashboard> Dashboards { get; private set; }
    public DbSet<DashboardCard> DashboardCards { get; private set; }
    public DbSet<DigitalAsset> DigitalAssets { get; private set; }
    public DbSet<Frequency> Frequencies { get; private set; }
    public DbSet<FrequencyType> FrequencyTypes { get; private set; }
    public DbSet<Note> Notes { get; private set; }
    public DbSet<Profile> Profiles { get; private set; }
    public DbSet<Commitment> ProfileCommitments { get; private set; }
    public DbSet<Tag> Tags { get; private set; }
    public DbSet<ToDo> ToDos { get; private set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        ChangeTracker.DetectChanges();

        foreach (var entity in ChangeTracker.Entries()
            .Where(e => e.Entity is ILoggable && ((e.State == EntityState.Added || (e.State == EntityState.Modified))))
            .Select(x => x.Entity as ILoggable))
        {
            var isNew = entity.CreatedOn == default(DateTime);
            entity.CreatedOn = isNew ? DateTime.UtcNow : entity.CreatedOn;
            entity.LastModifiedOn = DateTime.UtcNow;
        }

        foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted))
        {
            item.State = EntityState.Modified;
            item.CurrentValues["IsDeleted"] = true;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Commitments");

        modelBuilder.Entity<DigitalAsset>(b =>
        {
            b.Property(t => t.DigitalAssetId)
            .HasDefaultValueSql("newsequentialid()");
        });

        modelBuilder.Entity<Activity>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Behaviour>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<BehaviourType>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Card>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<CardLayout>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Commitment>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Dashboard>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<DashboardCard>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Frequency>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<FrequencyType>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Note>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<NoteTag>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Profile>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<Tag>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<ToDo>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<NoteTag>()
            .HasOne(nt => nt.Note)
            .WithMany(n => n.NoteTags)
            .HasForeignKey(nt => nt.NoteId);

        modelBuilder.Entity<NoteTag>()
            .HasOne(nt => nt.Tag)
            .WithMany(t => t.NoteTags)
            .HasForeignKey(nt => nt.TagId);

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

