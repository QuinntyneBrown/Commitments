using DigitalAssets.Features.DigitalAsset;
using DigitalAssets.Data;
using DigitalAssets.Domain.DigitalAssetAggregate;
using Commitments.Testing.Common;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace DigitalAssets.Api.Tests.Handlers.Commands;

public class UpdateDigitalAssetHandlerTests
{
    private readonly Mock<IDigitalAssetsDbContext> _mockContext;
    private readonly UpdateDigitalAssetRequestHandler _handler;

    public UpdateDigitalAssetHandlerTests()
    {
        _mockContext = MockDigitalAssetsDbContextFactory.Create();
        _handler = new UpdateDigitalAssetRequestHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_WhenDigitalAssetExists_ShouldUpdateAndReturnDto()
    {
        var digitalAssetId = Guid.NewGuid();
        var existingAsset = new DigitalAssetBuilder()
            .WithDigitalAssetId(digitalAssetId)
            .WithName("old.png")
            .WithContentType("image/png")
            .WithBytes(new byte[] { 1 })
            .Build();

        _mockContext.Setup(c => c.DigitalAssets.FindAsync(digitalAssetId))
            .ReturnsAsync(existingAsset);

        var request = new UpdateDigitalAssetRequest
        {
            DigitalAsset = new DigitalAssetDto
            {
                DigitalAssetId = digitalAssetId,
                Name = "updated.jpg",
                ContentType = "image/jpeg",
                Bytes = new byte[] { 4, 5, 6 }
            }
        };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAsset.Should().NotBeNull();
        response.DigitalAsset.Name.Should().Be("updated.jpg");
        response.DigitalAsset.ContentType.Should().Be("image/jpeg");
        response.DigitalAsset.Bytes.Should().BeEquivalentTo(new byte[] { 4, 5, 6 });
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDigitalAssetNotFound_ShouldReturnEmptyResponse()
    {
        var digitalAssetId = Guid.NewGuid();
        _mockContext.Setup(c => c.DigitalAssets.FindAsync(digitalAssetId))
            .ReturnsAsync((DigitalAsset?)null);

        var request = new UpdateDigitalAssetRequest
        {
            DigitalAsset = new DigitalAssetDto
            {
                DigitalAssetId = digitalAssetId,
                Name = "nonexistent.png"
            }
        };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAsset.Should().BeNull();
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
