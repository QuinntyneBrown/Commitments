using Commitments.Testing.Common;
using Dashboard.Features.Card;
using Dashboard.Data;
using FluentAssertions;
using Moq;
using Xunit;
using CardModel = Dashboard.Domain.CardAggregate.Card;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class UpdateCardHandlerTests
{
    [Fact]
    public async Task Handle_WhenCardExists_ShouldUpdateAndSave()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var card = new CardModel { CardId = cardId, Name = "Original", Description = "Original Desc" };
        var cards = new List<CardModel> { card };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(cards);
        mockDbSet.Setup(d => d.FindAsync(cardId)).ReturnsAsync(card);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Cards).Returns(mockDbSet.Object);

        var handler = new UpdateCardRequestHandler(mockContext.Object);
        var request = new UpdateCardRequest
        {
            Card = new CardDto { CardId = cardId, Name = "Updated Card", Description = "Updated Desc" }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Card.Should().NotBeNull();
        result.Card.Name.Should().Be("Updated Card");
        result.Card.Description.Should().Be("Updated Desc");
        card.Name.Should().Be("Updated Card");
        card.Description.Should().Be("Updated Desc");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCardNotFound_ShouldReturnEmptyResponse()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardModel>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((CardModel?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Cards).Returns(mockDbSet.Object);

        var handler = new UpdateCardRequestHandler(mockContext.Object);
        var request = new UpdateCardRequest
        {
            Card = new CardDto { CardId = Guid.NewGuid(), Name = "Updated", Description = "Desc" }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Card.Should().BeNull();
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
