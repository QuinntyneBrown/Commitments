// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.CommitmentAggregate;
using Commitments.Core.Model.FrequencyAggregate;
using Commitments.Core.Model.FrequencyTypeAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.FrequencyAggregate;

public class FrequencyTests
{
    [Fact]
    public void Frequency_ShouldInitializeWithEmptyCommitmentFrequenciesCollection()
    {
        // Arrange & Act
        var frequency = new Frequency();

        // Assert
        frequency.CommitmentFrequencies.Should().NotBeNull();
        frequency.CommitmentFrequencies.Should().BeEmpty();
    }

    [Fact]
    public void Frequency_ShouldSetFrequencyId()
    {
        // Arrange
        var frequency = new Frequency();
        var id = Guid.NewGuid();

        // Act
        frequency.FrequencyId = id;

        // Assert
        frequency.FrequencyId.Should().Be(id);
    }

    [Fact]
    public void Frequency_ShouldSetFrequencyTypeId()
    {
        // Arrange
        var frequency = new Frequency();
        var frequencyTypeId = Guid.NewGuid();

        // Act
        frequency.FrequencyTypeId = frequencyTypeId;

        // Assert
        frequency.FrequencyTypeId.Should().Be(frequencyTypeId);
    }

    [Fact]
    public void Frequency_ShouldSetIsDesirable()
    {
        // Arrange
        var frequency = new Frequency();

        // Act
        frequency.IsDesirable = true;

        // Assert
        frequency.IsDesirable.Should().BeTrue();
    }

    [Fact]
    public void Frequency_ShouldSetFrequencyType()
    {
        // Arrange
        var frequency = new Frequency();
        var frequencyType = new FrequencyType
        {
            FrequencyTypeId = Guid.NewGuid(),
            Name = "Daily"
        };

        // Act
        frequency.FrequencyType = frequencyType;

        // Assert
        frequency.FrequencyType.Should().Be(frequencyType);
    }

    [Fact]
    public void Frequency_ShouldAddCommitmentFrequency()
    {
        // Arrange
        var frequency = new Frequency();
        var commitmentFrequency = new CommitmentFrequency
        {
            CommitmentFrequencyId = Guid.NewGuid()
        };

        // Act
        frequency.CommitmentFrequencies.Add(commitmentFrequency);

        // Assert
        frequency.CommitmentFrequencies.Should().HaveCount(1);
        frequency.CommitmentFrequencies.Should().Contain(commitmentFrequency);
    }

    [Fact]
    public void Frequency_ShouldInheritFromBaseFrequency()
    {
        // Arrange
        var frequency = new Frequency();
        var baseFrequencyValue = Guid.NewGuid();

        // Act
        frequency.Frequency = baseFrequencyValue;

        // Assert
        frequency.Frequency.Should().Be(baseFrequencyValue);
    }

    [Fact]
    public void Frequency_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var frequency = new Frequency();
        var now = DateTime.UtcNow;

        frequency.CreatedOn = now;
        frequency.LastModifiedOn = now;
        frequency.IsDeleted = true;

        // Assert
        frequency.CreatedOn.Should().Be(now);
        frequency.LastModifiedOn.Should().Be(now);
        frequency.IsDeleted.Should().BeTrue();
    }
}
