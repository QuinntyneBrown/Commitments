// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Features.Commitment;
using Commitments.Domain.CommitmentAggregate;
using Commitments.Domain.FrequencyAggregate;
using Commitments.Domain.FrequencyTypeAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Domain.Tests.AggregateModel.CommitmentAggregate;

public class CommitmentFrequencyDtoTests
{
    [Fact]
    public void FromCommitmentFrequency_ShouldMapAllProperties()
    {
        // Arrange
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

        var commitmentFrequency = new CommitmentFrequency
        {
            CommitmentFrequencyId = Guid.NewGuid(),
            FrequencyId = frequency.FrequencyId,
            Frequency = frequency
        };

        // Act
        var dto = CommitmentFrequencyDto.FromCommitmentFrequency(commitmentFrequency);

        // Assert
        dto.CommitmentFrequencyId.Should().Be(commitmentFrequency.CommitmentFrequencyId);
        dto.FrequencyId.Should().Be(commitmentFrequency.FrequencyId);
        dto.Frequency.Should().NotBeNull();
    }

    [Fact]
    public void CommitmentFrequencyDto_ShouldSetProperties()
    {
        // Arrange
        var dto = new CommitmentFrequencyDto();
        var commitmentFrequencyId = Guid.NewGuid();
        var frequencyId = Guid.NewGuid();
        var name = "Test Name";

        // Act
        dto.CommitmentFrequencyId = commitmentFrequencyId;
        dto.FrequencyId = frequencyId;
        dto.Name = name;

        // Assert
        dto.CommitmentFrequencyId.Should().Be(commitmentFrequencyId);
        dto.FrequencyId.Should().Be(frequencyId);
        dto.Name.Should().Be(name);
    }
}
