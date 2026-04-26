using Commitments.Testing.Common;
using Dashboard.Features.DashboardCard;
using Dashboard.Data;
using Dashboard.Domain.DashboardCardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class CreateDashboardCardHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateDashboardCardAndSave()
    {
        // Arrange
        var dashboardCards = new List<DashboardCard>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboardCards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new CreateDashboardCardRequestHandler(mockContext.Object);
        var dashboardId = Guid.NewGuid();
        var cardId = Guid.NewGuid();
        var cardLayoutId = Guid.NewGuid();
        var request = new CreateDashboardCardRequest
        {
            DashboardCard = new DashboardCardDto
            {
                DashboardId = dashboardId,
                CardId = cardId,
                CardLayoutId = cardLayoutId
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DashboardCard.Should().NotBeNull();
        result.DashboardCard.DashboardId.Should().Be(dashboardId);
        result.DashboardCard.CardId.Should().Be(cardId);
        result.DashboardCard.CardLayoutId.Should().Be(cardLayoutId);
        dashboardCards.Should().HaveCount(1);
        dashboardCards[0].DashboardId.Should().Be(dashboardId);
        dashboardCards[0].CardId.Should().Be(cardId);
        dashboardCards[0].CardLayoutId.Should().Be(cardLayoutId);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
