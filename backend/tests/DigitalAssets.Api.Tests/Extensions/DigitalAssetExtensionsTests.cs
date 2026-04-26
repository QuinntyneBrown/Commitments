using DigitalAssets.Features.DigitalAsset;
using DigitalAssets.Domain.DigitalAssetAggregate;
using Commitments.Testing.Common.Builders;
using FluentAssertions;
using Xunit;

namespace DigitalAssets.Api.Tests.Extensions;

public class DigitalAssetExtensionsTests
{
    [Fact]
    public void ToDto_ShouldMapAllProperties()
    {
        var digitalAssetId = Guid.NewGuid();
        var digitalAsset = new DigitalAssetBuilder()
            .WithDigitalAssetId(digitalAssetId)
            .WithName("document.pdf")
            .WithContentType("application/pdf")
            .WithBytes(new byte[] { 10, 20, 30 })
            .Build();

        var dto = digitalAsset.ToDto();

        dto.Should().NotBeNull();
        dto.DigitalAssetId.Should().Be(digitalAssetId);
        dto.Name.Should().Be("document.pdf");
        dto.ContentType.Should().Be("application/pdf");
        dto.Bytes.Should().BeEquivalentTo(new byte[] { 10, 20, 30 });
    }

    [Fact]
    public void ToDto_WithNullOptionalFields_ShouldMapCorrectly()
    {
        var digitalAsset = new DigitalAssetBuilder()
            .WithName(null)
            .WithContentType(null)
            .WithBytes(null)
            .Build();

        var dto = digitalAsset.ToDto();

        dto.Should().NotBeNull();
        dto.DigitalAssetId.Should().NotBeEmpty();
        dto.Name.Should().BeNull();
        dto.ContentType.Should().BeNull();
        dto.Bytes.Should().BeNull();
    }
}
