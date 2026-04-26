using Commitments.Testing.Common;
using Dashboard.Features.Dashboard;
using Dashboard.Data;
using Dashboard.Domain.DashboardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class DeleteDashboardHandlerTests
{
    [Fact]
    public async Task Handle_WhenDashboardExists_ShouldRemoveAndSave()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var dashboard = new DashboardEntity("ToDelete", Guid.NewGuid()) { DashboardId = dashboardId };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<DashboardEntity>();
        mockDbSet.Setup(d => d.FindAsync(dashboardId)).ReturnsAsync(dashboard);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new DeleteDashboardRequestHandler(mockContext.Object);
        var request = new DeleteDashboardRequest { DashboardId = dashboardId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockDbSet.Verify(d => d.Remove(dashboard), Times.Once);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDashboardNotFound_ShouldNotCallSave()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<DashboardEntity>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((DashboardEntity?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new DeleteDashboardRequestHandler(mockContext.Object);
        var request = new DeleteDashboardRequest { DashboardId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        mockDbSet.Verify(d => d.Remove(It.IsAny<DashboardEntity>()), Times.Never);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
