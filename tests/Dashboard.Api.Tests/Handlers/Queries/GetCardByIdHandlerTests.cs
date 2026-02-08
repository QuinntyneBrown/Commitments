using Commitments.Testing.Common;
using Dashboard.Api.Features.Card;
using Dashboard.Core;
using FluentAssertions;
using Moq;
using Xunit;
using CardModel = Dashboard.Core.Model.CardAggregate.Card;

namespace Dashboard.Api.Tests.Handlers.Queries;

public class GetCardByIdHandlerTests
{
    [Fact]
    public async Task Handle_WhenCardExists_ShouldReturnCard()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var card = new CardModel { CardId = cardId, Name = "Test Card", Description = "Test Desc" };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardModel>();
        mockDbSet.Setup(d => d.FindAsync(cardId)).ReturnsAsync(card);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Cards).Returns(mockDbSet.Object);

        var handler = new GetCardByIdRequestHandler(mockContext.Object);
        var request = new GetCardByIdRequest { CardId = cardId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Card.Should().NotBeNull();
        result.Card!.CardId.Should().Be(cardId);
        result.Card.Name.Should().Be("Test Card");
        result.Card.Description.Should().Be("Test Desc");
    }

    [Fact]
    public async Task Handle_WhenCardNotFound_ShouldReturnNullCard()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardModel>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((CardModel?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Cards).Returns(mockDbSet.Object);

        var handler = new GetCardByIdRequestHandler(mockContext.Object);
        var request = new GetCardByIdRequest { CardId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Card.Should().BeNull();
    }
}
