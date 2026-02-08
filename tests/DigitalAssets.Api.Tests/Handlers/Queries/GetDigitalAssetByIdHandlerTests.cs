using DigitalAssets.Api.Features.DigitalAsset;
using DigitalAssets.Core;
using DigitalAssets.Core.Model.DigitalAssetAggregate;
using Commitments.Testing.Common;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace DigitalAssets.Api.Tests.Handlers.Queries;

public class GetDigitalAssetByIdHandlerTests
{
    private readonly Mock<IDigitalAssetsDbContext> _mockContext;
    private readonly GetDigitalAssetByIdRequestHandler _handler;

    public GetDigitalAssetByIdHandlerTests()
    {
        _mockContext = MockDigitalAssetsDbContextFactory.Create();
        _handler = new GetDigitalAssetByIdRequestHandler(_mockContext.Object);
    }

    [Fact]
    public async Task Handle_WhenDigitalAssetExists_ShouldReturnDto()
    {
        var digitalAssetId = Guid.NewGuid();
        var existingAsset = new DigitalAssetBuilder()
            .WithDigitalAssetId(digitalAssetId)
            .WithName("found.png")
            .WithContentType("image/png")
            .WithBytes(new byte[] { 1, 2, 3 })
            .Build();

        _mockContext.Setup(c => c.DigitalAssets.FindAsync(digitalAssetId))
            .ReturnsAsync(existingAsset);

        var request = new GetDigitalAssetByIdRequest { DigitalAssetId = digitalAssetId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAsset.Should().NotBeNull();
        response.DigitalAsset!.DigitalAssetId.Should().Be(digitalAssetId);
        response.DigitalAsset.Name.Should().Be("found.png");
        response.DigitalAsset.ContentType.Should().Be("image/png");
        response.DigitalAsset.Bytes.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
    }

    [Fact]
    public async Task Handle_WhenDigitalAssetNotFound_ShouldReturnNullDigitalAsset()
    {
        var digitalAssetId = Guid.NewGuid();
        _mockContext.Setup(c => c.DigitalAssets.FindAsync(digitalAssetId))
            .ReturnsAsync((DigitalAsset?)null);

        var request = new GetDigitalAssetByIdRequest { DigitalAssetId = digitalAssetId };

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAsset.Should().BeNull();
    }
}
