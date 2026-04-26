// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.FrequencyAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.CommitmentAggregate;

public class CommitmentFrequencyTests
{
    [Fact]
    public void CommitmentFrequency_ShouldSetCommitmentFrequencyId()
    {
        // Arrange
        var commitmentFrequency = new CommitmentFrequency();
        var id = Guid.NewGuid();

        // Act
        commitmentFrequency.CommitmentFrequencyId = id;

        // Assert
        commitmentFrequency.CommitmentFrequencyId.Should().Be(id);
    }

    [Fact]
    public void CommitmentFrequency_ShouldSetCommitmentId()
    {
        // Arrange
        var commitmentFrequency = new CommitmentFrequency();
        var commitmentId = Guid.NewGuid();

        // Act
        commitmentFrequency.CommitmentId = commitmentId;

        // Assert
        commitmentFrequency.CommitmentId.Should().Be(commitmentId);
    }

    [Fact]
    public void CommitmentFrequency_ShouldSetFrequencyId()
    {
        // Arrange
        var commitmentFrequency = new CommitmentFrequency();
        var frequencyId = Guid.NewGuid();

        // Act
        commitmentFrequency.FrequencyId = frequencyId;

        // Assert
        commitmentFrequency.FrequencyId.Should().Be(frequencyId);
    }

    [Fact]
    public void CommitmentFrequency_ShouldAllowNullCommitmentId()
    {
        // Arrange
        var commitmentFrequency = new CommitmentFrequency();

        // Act
        commitmentFrequency.CommitmentId = null;

        // Assert
        commitmentFrequency.CommitmentId.Should().BeNull();
    }

    [Fact]
    public void CommitmentFrequency_ShouldAllowNullFrequencyId()
    {
        // Arrange
        var commitmentFrequency = new CommitmentFrequency();

        // Act
        commitmentFrequency.FrequencyId = null;

        // Assert
        commitmentFrequency.FrequencyId.Should().BeNull();
    }

    [Fact]
    public void CommitmentFrequency_ShouldSetCommitmentNavigation()
    {
        // Arrange
        var commitmentFrequency = new CommitmentFrequency();
        var commitment = new Commitment { CommitmentId = Guid.NewGuid() };

        // Act
        commitmentFrequency.Commitment = commitment;

        // Assert
        commitmentFrequency.Commitment.Should().Be(commitment);
    }

    [Fact]
    public void CommitmentFrequency_ShouldSetFrequencyNavigation()
    {
        // Arrange
        var commitmentFrequency = new CommitmentFrequency();
        var frequency = new Frequency { FrequencyId = Guid.NewGuid() };

        // Act
        commitmentFrequency.Frequency = frequency;

        // Assert
        commitmentFrequency.Frequency.Should().Be(frequency);
    }
}
