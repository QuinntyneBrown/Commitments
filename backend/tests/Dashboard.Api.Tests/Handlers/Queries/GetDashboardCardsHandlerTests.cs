using Commitments.Testing.Common;
using Dashboard.Features.DashboardCard;
using Dashboard.Data;
using Dashboard.Domain.CardAggregate;
using Dashboard.Domain.CardLayoutAggregate;
using Dashboard.Domain.DashboardCardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Queries;

public class GetDashboardCardsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAllDashboardCards()
    {
        // Arrange
        var card = new Card { CardId = Guid.NewGuid(), Name = "Card 1", Description = "Desc" };
        var cardLayout = new CardLayout("Layout 1", "Layout Desc") { CardLayoutId = Guid.NewGuid() };
        var dashboardCards = new List<DashboardCard>
        {
            new DashboardCard
            {
                DashboardCardId = Guid.NewGuid(),
                DashboardId = Guid.NewGuid(),
                CardId = card.CardId,
                CardLayoutId = cardLayout.CardLayoutId,
                Card = card,
                CardLayout = cardLayout
            },
            new DashboardCard
            {
                DashboardCardId = Guid.NewGuid(),
                DashboardId = Guid.NewGuid(),
                CardId = Guid.NewGuid(),
                CardLayoutId = Guid.NewGuid()
            }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboardCards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new GetDashboardCardsRequestHandler(mockContext.Object);
        var request = new GetDashboardCardsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DashboardCards.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoDashboardCards_ShouldReturnEmptyList()
    {
        // Arrange
        var dashboardCards = new List<DashboardCard>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboardCards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new GetDashboardCardsRequestHandler(mockContext.Object);
        var request = new GetDashboardCardsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DashboardCards.Should().BeEmpty();
    }
}
