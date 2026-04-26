// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain;
using Commitments.Domain.ActivityAggregate;
using Commitments.Domain.BehaviourAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Domain.Tests.AggregateModel.ActivityAggregate;

public class ActivityTests
{
    [Fact]
    public void Activity_ShouldSetActivityId()
    {
        // Arrange
        var activity = new Activity();
        var id = Guid.NewGuid();

        // Act
        activity.ActivityId = id;

        // Assert
        activity.ActivityId.Should().Be(id);
    }

    [Fact]
    public void Activity_ShouldSetProfileId()
    {
        // Arrange
        var activity = new Activity();
        var profileId = Guid.NewGuid();

        // Act
        activity.ProfileId = profileId;

        // Assert
        activity.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public void Activity_ShouldSetBehaviourId()
    {
        // Arrange
        var activity = new Activity();
        var behaviourId = Guid.NewGuid();

        // Act
        activity.BehaviourId = behaviourId;

        // Assert
        activity.BehaviourId.Should().Be(behaviourId);
    }

    [Fact]
    public void Activity_ShouldSetPerformedOn()
    {
        // Arrange
        var activity = new Activity();
        var performedOn = DateTime.UtcNow;

        // Act
        activity.PerformedOn = performedOn;

        // Assert
        activity.PerformedOn.Should().Be(performedOn);
    }

    [Fact]
    public void Activity_ShouldSetDescription()
    {
        // Arrange
        var activity = new Activity();
        var description = "Test activity description";

        // Act
        activity.Description = description;

        // Assert
        activity.Description.Should().Be(description);
    }

    [Fact]
    public void Activity_ShouldSetBehaviour()
    {
        // Arrange
        var activity = new Activity();
        var behaviour = new Behaviour { BehaviourId = Guid.NewGuid() };

        // Act
        activity.Behaviour = behaviour;

        // Assert
        activity.Behaviour.Should().Be(behaviour);
    }

    [Fact]
    public void Activity_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var activity = new Activity();
        var now = DateTime.UtcNow;

        activity.CreatedOn = now;
        activity.LastModifiedOn = now;
        activity.IsDeleted = true;

        // Assert
        activity.CreatedOn.Should().Be(now);
        activity.LastModifiedOn.Should().Be(now);
        activity.IsDeleted.Should().BeTrue();
    }
}
