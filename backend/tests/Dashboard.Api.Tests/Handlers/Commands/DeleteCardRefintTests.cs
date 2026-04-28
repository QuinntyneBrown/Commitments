using Commitments.Testing.Common;
using Dashboard.Data;
using Dashboard.Domain.CardAggregate;
using Dashboard.Domain.DashboardCardAggregate;
using Dashboard.Features.Card;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class DeleteCardRefintTests
{
    [Fact]
    public async Task Handle_WhenReferencedByDashboardCard_ThrowsBadHttpRequestException(/* design 16 Slice B */)
    {
        var cardId = Guid.NewGuid();
        var existing = new Card { CardId = cardId, Name = "x" };
        var referencingTile = new DashboardCard { DashboardCardId = Guid.NewGuid(), CardId = cardId };

        var mockContext = MockDashboardDbContextFactory.Create();
        var cardSet = MockDbSetFactory.CreateMockDbSet(new List<Card> { existing });
        cardSet.Setup(d => d.FindAsync(cardId)).ReturnsAsync(existing);
        mockContext.Setup(c => c.Cards).Returns(cardSet.Object);
        mockContext.Setup(c => c.DashboardCards).Returns(MockDbSetFactory.CreateMockDbSet(new List<DashboardCard> { referencingTile }).Object);

        var handler = new DeleteCardRequestHandler(mockContext.Object);

        var act = async () => await handler.Handle(new DeleteCardRequest { CardId = cardId }, CancellationToken.None);

        await act.Should().ThrowAsync<BadHttpRequestException>()
            .Where(e => e.StatusCode == 400 && e.Message.Contains("Cannot delete"));

        cardSet.Verify(d => d.Remove(It.IsAny<Card>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenNotReferenced_RemovesCard()
    {
        var cardId = Guid.NewGuid();
        var existing = new Card { CardId = cardId, Name = "x" };

        var mockContext = MockDashboardDbContextFactory.Create();
        var cardSet = MockDbSetFactory.CreateMockDbSet(new List<Card> { existing });
        cardSet.Setup(d => d.FindAsync(cardId)).ReturnsAsync(existing);
        mockContext.Setup(c => c.Cards).Returns(cardSet.Object);
        mockContext.Setup(c => c.DashboardCards).Returns(MockDbSetFactory.CreateMockDbSet(new List<DashboardCard>()).Object);

        var handler = new DeleteCardRequestHandler(mockContext.Object);

        await handler.Handle(new DeleteCardRequest { CardId = cardId }, CancellationToken.None);

        cardSet.Verify(d => d.Remove(existing), Times.Once);
    }
}
