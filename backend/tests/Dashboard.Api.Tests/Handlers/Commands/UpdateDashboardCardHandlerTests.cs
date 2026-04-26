using Commitments.Testing.Common;
using Dashboard.Features.DashboardCard;
using Dashboard.Data;
using Dashboard.Domain.DashboardCardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class UpdateDashboardCardHandlerTests
{
    [Fact]
    public async Task Handle_WhenDashboardCardExists_ShouldUpdateAndSave()
    {
        // Arrange
        var dashboardCardId = Guid.NewGuid();
        var dashboardCard = new DashboardCard
        {
            DashboardCardId = dashboardCardId,
            DashboardId = Guid.NewGuid(),
            CardId = Guid.NewGuid(),
            CardLayoutId = Guid.NewGuid()
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<DashboardCard>();
        mockDbSet.Setup(d => d.FindAsync(dashboardCardId)).ReturnsAsync(dashboardCard);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new UpdateDashboardCardRequestHandler(mockContext.Object);
        var newDashboardId = Guid.NewGuid();
        var newCardId = Guid.NewGuid();
        var newCardLayoutId = Guid.NewGuid();
        var request = new UpdateDashboardCardRequest
        {
            DashboardCard = new DashboardCardDto
            {
                DashboardCardId = dashboardCardId,
                DashboardId = newDashboardId,
                CardId = newCardId,
                CardLayoutId = newCardLayoutId
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DashboardCard.Should().NotBeNull();
        result.DashboardCard.DashboardId.Should().Be(newDashboardId);
        result.DashboardCard.CardId.Should().Be(newCardId);
        result.DashboardCard.CardLayoutId.Should().Be(newCardLayoutId);
        dashboardCard.DashboardId.Should().Be(newDashboardId);
        dashboardCard.CardId.Should().Be(newCardId);
        dashboardCard.CardLayoutId.Should().Be(newCardLayoutId);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDashboardCardNotFound_ShouldReturnEmptyResponse()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<DashboardCard>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((DashboardCard?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new UpdateDashboardCardRequestHandler(mockContext.Object);
        var request = new UpdateDashboardCardRequest
        {
            DashboardCard = new DashboardCardDto
            {
                DashboardCardId = Guid.NewGuid(),
                DashboardId = Guid.NewGuid(),
                CardId = Guid.NewGuid(),
                CardLayoutId = Guid.NewGuid()
            }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.DashboardCard.Should().BeNull();
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
