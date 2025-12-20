// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Api.Controllers;
using Commitments.Core.Model.DashboardAggregate;
using Commitments.Api.Features.Dashboard;
using Commitments.Api.Features.Dashboard;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Commitments.Api.Tests.Controllers;

public class DashboardControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly DashboardController _controller;
    private readonly Guid _profileId = Guid.NewGuid();

    public DashboardControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var claims = new List<Claim>
        {
            new Claim("ProfileId", _profileId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.User).Returns(principal);
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        _controller = new DashboardController(_mockHttpContextAccessor.Object, _mockSender.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreateDashboardResponse()
    {
        // Arrange
        var request = new CreateDashboardRequest
        {
            Dashboard = new DashboardDto
            {
                DashboardId = Guid.NewGuid(),
                Name = "Test Dashboard"
            }
        };
        var expectedResponse = new CreateDashboardResponse
        {
            Dashboard = request.Dashboard
        };
        _mockSender.Setup(s => s.Send(It.IsAny<CreateDashboardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Create(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboard.DashboardId.Should().Be(request.Dashboard.DashboardId);
    }

    [Fact]
    public async Task Update_ShouldReturnUpdateDashboardResponse()
    {
        // Arrange
        var request = new UpdateDashboardRequest
        {
            Dashboard = new DashboardDto
            {
                DashboardId = Guid.NewGuid(),
                Name = "Updated Dashboard"
            }
        };
        var expectedResponse = new UpdateDashboardResponse
        {
            Dashboard = request.Dashboard
        };
        _mockSender.Setup(s => s.Send(It.IsAny<UpdateDashboardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboard.Name.Should().Be("Updated Dashboard");
    }

    [Fact]
    public async Task Delete_ShouldCallSender()
    {
        // Arrange
        var request = new DeleteDashboardRequest
        {
            DashboardId = Guid.NewGuid()
        };
        var expectedResponse = new DeleteDashboardResponse();
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteDashboardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        await _controller.Delete(request);

        // Assert
        _mockSender.Verify(s => s.Send(It.Is<DeleteDashboardRequest>(r => r.DashboardId == request.DashboardId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_ShouldReturnDashboard()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var request = new GetDashboardByIdRequest { DashboardId = dashboardId };
        var expectedResponse = new GetDashboardByIdResponse
        {
            Dashboard = new DashboardDto { DashboardId = dashboardId, Name = "Test" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDashboardByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(request);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboard.DashboardId.Should().Be(dashboardId);
    }

    [Fact]
    public async Task Get_ShouldReturnAllDashboards()
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
        var result = await _controller.Get();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboards.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByCurrentUser_ShouldReturnDashboardsForCurrentUser()
    {
        // Arrange
        var expectedResponse = new GetDashboardsByCurrentUserResponse
        {
            Dashboards = new List<DashboardDto>
            {
                new DashboardDto { DashboardId = Guid.NewGuid(), Name = "My Dashboard" }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDashboardsByCurrentUserRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetByCurrentUser();

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.Dashboards.Should().HaveCount(1);
    }
}
