using DigitalAssets.Api.Controllers;
using DigitalAssets.Api.Features.DigitalAsset;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;

namespace DigitalAssets.Api.Tests.Controllers;

public class DigitalAssetControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<ILogger<DigitalAssetController>> _mockLogger;
    private readonly DigitalAssetController _controller;

    public DigitalAssetControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockLogger = new Mock<ILogger<DigitalAssetController>>();
        _controller = new DigitalAssetController(_mockSender.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnDigitalAssets()
    {
        var expectedResponse = new GetDigitalAssetsResponse { DigitalAssets = new List<DigitalAssetDto>() };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDigitalAssetsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Get(CancellationToken.None);

        result.Value.Should().NotBeNull();
    }
}
