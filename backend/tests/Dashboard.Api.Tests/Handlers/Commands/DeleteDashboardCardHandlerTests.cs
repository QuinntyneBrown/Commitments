using Commitments.Testing.Common;
using Dashboard.Features.DashboardCard;
using Dashboard.Data;
using Dashboard.Domain.DashboardCardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class DeleteDashboardCardHandlerTests
{
    [Fact]
    public async Task Handle_WhenDashboardCardExists_ShouldRemoveAndSave()
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

        var handler = new DeleteDashboardCardRequestHandler(mockContext.Object);
        var request = new DeleteDashboardCardRequest { DashboardCardId = dashboardCardId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockDbSet.Verify(d => d.Remove(dashboardCard), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDashboardCardNotFound_ShouldNotCallSave()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<DashboardCard>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((DashboardCard?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.DashboardCards).Returns(mockDbSet.Object);

        var handler = new DeleteDashboardCardRequestHandler(mockContext.Object);
        var request = new DeleteDashboardCardRequest { DashboardCardId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockDbSet.Verify(d => d.Remove(It.IsAny<DashboardCard>()), Times.Never);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
