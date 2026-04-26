using DigitalAssets.Features.DigitalAsset;
using DigitalAssets.Data;
using DigitalAssets.Domain.DigitalAssetAggregate;
using Commitments.Testing.Common;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace DigitalAssets.Api.Tests.Handlers.Commands;

public class DeleteDigitalAssetHandlerTests
{
    private readonly Mock<IDigitalAssetsDbContext> _mockContext;
    private readonly DeleteDigitalAssetRequestHandler _handler;

    public DeleteDigitalAssetHandlerTests()
    {
        _mockContext = MockDigitalAssetsDbContextFactory.Create();
        _handler = new DeleteDigitalAssetRequestHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_WhenDigitalAssetExists_ShouldRemoveAndSave()
    {
        var digitalAssetId = Guid.NewGuid();
        var existingAsset = new DigitalAssetBuilder()
            .WithDigitalAssetId(digitalAssetId)
            .Build();

        _mockContext.Setup(c => c.DigitalAssets.FindAsync(digitalAssetId))
            .ReturnsAsync(existingAsset);

        var request = new DeleteDigitalAssetRequest { DigitalAssetId = digitalAssetId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        _mockContext.Verify(c => c.DigitalAssets.Remove(existingAsset), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDigitalAssetNotFound_ShouldReturnResponseWithoutRemoving()
    {
        var digitalAssetId = Guid.NewGuid();
        _mockContext.Setup(c => c.DigitalAssets.FindAsync(digitalAssetId))
            .ReturnsAsync((DigitalAsset?)null);

        var request = new DeleteDigitalAssetRequest { DigitalAssetId = digitalAssetId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        _mockContext.Verify(c => c.DigitalAssets.Remove(It.IsAny<DigitalAsset>()), Times.Never);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
