using Dashboard.Api.Features.Card;
using FluentAssertions;
using Xunit;
using CardModel = Dashboard.Core.Model.CardAggregate.Card;

namespace Dashboard.Api.Tests.Extensions;

public class CardExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var card = new CardModel
        {
            CardId = cardId,
            Name = "Test Card",
            Description = "Test Description"
        };

        // Act
        var dto = card.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.CardId.Should().Be(cardId);
        dto.Name.Should().Be("Test Card");
        dto.Description.Should().Be("Test Description");
    }

    [Fact]
    public void ToDto_ShouldReturnNewDtoInstance()
    {
        // Arrange
        var card = new CardModel
        {
            CardId = Guid.NewGuid(),
            Name = "Card",
            Description = "Desc"
        };

        // Act
        var dto1 = card.ToDto();
        var dto2 = card.ToDto();

        // Assert
        dto1.Should().NotBeSameAs(dto2);
        dto1.CardId.Should().Be(dto2.CardId);
        dto1.Name.Should().Be(dto2.Name);
    }

    [Fact]
    public void ToDto_WithEmptyValues_ShouldMapCorrectly()
    {
        // Arrange
        var card = new CardModel
        {
            CardId = Guid.Empty,
            Name = "",
            Description = ""
        };

        // Act
        var dto = card.ToDto();

        // Assert
        dto.Should().NotBeNull();
        dto.CardId.Should().Be(Guid.Empty);
        dto.Name.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
    }
}
