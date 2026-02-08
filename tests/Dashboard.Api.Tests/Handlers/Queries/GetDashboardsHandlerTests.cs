using Commitments.Testing.Common;
using Dashboard.Api.Features.Dashboard;
using Dashboard.Core;
using Dashboard.Core.Model.DashboardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Queries;

public class GetDashboardsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAllDashboards()
    {
        // Arrange
        var dashboards = new List<DashboardEntity>
        {
            new DashboardEntity("Dashboard 1", Guid.NewGuid()) { DashboardId = Guid.NewGuid() },
            new DashboardEntity("Dashboard 2", Guid.NewGuid()) { DashboardId = Guid.NewGuid() }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new GetDashboardsRequestHandler(mockContext.Object);
        var request = new GetDashboardsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Dashboards.Should().HaveCount(2);
        result.Dashboards[0].Name.Should().Be("Dashboard 1");
        result.Dashboards[1].Name.Should().Be("Dashboard 2");
    }

    [Fact]
    public async Task Handle_WhenNoDashboards_ShouldReturnEmptyList()
    {
        // Arrange
        var dashboards = new List<DashboardEntity>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new GetDashboardsRequestHandler(mockContext.Object);
        var request = new GetDashboardsRequest();

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Dashboards.Should().BeEmpty();
    }
}
