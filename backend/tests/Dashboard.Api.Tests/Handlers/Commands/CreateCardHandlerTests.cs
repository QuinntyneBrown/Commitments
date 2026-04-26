using Commitments.Testing.Common;
using Dashboard.Api.Features.Card;
using Dashboard.Core;
using FluentAssertions;
using Moq;
using Xunit;
using CardModel = Dashboard.Core.Model.CardAggregate.Card;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class CreateCardHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateCardAndSave()
    {
        // Arrange
        var cards = new List<CardModel>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(cards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Cards).Returns(mockDbSet.Object);

        var handler = new CreateCardRequestHandler(mockContext.Object);
        var request = new CreateCardRequest
        {
            Card = new CardDto { Name = "Test Card", Description = "Test Description" }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Card.Should().NotBeNull();
        result.Card.Name.Should().Be("Test Card");
        result.Card.Description.Should().Be("Test Description");
        cards.Should().HaveCount(1);
        cards[0].Name.Should().Be("Test Card");
        cards[0].Description.Should().Be("Test Description");
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
