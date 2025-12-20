// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel;

public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        entity.CreatedOn.Should().Be(default);
        entity.LastModifiedOn.Should().Be(default);
        entity.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void BaseEntity_ShouldSetCreatedOn()
    {
        // Arrange
        var entity = new TestEntity();
        var createdDate = DateTime.UtcNow;

        // Act
        entity.CreatedOn = createdDate;

        // Assert
        entity.CreatedOn.Should().Be(createdDate);
    }

    [Fact]
    public void BaseEntity_ShouldSetLastModifiedOn()
    {
        // Arrange
        var entity = new TestEntity();
        var modifiedDate = DateTime.UtcNow;

        // Act
        entity.LastModifiedOn = modifiedDate;

        // Assert
        entity.LastModifiedOn.Should().Be(modifiedDate);
    }

    [Fact]
    public void BaseEntity_ShouldSetIsDeleted()
    {
        // Arrange
        var entity = new TestEntity();

        // Act
        entity.IsDeleted = true;

        // Assert
        entity.IsDeleted.Should().BeTrue();
    }

    private class TestEntity : BaseEntity
    {
    }
}
