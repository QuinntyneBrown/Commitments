// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;
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
using Commitments.Core.AggregateModel.UserAggregate;
using Commitments.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Commitments.Infrastructure.Tests.Data;

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
        // Assert
        _context.Activities.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveBehavioursDbSet()
    {
        // Assert
        _context.Behaviours.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveBehaviourTypesDbSet()
    {
        // Assert
        _context.BehaviourTypes.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveCommitmentsDbSet()
    {
        // Assert
        _context.Commitments.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveCommitmentFrequenciesDbSet()
    {
        // Assert
        _context.CommitmentFrequencies.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveFrequenciesDbSet()
    {
        // Assert
        _context.Frequencies.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveFrequencyTypesDbSet()
    {
        // Assert
        _context.FrequencyTypes.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveCardsDbSet()
    {
        // Assert
        _context.Cards.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveCardLayoutsDbSet()
    {
        // Assert
        _context.CardLayouts.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveDashboardsDbSet()
    {
        // Assert
        _context.Dashboards.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveDashboardCardsDbSet()
    {
        // Assert
        _context.DashboardCards.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveProfilesDbSet()
    {
        // Assert
        _context.Profiles.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveUsersDbSet()
    {
        // Assert
        _context.Users.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentsDbContext_ShouldHaveDigitalAssetsDbSet()
    {
        // Assert
        _context.DigitalAssets.Should().NotBeNull();
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveActivity()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            PerformedOn = DateTime.UtcNow,
            Description = "Test Activity"
        };

        // Act
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedActivity = await _context.Activities.FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId);
        retrievedActivity.Should().NotBeNull();
        retrievedActivity!.Description.Should().Be("Test Activity");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveBehaviour()
    {
        // Arrange
        var behaviour = new Behaviour
        {
            BehaviourId = Guid.NewGuid(),
            Name = "Test Behaviour",
            Slug = "test-behaviour",
            Description = "A test behaviour",
            BehaviourTypeId = Guid.NewGuid()
        };

        // Act
        _context.Behaviours.Add(behaviour);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedBehaviour = await _context.Behaviours.FirstOrDefaultAsync(b => b.BehaviourId == behaviour.BehaviourId);
        retrievedBehaviour.Should().NotBeNull();
        retrievedBehaviour!.Name.Should().Be("Test Behaviour");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveBehaviourType()
    {
        // Arrange
        var behaviourType = new BehaviourType
        {
            BehaviourTypeId = Guid.NewGuid(),
            Name = "Test Behaviour Type"
        };

        // Act
        _context.BehaviourTypes.Add(behaviourType);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedBehaviourType = await _context.BehaviourTypes.FirstOrDefaultAsync(bt => bt.BehaviourTypeId == behaviourType.BehaviourTypeId);
        retrievedBehaviourType.Should().NotBeNull();
        retrievedBehaviourType!.Name.Should().Be("Test Behaviour Type");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveCommitment()
    {
        // Arrange
        var commitment = new Commitment
        {
            CommitmentId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid()
        };

        // Act
        _context.Commitments.Add(commitment);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedCommitment = await _context.Commitments.FirstOrDefaultAsync(c => c.CommitmentId == commitment.CommitmentId);
        retrievedCommitment.Should().NotBeNull();
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveFrequency()
    {
        // Arrange
        var frequency = new Frequency
        {
            FrequencyId = Guid.NewGuid(),
            FrequencyTypeId = Guid.NewGuid(),
            IsDesirable = true
        };

        // Act
        _context.Frequencies.Add(frequency);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedFrequency = await _context.Frequencies.FirstOrDefaultAsync(f => f.FrequencyId == frequency.FrequencyId);
        retrievedFrequency.Should().NotBeNull();
        retrievedFrequency!.IsDesirable.Should().BeTrue();
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveFrequencyType()
    {
        // Arrange
        var frequencyType = new FrequencyType
        {
            FrequencyTypeId = Guid.NewGuid(),
            Name = "Daily"
        };

        // Act
        _context.FrequencyTypes.Add(frequencyType);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedFrequencyType = await _context.FrequencyTypes.FirstOrDefaultAsync(ft => ft.FrequencyTypeId == frequencyType.FrequencyTypeId);
        retrievedFrequencyType.Should().NotBeNull();
        retrievedFrequencyType!.Name.Should().Be("Daily");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveCard()
    {
        // Arrange
        var card = new Card
        {
            CardId = Guid.NewGuid(),
            Name = "Test Card",
            Description = "Test Description"
        };

        // Act
        _context.Cards.Add(card);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedCard = await _context.Cards.FirstOrDefaultAsync(c => c.CardId == card.CardId);
        retrievedCard.Should().NotBeNull();
        retrievedCard!.Name.Should().Be("Test Card");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveDashboard()
    {
        // Arrange
        var dashboard = new Dashboard("Test Dashboard");
        dashboard.DashboardId = Guid.NewGuid();

        // Act
        _context.Dashboards.Add(dashboard);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedDashboard = await _context.Dashboards.FirstOrDefaultAsync(d => d.DashboardId == dashboard.DashboardId);
        retrievedDashboard.Should().NotBeNull();
        retrievedDashboard!.Name.Should().Be("Test Dashboard");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveUser()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = "testuser",
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com"
        };

        // Act
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
        retrievedUser.Should().NotBeNull();
        retrievedUser!.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveProfile()
    {
        // Arrange
        var profile = new Profile
        {
            ProfileId = Guid.NewGuid()
        };

        // Act
        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedProfile = await _context.Profiles.FirstOrDefaultAsync(p => p.ProfileId == profile.ProfileId);
        retrievedProfile.Should().NotBeNull();
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldAddAndRetrieveDigitalAsset()
    {
        // Arrange
        var digitalAsset = new DigitalAsset
        {
            DigitalAssetId = Guid.NewGuid(),
            Name = "test-image.png",
            ContentType = "image/png",
            Bytes = new byte[] { 1, 2, 3 }
        };

        // Act
        _context.DigitalAssets.Add(digitalAsset);
        await _context.SaveChangesAsync();

        // Assert
        var retrievedDigitalAsset = await _context.DigitalAssets.FirstOrDefaultAsync(da => da.DigitalAssetId == digitalAsset.DigitalAssetId);
        retrievedDigitalAsset.Should().NotBeNull();
        retrievedDigitalAsset!.Name.Should().Be("test-image.png");
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldSetCreatedOnWhenSaving()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            PerformedOn = DateTime.UtcNow,
            Description = "Test Activity"
        };

        // Act
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        // Assert
        activity.CreatedOn.Should().NotBe(default);
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldSetLastModifiedOnWhenSaving()
    {
        // Arrange
        var activity = new Activity
        {
            ActivityId = Guid.NewGuid(),
            ProfileId = Guid.NewGuid(),
            BehaviourId = Guid.NewGuid(),
            PerformedOn = DateTime.UtcNow,
            Description = "Test Activity"
        };

        // Act
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        // Assert
        activity.LastModifiedOn.Should().NotBe(default);
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldSoftDeleteOnRemove()
    {
        // Arrange
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

        // Act
        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();

        // Assert
        // Due to query filters, we need to ignore the filter to check
        var deletedActivity = await _context.Activities.IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.ActivityId == activity.ActivityId);
        deletedActivity.Should().NotBeNull();
        deletedActivity!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task CommitmentsDbContext_QueryFilter_ShouldExcludeDeletedActivities()
    {
        // Arrange
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

        // Act
        var activities = await _context.Activities.ToListAsync();

        // Assert
        activities.Should().NotContain(a => a.ActivityId == activity.ActivityId);
    }

    [Fact]
    public async Task CommitmentsDbContext_ShouldUpdateLastModifiedOnWhenModifying()
    {
        // Arrange
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
        await Task.Delay(10); // Small delay to ensure different timestamp

        // Act
        activity.Description = "Updated Description";
        await _context.SaveChangesAsync();

        // Assert
        activity.LastModifiedOn.Should().BeAfter(originalModifiedOn);
    }
}
