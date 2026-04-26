using Commitments.Testing.Common;
using Dashboard.Api.Features.Dashboard;
using Dashboard.Core;
using Dashboard.Core.Model.DashboardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Queries;

public class GetDashboardByIdHandlerTests
{
    [Fact]
    public async Task Handle_WhenDashboardExists_ShouldReturnDashboard()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var profileId = Guid.NewGuid();
        var dashboard = new DashboardEntity("Test Dashboard", profileId) { DashboardId = dashboardId };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<DashboardEntity>();
        mockDbSet.Setup(d => d.FindAsync(dashboardId)).ReturnsAsync(dashboard);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new GetDashboardByIdRequestHandler(mockContext.Object);
        var request = new GetDashboardByIdRequest { DashboardId = dashboardId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Dashboard.Should().NotBeNull();
        result.Dashboard!.DashboardId.Should().Be(dashboardId);
        result.Dashboard.Name.Should().Be("Test Dashboard");
        result.Dashboard.ProfileId.Should().Be(profileId);
    }

    [Fact]
    public async Task Handle_WhenDashboardNotFound_ShouldReturnNullDashboard()
    {
        // Arrange
        var mockDbSet = MockDbSetFactory.CreateMockDbSet<DashboardEntity>();
        mockDbSet.Setup(d => d.FindAsync(It.IsAny<Guid>())).ReturnsAsync((DashboardEntity?)null);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new GetDashboardByIdRequestHandler(mockContext.Object);
        var request = new GetDashboardByIdRequest { DashboardId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Dashboard.Should().BeNull();
    }
}
