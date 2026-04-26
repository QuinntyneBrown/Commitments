using Commitments.Testing.Common;
using Dashboard.Features.Card;
using Dashboard.Data;
using FluentAssertions;
using Moq;
using Xunit;
using CardModel = Dashboard.Domain.CardAggregate.Card;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class DeleteCardHandlerTests
{
    [Fact]
    public async Task Handle_WhenCardExists_ShouldRemoveAndSave()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var card = new CardModel { CardId = cardId, Name = "ToDelete", Description = "Desc" };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardModel>();
        mockDbSet.Setup(d => d.FindAsync(cardId)).ReturnsAsync(card);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Cards).Returns(mockDbSet.Object);

        var handler = new DeleteCardRequestHandler(mockContext.Object);
        var request = new DeleteCardRequest { CardId = cardId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockDbSet.Verify(d => d.Remove(card), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCardNotFound_ShouldNotCallSave()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<CardModel>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((CardModel?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Cards).Returns(mockDbSet.Object);

        var handler = new DeleteCardRequestHandler(mockContext.Object);
        var request = new DeleteCardRequest { CardId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockDbSet.Verify(d => d.Remove(It.IsAny<CardModel>()), Times.Never);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
