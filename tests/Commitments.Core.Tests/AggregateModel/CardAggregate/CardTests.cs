// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.CardAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.CardAggregate;

public class CardTests
{
    [Fact]
    public void Card_ShouldSetCardId()
    {
        // Arrange
        var card = new Card();
        var id = Guid.NewGuid();

        // Act
        card.CardId = id;

        // Assert
        card.CardId.Should().Be(id);
    }

    [Fact]
    public void Card_ShouldSetName()
    {
        // Arrange
        var card = new Card();
        var name = "Test Card";

        // Act
        card.Name = name;

        // Assert
        card.Name.Should().Be(name);
    }

    [Fact]
    public void Card_ShouldSetDescription()
    {
        // Arrange
        var card = new Card();
        var description = "This is a test card description";

        // Act
        card.Description = description;

        // Assert
        card.Description.Should().Be(description);
    }

    [Fact]
    public void Card_ShouldAllowNullName()
    {
        // Arrange
        var card = new Card();

        // Act
        card.Name = null!;

        // Assert
        card.Name.Should().BeNull();
    }

    [Fact]
    public void Card_ShouldAllowNullDescription()
    {
        // Arrange
        var card = new Card();

        // Act
        card.Description = null!;

        // Assert
        card.Description.Should().BeNull();
    }

    [Fact]
    public void Card_ShouldSetAllProperties()
    {
        // Arrange
        var card = new Card();
        var id = Guid.NewGuid();
        var name = "Test Card";
        var description = "Test Description";

        // Act
        card.CardId = id;
        card.Name = name;
        card.Description = description;

        // Assert
        card.CardId.Should().Be(id);
        card.Name.Should().Be(name);
        card.Description.Should().Be(description);
    }
}
