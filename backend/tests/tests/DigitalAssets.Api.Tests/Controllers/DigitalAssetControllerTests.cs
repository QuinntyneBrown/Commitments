using DigitalAssets.Api.Controllers;
using DigitalAssets.Api.Features.DigitalAsset;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public async Task Create_ShouldReturnCreateDigitalAssetResponse()
    {
        var request = new CreateDigitalAssetRequest
        {
            DigitalAsset = new DigitalAssetDto
            {
                Name = "test.png",
                ContentType = "image/png",
                Bytes = new byte[] { 1, 2, 3 }
            }
        };
        var expectedResponse = new CreateDigitalAssetResponse { DigitalAsset = request.DigitalAsset };
        _mockSender.Setup(s => s.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Create(request, CancellationToken.None);

        result.Value.Should().NotBeNull();
        result.Value!.DigitalAsset.Name.Should().Be("test.png");
    }

    [Fact]
    public async Task Update_ShouldReturnUpdateDigitalAssetResponse()
    {
        var request = new UpdateDigitalAssetRequest
        {
            DigitalAsset = new DigitalAssetDto
            {
                DigitalAssetId = Guid.NewGuid(),
                Name = "updated.png",
                ContentType = "image/png",
                Bytes = new byte[] { 4, 5, 6 }
            }
        };
        var expectedResponse = new UpdateDigitalAssetResponse { DigitalAsset = request.DigitalAsset };
        _mockSender.Setup(s => s.Send(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Update(request, CancellationToken.None);

        result.Value.Should().NotBeNull();
        result.Value!.DigitalAsset.Name.Should().Be("updated.png");
    }

    [Fact]
    public async Task Get_ShouldReturnDigitalAssets()
    {
        var expectedResponse = new GetDigitalAssetsResponse { DigitalAssets = new List<DigitalAssetDto>() };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDigitalAssetsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Get(CancellationToken.None);

        result.Value.Should().NotBeNull();
        result.Value!.DigitalAssets.Should().BeEmpty();
    }

    [Fact]
    public async Task GetById_WhenDigitalAssetExists_ShouldReturnDigitalAsset()
    {
        var digitalAssetId = Guid.NewGuid();
        var expectedResponse = new GetDigitalAssetByIdResponse
        {
            DigitalAsset = new DigitalAssetDto { DigitalAssetId = digitalAssetId, Name = "found.png" }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDigitalAssetByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.GetById(digitalAssetId, CancellationToken.None);

        result.Value.Should().NotBeNull();
        result.Value!.DigitalAsset!.DigitalAssetId.Should().Be(digitalAssetId);
    }

    [Fact]
    public async Task GetById_WhenDigitalAssetNotFound_ShouldReturnNotFound()
    {
        var digitalAssetId = Guid.NewGuid();
        var expectedResponse = new GetDigitalAssetByIdResponse { DigitalAsset = null };
        _mockSender.Setup(s => s.Send(It.IsAny<GetDigitalAssetByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.GetById(digitalAssetId, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnDeleteDigitalAssetResponse()
    {
        var digitalAssetId = Guid.NewGuid();
        var expectedResponse = new DeleteDigitalAssetResponse();
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteDigitalAssetRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Delete(digitalAssetId, CancellationToken.None);

        result.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_ShouldReturnUploadDigitalAssetResponse()
    {
        var expectedResponse = new UploadDigitalAssetResponse
        {
            DigitalAssets = new List<DigitalAssetDto>
            {
                new DigitalAssetDto { Name = "uploaded.png", ContentType = "image/png" }
            }
        };
        _mockSender.Setup(s => s.Send(It.IsAny<UploadDigitalAssetRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.Post("upload");

        result.Value.Should().NotBeNull();
        result.Value!.DigitalAssets.Should().HaveCount(1);
        result.Value.DigitalAssets[0].Name.Should().Be("uploaded.png");
    }
}
