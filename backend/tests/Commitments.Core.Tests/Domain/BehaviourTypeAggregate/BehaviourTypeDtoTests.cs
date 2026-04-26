// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Features.BehaviourType;
using Commitments.Domain.BehaviourTypeAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Domain.Tests.AggregateModel.BehaviourTypeAggregate;

public class BehaviourTypeDtoTests
{
    [Fact]
    public void FromBehaviourType_ShouldMapAllProperties()
    {
        // Arrange
        var behaviourType = new BehaviourType
        {
            BehaviourTypeId = Guid.NewGuid(),
            Name = "Test Behaviour Type"
        };

        // Act
        var dto = BehaviourTypeDto.FromBehaviourType(behaviourType);

        // Assert
        dto.BehaviourTypeId.Should().Be(behaviourType.BehaviourTypeId);
        dto.Name.Should().Be(behaviourType.Name);
    }

    [Fact]
    public void BehaviourTypeDto_ShouldSetProperties()
    {
        // Arrange
        var dto = new BehaviourTypeDto();
        var behaviourTypeId = Guid.NewGuid();
        var name = "Test Name";

        // Act
        dto.BehaviourTypeId = behaviourTypeId;
        dto.Name = name;

        // Assert
        dto.BehaviourTypeId.Should().Be(behaviourTypeId);
        dto.Name.Should().Be(name);
    }
}
