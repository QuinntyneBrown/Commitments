using Commitments.Testing.Common;
using Dashboard.Features.Card;
using Dashboard.Data;
using FluentAssertions;
using Moq;
using Xunit;
using CardModel = Dashboard.Domain.CardAggregate.Card;

namespace Dashboard.Api.Tests.Handlers.Queries;

public class GetCardsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAllCards()
    {
        // Arrange
        var cards = new List<CardModel>
        {
            new CardModel { CardId = Guid.NewGuid(), Name = "Card 1", Description = "Desc 1" },
            new CardModel { CardId = Guid.NewGuid(), Name = "Card 2", Description = "Desc 2" }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(cards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Cards).Returns(mockDbSet.Object);

        var handler = new GetCardsRequestHandler(mockContext.Object);
        var request = new GetCardsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Cards.Should().HaveCount(2);
        result.Cards[0].Name.Should().Be("Card 1");
        result.Cards[1].Name.Should().Be("Card 2");
    }

    [Fact]
    public async Task Handle_WhenNoCards_ShouldReturnEmptyList()
    {
        // Arrange
        var cards = new List<CardModel>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(cards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Cards).Returns(mockDbSet.Object);

        var handler = new GetCardsRequestHandler(mockContext.Object);
        var request = new GetCardsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Cards.Should().BeEmpty();
    }
}
