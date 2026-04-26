// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using Commitments.Domain.CommitmentAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Domain.Tests.AggregateModel.BehaviourAggregate;

public class BehaviourTests
{
    [Fact]
    public void Behaviour_ShouldInitializeWithEmptyCommitmentsCollection()
    {
        // Arrange & Act
        var behaviour = new Behaviour();

        // Assert
        behaviour.Commitments.Should().NotBeNull();
        behaviour.Commitments.Should().BeEmpty();
    }

    [Fact]
    public void Behaviour_ShouldSetBehaviourId()
    {
        // Arrange
        var behaviour = new Behaviour();
        var id = Guid.NewGuid();

        // Act
        behaviour.BehaviourId = id;

        // Assert
        behaviour.BehaviourId.Should().Be(id);
    }

    [Fact]
    public void Behaviour_ShouldSetName()
    {
        // Arrange
        var behaviour = new Behaviour();
        var name = "Test Behaviour";

        // Act
        behaviour.Name = name;

        // Assert
        behaviour.Name.Should().Be(name);
    }

    [Fact]
    public void Behaviour_ShouldSetSlug()
    {
        // Arrange
        var behaviour = new Behaviour();
        var slug = "test-behaviour";

        // Act
        behaviour.Slug = slug;

        // Assert
        behaviour.Slug.Should().Be(slug);
    }

    [Fact]
    public void Behaviour_ShouldSetDescription()
    {
        // Arrange
        var behaviour = new Behaviour();
        var description = "A test behaviour description";

        // Act
        behaviour.Description = description;

        // Assert
        behaviour.Description.Should().Be(description);
    }

    [Fact]
    public void Behaviour_ShouldSetBehaviourTypeId()
    {
        // Arrange
        var behaviour = new Behaviour();
        var behaviourTypeId = Guid.NewGuid();

        // Act
        behaviour.BehaviourTypeId = behaviourTypeId;

        // Assert
        behaviour.BehaviourTypeId.Should().Be(behaviourTypeId);
    }

    [Fact]
    public void Behaviour_ShouldSetBehaviourType()
    {
        // Arrange
        var behaviour = new Behaviour();
        var behaviourType = new BehaviourType
        {
            BehaviourTypeId = Guid.NewGuid(),
            Name = "Test Type"
        };

        // Act
        behaviour.BehaviourType = behaviourType;

        // Assert
        behaviour.BehaviourType.Should().Be(behaviourType);
    }

    [Fact]
    public void Behaviour_ShouldAddCommitment()
    {
        // Arrange
        var behaviour = new Behaviour();
        var commitment = new Commitment { CommitmentId = Guid.NewGuid() };

        // Act
        behaviour.Commitments.Add(commitment);

        // Assert
        behaviour.Commitments.Should().HaveCount(1);
        behaviour.Commitments.Should().Contain(commitment);
    }

    [Fact]
    public void Behaviour_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var behaviour = new Behaviour();
        var now = DateTime.UtcNow;

        behaviour.CreatedOn = now;
        behaviour.LastModifiedOn = now;
        behaviour.IsDeleted = true;

        // Assert
        behaviour.CreatedOn.Should().Be(now);
        behaviour.LastModifiedOn.Should().Be(now);
        behaviour.IsDeleted.Should().BeTrue();
    }
}
