using Dashboard.Api.Controllers;
using Dashboard.Api.Features.Dashboard;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Controllers;

public class DashboardControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<ILogger<DashboardController>> _mockLogger;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly DashboardController _controller;
    private readonly Guid _profileId = Guid.NewGuid();

    public DashboardControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockLogger = new Mock<ILogger<DashboardController>>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = _profileId.ToString();
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        _controller = new DashboardController(_mockSender.Object, _mockLogger.Object, _mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task GetByCurrentProfile_ShouldSendRequestWithProfileIdFromHeader()
    {
        // Arrange
        var expectedResponse = new GetDashboardByProfileIdResponse
        {
            Dashboards = new List<DashboardDto>
            {
                new DashboardDto { DashboardId = Guid.NewGuid(), Name = "My Dashboard", ProfileId = _profileId }
            }
        };
        _mockSender.Setup(s => s.Send(It.Is<GetDashboardByProfileIdRequest>(r => r.ProfileId == _profileId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetByCurrentProfile();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboards.Should().HaveCount(1);
        result.Value.Dashboards[0].ProfileId.Should().Be(_profileId);
    }

    [Fact]
    public async Task Create_ShouldReturnCreateDashboardResponse()
    {
        // Arrange
        var request = new CreateDashboardRequest
        {
            Dashboard = new DashboardDto { Name = "New Dashboard", ProfileId = Guid.NewGuid() }
        };
        var expectedResponse = new CreateDashboardResponse
        {
            Dashboard = new DashboardDto { DashboardId = Guid.NewGuid(), Name = "New Dashboard" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<CreateDashboardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Create(request, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboard.Name.Should().Be("New Dashboard");
    }

    [Fact]
    public async Task Update_ShouldReturnUpdateDashboardResponse()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var request = new UpdateDashboardRequest
        {
            Dashboard = new DashboardDto { DashboardId = dashboardId, Name = "Updated" }
        };
        var expectedResponse = new UpdateDashboardResponse
        {
            Dashboard = new DashboardDto { DashboardId = dashboardId, Name = "Updated" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<UpdateDashboardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(request, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboard.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task Get_ShouldReturnDashboards()
    {
        // Arrange
        var expectedResponse = new GetDashboardsResponse
        {
            Dashboards = new List<DashboardDto>
            {
                new DashboardDto { DashboardId = Guid.NewGuid(), Name = "Dashboard 1" },
                new DashboardDto { DashboardId = Guid.NewGuid(), Name = "Dashboard 2" }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDashboardsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get(CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboards.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_WhenFound_ShouldReturnDashboard()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var expectedResponse = new GetDashboardByIdResponse
        {
            Dashboard = new DashboardDto { DashboardId = dashboardId, Name = "Found" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDashboardByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(dashboardId, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboard!.DashboardId.Should().Be(dashboardId);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var expectedResponse = new GetDashboardByIdResponse { Dashboard = null };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDashboardByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(dashboardId, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnDeleteDashboardResponse()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var expectedResponse = new DeleteDashboardResponse();
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteDashboardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Delete(dashboardId, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        _mockSender.Verify(s => s.Send(It.Is<DeleteDashboardRequest>(r => r.DashboardId == dashboardId), It.IsAny<CancellationToken>()), Times.Once);
    }
}
