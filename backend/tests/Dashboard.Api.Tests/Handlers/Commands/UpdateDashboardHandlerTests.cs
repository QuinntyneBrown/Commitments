using Commitments.Testing.Common;
using Dashboard.Features.Dashboard;
using Dashboard.Data;
using Dashboard.Domain.DashboardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class UpdateDashboardHandlerTests
{
    [Fact]
    public async Task Handle_WhenDashboardExists_ShouldUpdateAndSave()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var dashboard = new DashboardEntity("Original", profileId) { DashboardId = dashboardId };
        var dashboards = new List<DashboardEntity> { dashboard };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboards);
        mockDbSet.Setup(d => d.FindAsync(dashboardId)).ReturnsAsync(dashboard);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new UpdateDashboardRequestHandler(mockContext.Object);
        var newProfileId = Guid.NewGuid();
        var request = new UpdateDashboardRequest
        {
            Dashboard = new DashboardDto { DashboardId = dashboardId, Name = "Updated", ProfileId = newProfileId }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Dashboard.Should().NotBeNull();
        result.Dashboard.Name.Should().Be("Updated");
        result.Dashboard.ProfileId.Should().Be(newProfileId);
        dashboard.Name.Should().Be("Updated");
        dashboard.ProfileId.Should().Be(newProfileId);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDashboardNotFound_ShouldReturnEmptyResponse()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<DashboardEntity>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((DashboardEntity?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new UpdateDashboardRequestHandler(mockContext.Object);
        var request = new UpdateDashboardRequest
        {
            Dashboard = new DashboardDto { DashboardId = Guid.NewGuid(), Name = "Updated" }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Dashboard.Should().BeNull();
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
