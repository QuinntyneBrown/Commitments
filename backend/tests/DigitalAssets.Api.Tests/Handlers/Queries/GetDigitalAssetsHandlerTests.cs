using DigitalAssets.Api.Features.DigitalAsset;
using DigitalAssets.Core;
using DigitalAssets.Core.Model.DigitalAssetAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace DigitalAssets.Api.Tests.Handlers.Queries;

public class GetDigitalAssetsHandlerTests
{
    [Fact]
    public async Task Handle_WhenNoDigitalAssets_ShouldReturnEmptyList()
    {
        var mockContext = MockDigitalAssetsDbContextFactory.Create();
        var emptyList = new List<DigitalAsset>();
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(emptyList);
        mockContext.Setup(c => c.DigitalAssets).Returns(mockDbSet.Object);

        var handler = new GetDigitalAssetsRequestHandler(mockContext.Object);

        var response = await handler.Handle(new GetDigitalAssetsRequest(), CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAssets.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenDigitalAssetsExist_ShouldReturnPopulatedList()
    {
        var mockContext = MockDigitalAssetsDbContextFactory.Create();
        var digitalAssets = new List<DigitalAsset>
        {
            new DigitalAsset { DigitalAssetId = Guid.NewGuid(), Name = "file1.png", ContentType = "image/png", Bytes = new byte[] { 1 } },
            new DigitalAsset { DigitalAssetId = Guid.NewGuid(), Name = "file2.jpg", ContentType = "image/jpeg", Bytes = new byte[] { 2 } }
        };
        var mockDbSet = MockDbSetFactory.CreateMockDbSet(digitalAssets);
        mockContext.Setup(c => c.DigitalAssets).Returns(mockDbSet.Object);

        var handler = new GetDigitalAssetsRequestHandler(mockContext.Object);

        var response = await handler.Handle(new GetDigitalAssetsRequest(), CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAssets.Should().HaveCount(2);
        response.DigitalAssets[0].Name.Should().Be("file1.png");
        response.DigitalAssets[1].Name.Should().Be("file2.jpg");
    }
}
