using Dashboard.Controllers;
using Dashboard.Features.DashboardCard;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Controllers;

public class DashboardCardControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<ILogger<DashboardCardController>> _mockLogger;
    private readonly DashboardCardController _controller;

    public DashboardCardControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockLogger = new Mock<ILogger<DashboardCardController>>();
        _controller = new DashboardCardController(_mockSender.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreateDashboardCardResponse()
    {
        // Arrange
        var request = new CreateDashboardCardRequest
        {
            DashboardCard = new DashboardCardDto
            {
                DashboardId = Guid.NewGuid(),
                CardId = Guid.NewGuid(),
                CardLayoutId = Guid.NewGuid()
            }
        };
        var expectedResponse = new CreateDashboardCardResponse
        {
            DashboardCard = new DashboardCardDto { DashboardCardId = Guid.NewGuid() }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<CreateDashboardCardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Create(request, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.DashboardCard.DashboardCardId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Update_ShouldReturnUpdateDashboardCardResponse()
    {
        // Arrange
        var dashboardCardId = Guid.NewGuid();
        var request = new UpdateDashboardCardRequest
        {
            DashboardCard = new DashboardCardDto
            {
                DashboardCardId = dashboardCardId,
                DashboardId = Guid.NewGuid(),
                CardId = Guid.NewGuid(),
                CardLayoutId = Guid.NewGuid()
            }
        };
        var expectedResponse = new UpdateDashboardCardResponse
        {
            DashboardCard = new DashboardCardDto { DashboardCardId = dashboardCardId }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<UpdateDashboardCardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(request, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.DashboardCard.DashboardCardId.Should().Be(dashboardCardId);
    }

    [Fact]
    public async Task SaveRange_ShouldReturnSaveDashboardCardRangeResponse()
    {
        // Arrange
        var dashboardId = Guid.NewGuid();
        var request = new SaveDashboardCardRangeRequest
        {
            DashboardId = dashboardId,
            DashboardCards = new List<DashboardCardDto>
            {
                new DashboardCardDto { CardId = Guid.NewGuid(), CardLayoutId = Guid.NewGuid() },
                new DashboardCardDto { CardId = Guid.NewGuid(), CardLayoutId = Guid.NewGuid() }
            }
        };
        var expectedResponse = new SaveDashboardCardRangeResponse
        {
            DashboardCardIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<SaveDashboardCardRangeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.SaveRange(request, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.DashboardCardIds.Should().HaveCount(2);
    }

    [Fact]
    public async Task Get_ShouldReturnDashboardCards()
    {
        // Arrange
        var expectedResponse = new GetDashboardCardsResponse
        {
            DashboardCards = new List<DashboardCardDto>
            {
                new DashboardCardDto { DashboardCardId = Guid.NewGuid() },
                new DashboardCardDto { DashboardCardId = Guid.NewGuid() }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDashboardCardsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get(CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.DashboardCards.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_WhenFound_ShouldReturnDashboardCard()
    {
        // Arrange
        var dashboardCardId = Guid.NewGuid();
        var expectedResponse = new GetDashboardCardByIdResponse
        {
            DashboardCard = new DashboardCardDto { DashboardCardId = dashboardCardId }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDashboardCardByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(dashboardCardId, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.DashboardCard!.DashboardCardId.Should().Be(dashboardCardId);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var dashboardCardId = Guid.NewGuid();
        var expectedResponse = new GetDashboardCardByIdResponse { DashboardCard = null };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDashboardCardByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(dashboardCardId, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnDeleteDashboardCardResponse()
    {
        // Arrange
        var dashboardCardId = Guid.NewGuid();
        var expectedResponse = new DeleteDashboardCardResponse();
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteDashboardCardRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Delete(dashboardCardId, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        _mockSender.Verify(s => s.Send(It.Is<DeleteDashboardCardRequest>(r => r.DashboardCardId == dashboardCardId), It.IsAny<CancellationToken>()), Times.Once);
    }
}
