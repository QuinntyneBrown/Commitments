using Dashboard.Api.Controllers;
using Dashboard.Api.Features.CardLayout;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dashboard.Api.Tests.Controllers;

public class CardLayoutControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<ILogger<CardLayoutController>> _mockLogger;
    private readonly CardLayoutController _controller;

    public CardLayoutControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockLogger = new Mock<ILogger<CardLayoutController>>();
        _controller = new CardLayoutController(_mockSender.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnCreateCardLayoutResponse()
    {
        // Arrange
        var request = new CreateCardLayoutRequest
        {
            CardLayout = new CardLayoutDto { Name = "New Layout", Description = "Layout Desc" }
        };
        var expectedResponse = new CreateCardLayoutResponse
        {
            CardLayout = new CardLayoutDto { CardLayoutId = Guid.NewGuid(), Name = "New Layout", Description = "Layout Desc" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<CreateCardLayoutRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Create(request, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.CardLayout.Name.Should().Be("New Layout");
    }

    [Fact]
    public async Task Update_ShouldReturnUpdateCardLayoutResponse()
    {
        // Arrange
        var cardLayoutId = Guid.NewGuid();
        var request = new UpdateCardLayoutRequest
        {
            CardLayout = new CardLayoutDto { CardLayoutId = cardLayoutId, Name = "Updated Layout", Description = "Updated Desc" }
        };
        var expectedResponse = new UpdateCardLayoutResponse
        {
            CardLayout = new CardLayoutDto { CardLayoutId = cardLayoutId, Name = "Updated Layout", Description = "Updated Desc" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<UpdateCardLayoutRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Update(request, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.CardLayout.Name.Should().Be("Updated Layout");
    }

    [Fact]
    public async Task Get_ShouldReturnCardLayouts()
    {
        // Arrange
        var expectedResponse = new GetCardLayoutsResponse
        {
            CardLayouts = new List<CardLayoutDto>
            {
                new CardLayoutDto { CardLayoutId = Guid.NewGuid(), Name = "Layout 1" },
                new CardLayoutDto { CardLayoutId = Guid.NewGuid(), Name = "Layout 2" }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetCardLayoutsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Get(CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.CardLayouts.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_WhenFound_ShouldReturnCardLayout()
    {
        // Arrange
        var cardLayoutId = Guid.NewGuid();
        var expectedResponse = new GetCardLayoutByIdResponse
        {
            CardLayout = new CardLayoutDto { CardLayoutId = cardLayoutId, Name = "Found Layout" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetCardLayoutByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(cardLayoutId, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        result.Value!.CardLayout!.CardLayoutId.Should().Be(cardLayoutId);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        var cardLayoutId = Guid.NewGuid();
        var expectedResponse = new GetCardLayoutByIdResponse { CardLayout = null };
        _mockSender.Setup(s => s.Send(It.IsAny<GetCardLayoutByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.GetById(cardLayoutId, CancellationToken.None);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnDeleteCardLayoutResponse()
    {
        // Arrange
        var cardLayoutId = Guid.NewGuid();
        var expectedResponse = new DeleteCardLayoutResponse();
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteCardLayoutRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.Delete(cardLayoutId, CancellationToken.None);

        // Assert
        result.Value.Should().NotBeNull();
        _mockSender.Verify(s => s.Send(It.Is<DeleteCardLayoutRequest>(r => r.CardLayoutId == cardLayoutId), It.IsAny<CancellationToken>()), Times.Once);
    }
}
