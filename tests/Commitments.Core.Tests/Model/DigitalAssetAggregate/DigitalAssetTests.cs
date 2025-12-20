// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Model.DigitalAssetAggregate;
using FluentAssertions;
using Xunit;

namespace Commitments.Core.Tests.AggregateModel.DigitalAssetAggregate;

public class DigitalAssetTests
{
    [Fact]
    public void DigitalAsset_ShouldSetDigitalAssetId()
    {
        // Arrange
        var digitalAsset = new DigitalAsset();
        var id = Guid.NewGuid();

        // Act
        digitalAsset.DigitalAssetId = id;

        // Assert
        digitalAsset.DigitalAssetId.Should().Be(id);
    }

    [Fact]
    public void DigitalAsset_ShouldSetBytes()
    {
        // Arrange
        var digitalAsset = new DigitalAsset();
        var bytes = new byte[] { 1, 2, 3, 4, 5 };

        // Act
        digitalAsset.Bytes = bytes;

        // Assert
        digitalAsset.Bytes.Should().BeEquivalentTo(bytes);
    }

    [Fact]
    public void DigitalAsset_ShouldSetContentType()
    {
        // Arrange
        var digitalAsset = new DigitalAsset();
        var contentType = "image/png";

        // Act
        digitalAsset.ContentType = contentType;

        // Assert
        digitalAsset.ContentType.Should().Be(contentType);
    }

    [Fact]
    public void DigitalAsset_ShouldSetName()
    {
        // Arrange
        var digitalAsset = new DigitalAsset();
        var name = "test-image.png";

        // Act
        digitalAsset.Name = name;

        // Assert
        digitalAsset.Name.Should().Be(name);
    }

    [Fact]
    public void DigitalAsset_ShouldAllowNullBytes()
    {
        // Arrange
        var digitalAsset = new DigitalAsset();

        // Act
        digitalAsset.Bytes = null;

        // Assert
        digitalAsset.Bytes.Should().BeNull();
    }

    [Fact]
    public void DigitalAsset_ShouldAllowNullContentType()
    {
        // Arrange
        var digitalAsset = new DigitalAsset();

        // Act
        digitalAsset.ContentType = null;

        // Assert
        digitalAsset.ContentType.Should().BeNull();
    }

    [Fact]
    public void DigitalAsset_ShouldAllowNullName()
    {
        // Arrange
        var digitalAsset = new DigitalAsset();

        // Act
        digitalAsset.Name = null;

        // Assert
        digitalAsset.Name.Should().BeNull();
    }

    [Fact]
    public void DigitalAsset_ShouldHandleLargeByteArray()
    {
        // Arrange
        var digitalAsset = new DigitalAsset();
        var bytes = new byte[1000000]; // 1MB

        // Act
        digitalAsset.Bytes = bytes;

        // Assert
        digitalAsset.Bytes.Should().HaveCount(1000000);
    }
}
