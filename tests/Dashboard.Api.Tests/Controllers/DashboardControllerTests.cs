using Dashboard.Api.Controllers;
using Dashboard.Api.Features.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace Dashboard.Api.Tests.Controllers;

public class DashboardControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<ILogger<DashboardController>> _mockLogger;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly DashboardController _controller;

    public DashboardControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockLogger = new Mock<ILogger<DashboardController>>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _controller = new DashboardController(_mockSender.Object, _mockLogger.Object, _mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnDashboards()
    {
        var expectedResponse = new GetDashboardsResponse { Dashboards = new List<DashboardDto>() };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDashboardsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Get(CancellationToken.None);

        result.Value.Should().NotBeNull();
    }
}
