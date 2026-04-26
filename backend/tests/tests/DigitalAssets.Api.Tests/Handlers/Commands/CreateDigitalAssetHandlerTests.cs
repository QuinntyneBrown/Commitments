using DigitalAssets.Api.Features.DigitalAsset;
using DigitalAssets.Core;
using DigitalAssets.Core.Model.DigitalAssetAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace DigitalAssets.Api.Tests.Handlers.Commands;

public class CreateDigitalAssetHandlerTests
{
    private readonly Mock<IDigitalAssetsDbContext> _mockContext;
    private readonly CreateDigitalAssetRequestHandler _handler;

    public CreateDigitalAssetHandlerTests()
    {
        _mockContext = MockDigitalAssetsDbContextFactory.Create();
        _handler = new CreateDigitalAssetRequestHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateDigitalAssetAndReturnDto()
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

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAsset.Should().NotBeNull();
        response.DigitalAsset.Name.Should().Be("test.png");
        response.DigitalAsset.ContentType.Should().Be("image/png");
        response.DigitalAsset.Bytes.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        _mockContext.Verify(c => c.DigitalAssets.Add(It.IsAny<DigitalAsset>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldMapRequestPropertiesToEntity()
    {
        DigitalAsset? addedEntity = null;
        _mockContext.Setup(c => c.DigitalAssets.Add(It.IsAny<DigitalAsset>()))
            .Callback<DigitalAsset>(da => addedEntity = da);

        var request = new CreateDigitalAssetRequest
        {
            DigitalAsset = new DigitalAssetDto
            {
                Name = "document.pdf",
                ContentType = "application/pdf",
                Bytes = new byte[] { 10, 20 }
            }
        };

        await _handler.Handle(request, CancellationToken.None);

        addedEntity.Should().NotBeNull();
        addedEntity!.Name.Should().Be("document.pdf");
        addedEntity.ContentType.Should().Be("application/pdf");
        addedEntity.Bytes.Should().BeEquivalentTo(new byte[] { 10, 20 });
    }
}
