// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CommitmentAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.CommitmentAggregate;

public class CommitmentTests
{
    [Fact]
    public void Commitment_ShouldInitializeWithEmptyCollections()
    {
        // Arrange & Act
        var commitment = new Commitment();

        // Assert
        commitment.CommitmentFrequencies.Should().NotBeNull();
        commitment.CommitmentFrequencies.Should().BeEmpty();
        commitment.CommitmentPreConditions.Should().NotBeNull();
        commitment.CommitmentPreConditions.Should().BeEmpty();
    }

    [Fact]
    public void Commitment_ShouldSetCommitmentId()
    {
        // Arrange
        var commitment = new Commitment();
        var id = Guid.NewGuid();

        // Act
        commitment.CommitmentId = id;

        // Assert
        commitment.CommitmentId.Should().Be(id);
    }

    [Fact]
    public void Commitment_ShouldSetBehaviourId()
    {
        // Arrange
        var commitment = new Commitment();
        var behaviourId = Guid.NewGuid();

        // Act
        commitment.BehaviourId = behaviourId;

        // Assert
        commitment.BehaviourId.Should().Be(behaviourId);
    }

    [Fact]
    public void Commitment_ShouldSetProfileId()
    {
        // Arrange
        var commitment = new Commitment();
        var profileId = Guid.NewGuid();

        // Act
        commitment.ProfileId = profileId;

        // Assert
        commitment.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public void Commitment_ShouldAddCommitmentFrequency()
    {
        // Arrange
        var commitment = new Commitment();
        var commitmentFrequency = new CommitmentFrequency
        {
            CommitmentFrequencyId = Guid.NewGuid(),
            FrequencyId = Guid.NewGuid()
        };

        // Act
        commitment.CommitmentFrequencies.Add(commitmentFrequency);

        // Assert
        commitment.CommitmentFrequencies.Should().HaveCount(1);
        commitment.CommitmentFrequencies.Should().Contain(commitmentFrequency);
    }

    [Fact]
    public void Commitment_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var commitment = new Commitment();
        var now = DateTime.UtcNow;

        commitment.CreatedOn = now;
        commitment.LastModifiedOn = now;
        commitment.IsDeleted = true;

        // Assert
        commitment.CreatedOn.Should().Be(now);
        commitment.LastModifiedOn.Should().Be(now);
        commitment.IsDeleted.Should().BeTrue();
    }
}
