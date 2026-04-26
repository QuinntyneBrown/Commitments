// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Features.Behaviour;
using Commitments.Domain.BehaviourAggregate;
using Commitments.Domain.BehaviourTypeAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Domain.Tests.AggregateModel.BehaviourAggregate;

public class BehaviourDtoTests
{
    [Fact]
    public void FromBehaviour_ShouldMapAllProperties()
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

        // Act
        var dto = BehaviourDto.FromBehaviour(behaviour);

        // Assert
        dto.BehaviourId.Should().Be(behaviour.BehaviourId);
        dto.Name.Should().Be(behaviour.Name);
        dto.Slug.Should().Be(behaviour.Slug);
        dto.Description.Should().Be(behaviour.Description);
        dto.BehaviourTypeId.Should().Be(behaviour.BehaviourTypeId);
        dto.BehaviourType.Should().NotBeNull();
        dto.BehaviourType.Name.Should().Be(behaviourType.Name);
    }

    [Fact]
    public void BehaviourDto_ShouldSetProperties()
    {
        // Arrange
        var dto = new BehaviourDto();
        var behaviourId = Guid.NewGuid();
        var behaviourTypeId = Guid.NewGuid();
        var name = "Test Name";
        var slug = "test-slug";
        var description = "Test Description";

        // Act
        dto.BehaviourId = behaviourId;
        dto.BehaviourTypeId = behaviourTypeId;
        dto.Name = name;
        dto.Slug = slug;
        dto.Description = description;
        dto.IsDesired = true;

        // Assert
        dto.BehaviourId.Should().Be(behaviourId);
        dto.BehaviourTypeId.Should().Be(behaviourTypeId);
        dto.Name.Should().Be(name);
        dto.Slug.Should().Be(slug);
        dto.Description.Should().Be(description);
        dto.IsDesired.Should().BeTrue();
    }
}
