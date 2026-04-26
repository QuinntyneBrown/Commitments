using Commitments.Testing.Common;
using Dashboard.Features.Dashboard;
using Dashboard.Data;
using Dashboard.Domain.DashboardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Queries;

public class GetDashboardByProfileIdHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnDashboardsForProfile()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var otherProfileId = Guid.NewGuid();
        var dashboards = new List<DashboardEntity>
        {
            new DashboardEntity("My Dashboard", profileId) { DashboardId = Guid.NewGuid() },
            new DashboardEntity("Other Dashboard", otherProfileId) { DashboardId = Guid.NewGuid() },
            new DashboardEntity("My Second Dashboard", profileId) { DashboardId = Guid.NewGuid() }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new GetDashboardByProfileIdRequestHandler(mockContext.Object);
        var request = new GetDashboardByProfileIdRequest { ProfileId = profileId };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Dashboards.Should().HaveCount(2);
        result.Dashboards.Should().AllSatisfy(d => d.ProfileId.Should().Be(profileId));
    }

    [Fact]
    public async Task Handle_WhenNoMatchingDashboards_ShouldReturnEmptyList()
    {
        // Arrange
        var dashboards = new List<DashboardEntity>
        {
            new DashboardEntity("Other Dashboard", Guid.NewGuid()) { DashboardId = Guid.NewGuid() }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new GetDashboardByProfileIdRequestHandler(mockContext.Object);
        var request = new GetDashboardByProfileIdRequest { ProfileId = Guid.NewGuid() };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Dashboards.Should().BeEmpty();
    }
}
