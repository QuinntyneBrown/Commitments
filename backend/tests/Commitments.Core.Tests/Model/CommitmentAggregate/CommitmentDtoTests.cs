// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Features.Commitment;
using Commitments.Core.Model.BehaviourAggregate;
using Commitments.Core.Model.BehaviourTypeAggregate;
using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.CommitmentAggregate;

public class CommitmentDtoTests
{
    [Fact]
    public void CommitmentDto_ShouldInitializeWithEmptyCommitmentFrequencies()
    {
        // Arrange & Act
        var dto = new CommitmentDto();

        // Assert
        dto.CommitmentFrequencies.Should().NotBeNull();
        dto.CommitmentFrequencies.Should().BeEmpty();
    }

    [Fact]
    public void FromCommitment_ShouldMapAllProperties()
    {
        // Arrange
        var behaviourType = new BehaviourType
        {
            BehaviourTypeId = Guid.NewGuid(),
            Name = "Test Behaviour Type"
        };

        var behaviour = new Behaviour
        {
            BehaviourId = Guid.NewGuid(),
            Name = "Test Behaviour",
            Slug = "test-behaviour",
            Description = "A test behaviour",
            BehaviourTypeId = behaviourType.BehaviourTypeId,
            BehaviourType = behaviourType
        };

        var frequencyType = new FrequencyType
        {
            FrequencyTypeId = Guid.NewGuid(),
            Name = "Daily"
        };

        var frequency = new Frequency
        {
            FrequencyId = Guid.NewGuid(),
            FrequencyTypeId = frequencyType.FrequencyTypeId,
            FrequencyType = frequencyType,
            IsDesirable = true
        };

        var commitment = new Commitment
        {
            CommitmentId = Guid.NewGuid(),
            BehaviourId = behaviour.BehaviourId,
            ProfileId = Guid.NewGuid(),
            Behaviour = behaviour
        };

        commitment.CommitmentFrequencies.Add(new CommitmentFrequency
        {
            CommitmentFrequencyId = Guid.NewGuid(),
            CommitmentId = commitment.CommitmentId,
            FrequencyId = frequency.FrequencyId,
            Frequency = frequency
        });

        // Act
        var dto = CommitmentDto.FromCommitment(commitment);

        // Assert
        dto.CommitmentId.Should().Be(commitment.CommitmentId);
        dto.BehaviourId.Should().Be(commitment.BehaviourId);
        dto.ProfileId.Should().Be(commitment.ProfileId);
        dto.Behaviour.Should().NotBeNull();
        dto.Behaviour.Name.Should().Be(behaviour.Name);
        dto.CommitmentFrequencies.Should().HaveCount(1);
    }

    [Fact]
    public void CommitmentDto_ShouldSetProperties()
    {
        // Arrange
        var dto = new CommitmentDto();
        var commitmentId = Guid.NewGuid();
        var behaviourId = Guid.NewGuid();
        var profileId = Guid.NewGuid();

        // Act
        dto.CommitmentId = commitmentId;
        dto.BehaviourId = behaviourId;
        dto.ProfileId = profileId;

        // Assert
        dto.CommitmentId.Should().Be(commitmentId);
        dto.BehaviourId.Should().Be(behaviourId);
        dto.ProfileId.Should().Be(profileId);
    }
}
