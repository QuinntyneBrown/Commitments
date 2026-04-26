using Commitments.Testing.Common;
using Dashboard.Features.Dashboard;
using Dashboard.Data;
using Dashboard.Domain.DashboardAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Handlers.Commands;

public class CreateDashboardHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateDashboardAndSave()
    {
        // Arrange
        var dashboards = new List<DashboardEntity>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new CreateDashboardRequestHandler(mockContext.Object);
        var profileId = Guid.NewGuid();
        var request = new CreateDashboardRequest
        {
            Dashboard = new DashboardDto { Name = "Test Dashboard", ProfileId = profileId }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Dashboard.Should().NotBeNull();
        result.Dashboard.Name.Should().Be("Test Dashboard");
        result.Dashboard.ProfileId.Should().Be(profileId);
        dashboards.Should().HaveCount(1);
        dashboards[0].Name.Should().Be("Test Dashboard");
        dashboards[0].ProfileId.Should().Be(profileId);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNullProfileId_ShouldCreateDashboard()
    {
        // Arrange
        var dashboards = new List<DashboardEntity>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(dashboards);
        var mockContext = MockDashboardDbContextFactory.Create();
        mockContext.Setup(c => c.Dashboards).Returns(mockDbSet.Object);

        var handler = new CreateDashboardRequestHandler(mockContext.Object);
        var request = new CreateDashboardRequest
        {
            Dashboard = new DashboardDto { Name = "No Profile Dashboard", ProfileId = null }
        };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        result.Dashboard.Should().NotBeNull();
        result.Dashboard.Name.Should().Be("No Profile Dashboard");
        result.Dashboard.ProfileId.Should().BeNull();
        dashboards.Should().HaveCount(1);
        mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
