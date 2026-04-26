using Commitments.Testing.Common;
using Dashboard.Api.Features.DashboardCard;
using Dashboard.Core;
using Dashboard.Core.Model.CardAggregate;
using Dashboard.Core.Model.CardLayoutAggregate;
using Dashboard.Core.Model.DashboardCardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Queries;

public class GetDashboardCardByIdHandlerTests
{
    [Fact]
    public async Task Handle_WhenDashboardCardExists_ShouldReturnDashboardCard()
    {
        // Arrange
        var dashboardCardId = Guid.NewGuid();
        var card = new Card { CardId = Guid.NewGuid(), Name = "Card", Description = "Desc" };
        var cardLayout = new CardLayout("Layout", "Layout Desc") { CardLayoutId = Guid.NewGuid() };
        var dashboardCard = new DashboardCard
        {
            DashboardCardId = dashboardCardId,
            DashboardId = Guid.NewGuid(),
            CardId = card.CardId,
            CardLayoutId = cardLayout.CardLayoutId,
            Card = card,
            CardLayout = cardLayout
        };
        var dashboardCards = new List<DashboardCard> { dashboardCard };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboardCards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new GetDashboardCardByIdRequestHandler(mockContext.Object);
        var request = new GetDashboardCardByIdRequest { DashboardCardId = dashboardCardId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DashboardCard.Should().NotBeNull();
        result.DashboardCard!.DashboardCardId.Should().Be(dashboardCardId);
        result.DashboardCard.Card.Should().NotBeNull();
        result.DashboardCard.Card.Name.Should().Be("Card");
        result.DashboardCard.CardLayout.Should().NotBeNull();
        result.DashboardCard.CardLayout.Name.Should().Be("Layout");
    }

    [Fact]
    public async Task Handle_WhenDashboardCardNotFound_ShouldReturnNullDashboardCard()
    {
        // Arrange
        var dashboardCards = new List<DashboardCard>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboardCards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new GetDashboardCardByIdRequestHandler(mockContext.Object);
        var request = new GetDashboardCardByIdRequest { DashboardCardId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DashboardCard.Should().BeNull();
    }
}
