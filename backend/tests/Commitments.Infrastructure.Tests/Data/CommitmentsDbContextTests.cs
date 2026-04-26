// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Domain.FrequencyAggregate;
using Commitments.Domain.FrequencyTypeAggregate;
using Commitments.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Data.Tests.Data;

public class CommitmentsDbContextTests : IDisposable
{
    private readonly CommitmentsDbContext _context;

    public CommitmentsDbContextTests()
    {
        var options = new DbContextOptionsBuilder<CommitmentsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new CommitmentsDbContext(options);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveActivitiesDbSet()
    {
        _context.Activities.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveBehavioursDbSet()
    {
        _context.Behaviours.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveBehaviourTypesDbSet()
    {
        _context.BehaviourTypes.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveCommitmentsDbSet()
    {
        _context.Commitments.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveCommitmentFrequenciesDbSet()
    {
        _context.CommitmentFrequencies.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveFrequenciesDbSet()
    {
        _context.Frequencies.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveFrequencyTypesDbSet()
    {
        _context.FrequencyTypes.Should().NotBeNull();
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveActivity()
    {
        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            PerformedOn = DateTime.UtcNow,
            Description = "Test Activity"
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        var retrievedActivity = await _context.Activities.FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId);
        retrievedActivity.Should().NotBeNull();
        retrievedActivity!.Description.Should().Be("Test Activity");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveBehaviour()
    {
        var behaviour = new Behaviour
        {
            BehaviourId = Guid.NewGuid(),
            Name = "Test Behaviour",
            Slug = "test-behaviour",
            Description = "A test behaviour",
            BehaviourTypeId = Guid.NewGuid()
        };

        _context.Behaviours.Add(behaviour);
        await _context.SaveChangesAsync();

        var retrievedBehaviour = await _context.Behaviours.FirstOrDefaultAsync(b => b.BehaviourId == behaviour.BehaviourId);
        retrievedBehaviour.Should().NotBeNull();
        retrievedBehaviour!.Name.Should().Be("Test Behaviour");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveBehaviourType()
    {
        var behaviourType = new BehaviourType
        {
            BehaviourTypeId = Guid.NewGuid(),
            Name = "Test Behaviour Type"
        };

        _context.BehaviourTypes.Add(behaviourType);
        await _context.SaveChangesAsync();

        var retrievedBehaviourType = await _context.BehaviourTypes.FirstOrDefaultAsync(bt => bt.BehaviourTypeId == behaviourType.BehaviourTypeId);
        retrievedBehaviourType.Should().NotBeNull();
        retrievedBehaviourType!.Name.Should().Be("Test Behaviour Type");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveCommitment()
    {
        var commitment = new Commitment
        {
            CommitmentId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid()
        };

        _context.Commitments.Add(commitment);
        await _context.SaveChangesAsync();

        var retrievedCommitment = await _context.Commitments.FirstOrDefaultAsync(c => c.CommitmentId == commitment.CommitmentId);
        retrievedCommitment.Should().NotBeNull();
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveFrequency()
    {
        var frequency = new Frequency
        {
            FrequencyId = Guid.NewGuid(),
            FrequencyTypeId = Guid.NewGuid(),
            IsDesirable = true
        };

        _context.Frequencies.Add(frequency);
        await _context.SaveChangesAsync();

        var retrievedFrequency = await _context.Frequencies.FirstOrDefaultAsync(f => f.FrequencyId == frequency.FrequencyId);
        retrievedFrequency.Should().NotBeNull();
        retrievedFrequency!.IsDesirable.Should().BeTrue();
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveFrequencyType()
    {
        var frequencyType = new FrequencyType
        {
            FrequencyTypeId = Guid.NewGuid(),
            Name = "Daily"
        };

        _context.FrequencyTypes.Add(frequencyType);
        await _context.SaveChangesAsync();

        var retrievedFrequencyType = await _context.FrequencyTypes.FirstOrDefaultAsync(ft => ft.FrequencyTypeId == frequencyType.FrequencyTypeId);
        retrievedFrequencyType.Should().NotBeNull();
        retrievedFrequencyType!.Name.Should().Be("Daily");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldSetCreatedOnWhenSaving()
    {
        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            PerformedOn = DateTime.UtcNow,
            Description = "Test Activity"
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        activity.CreatedOn.Should().NotBe(default);
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldSetLastModifiedOnWhenSaving()
    {
        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            PerformedOn = DateTime.UtcNow,
            Description = "Test Activity"
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        activity.LastModifiedOn.Should().NotBe(default);
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldSoftDeleteOnRemove()
    {
        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            PerformedOn = DateTime.UtcNow,
            Description = "Test Activity"
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();

        var deletedActivity = await _context.Activities.IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId);
        deletedActivity.Should().NotBeNull();
        deletedActivity!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task CommitmentsDbContext_QueryFilter_ShouldExcludeDeletedActivities()
    {
        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            PerformedOn = DateTime.UtcNow,
            Description = "Test Activity",
            IsDeleted = true
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        var activities = await _context.Activities.ToListAsync();

        activities.Should().NotContain(a => a.ActivityId == activity.ActivityId);
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldUpdateLastModifiedOnWhenModifying()
    {
        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            PerformedOn = DateTime.UtcNow,
            Description = "Test Activity"
        };

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        var originalModifiedOn = activity.LastModifiedOn;
        await Task.Delay(10);

        activity.Description = "Updated Description";
        await _context.SaveChangesAsync();

        activity.LastModifiedOn.Should().BeAfter(originalModifiedOn);
    }
}
