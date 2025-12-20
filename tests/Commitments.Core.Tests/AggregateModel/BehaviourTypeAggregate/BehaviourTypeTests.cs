// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.BehaviourAggregate;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.BehaviourTypeAggregate;

public class BehaviourTypeTests
{
    [Fact]
    public void BehaviourType_ShouldInitializeWithEmptyBehavioursCollection()
    {
        // Arrange & Act
        var behaviourType = new BehaviourType();

        // Assert
        behaviourType.Behaviours.Should().NotBeNull();
        behaviourType.Behaviours.Should().BeEmpty();
    }

    [Fact]
    public void BehaviourType_ShouldSetBehaviourTypeId()
    {
        // Arrange
        var behaviourType = new BehaviourType();
        var id = Guid.NewGuid();

        // Act
        behaviourType.BehaviourTypeId = id;

        // Assert
        behaviourType.BehaviourTypeId.Should().Be(id);
    }

    [Fact]
    public void BehaviourType_ShouldSetName()
    {
        // Arrange
        var behaviourType = new BehaviourType();
        var name = "Test Behaviour Type";

        // Act
        behaviourType.Name = name;

        // Assert
        behaviourType.Name.Should().Be(name);
    }

    [Fact]
    public void BehaviourType_ShouldAddBehaviour()
    {
        // Arrange
        var behaviourType = new BehaviourType();
        var behaviour = new Behaviour { BehaviourId = Guid.NewGuid(), Name = "Test" };

        // Act
        behaviourType.Behaviours.Add(behaviour);

        // Assert
        behaviourType.Behaviours.Should().HaveCount(1);
        behaviourType.Behaviours.Should().Contain(behaviour);
    }

    [Fact]
    public void BehaviourType_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var behaviourType = new BehaviourType();
        var now = DateTime.UtcNow;

        behaviourType.CreatedOn = now;
        behaviourType.LastModifiedOn = now;
        behaviourType.IsDeleted = true;

        // Assert
        behaviourType.CreatedOn.Should().Be(now);
        behaviourType.LastModifiedOn.Should().Be(now);
        behaviourType.IsDeleted.Should().BeTrue();
    }
}
