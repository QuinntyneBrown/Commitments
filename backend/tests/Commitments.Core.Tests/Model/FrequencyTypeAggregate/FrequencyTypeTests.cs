// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.FrequencyTypeAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.FrequencyTypeAggregate;

public class FrequencyTypeTests
{
    [Fact]
    public void FrequencyType_ShouldSetFrequencyTypeId()
    {
        // Arrange
        var frequencyType = new FrequencyType();
        var id = Guid.NewGuid();

        // Act
        frequencyType.FrequencyTypeId = id;

        // Assert
        frequencyType.FrequencyTypeId.Should().Be(id);
    }

    [Fact]
    public void FrequencyType_ShouldSetName()
    {
        // Arrange
        var frequencyType = new FrequencyType();
        var name = "Daily";

        // Act
        frequencyType.Name = name;

        // Assert
        frequencyType.Name.Should().Be(name);
    }

    [Fact]
    public void FrequencyType_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var frequencyType = new FrequencyType();
        var now = DateTime.UtcNow;

        frequencyType.CreatedOn = now;
        frequencyType.LastModifiedOn = now;
        frequencyType.IsDeleted = true;

        // Assert
        frequencyType.CreatedOn.Should().Be(now);
        frequencyType.LastModifiedOn.Should().Be(now);
        frequencyType.IsDeleted.Should().BeTrue();
    }
}
