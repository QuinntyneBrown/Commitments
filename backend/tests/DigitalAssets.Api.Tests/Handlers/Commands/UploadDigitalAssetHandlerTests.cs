using DigitalAssets.Features.DigitalAsset;
using DigitalAssets.Data;
using DigitalAssets.Domain.DigitalAssetAggregate;
using Commitments.Testing.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace DigitalAssets.Api.Tests.Handlers.Commands;

public class UploadDigitalAssetHandlerTests
{
    private readonly Mock<IDigitalAssetsDbContext> _mockContext;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly UploadDigitalAssetRequestHandler _handler;

    public UploadDigitalAssetHandlerTests()
    {
        _mockContext = MockDigitalAssetsDbContextFactory.Create();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _handler = new UploadDigitalAssetRequestHandler(_mockContext.Object, _mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task Handle_WithSingleFile_ShouldUploadAndReturnDto()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("test.png");
        fileMock.Setup(f => f.ContentType).Returns("image/png");
        fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((s, ct) =>
            {
                var bytes = new byte[] { 1, 2, 3 };
                s.Write(bytes, 0, bytes.Length);
            })
            .Returns(Task.CompletedTask);

        var formFileCollection = new FormFileCollection { fileMock.Object };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        var request = new UploadDigitalAssetRequest();

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAssets.Should().HaveCount(1);
        response.DigitalAssets[0].Name.Should().Be("test.png");
        response.DigitalAssets[0].ContentType.Should().Be("image/png");
        response.DigitalAssets[0].Bytes.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
        _mockContext.Verify(c => c.DigitalAssets.Add(It.IsAny<DigitalAsset>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultipleFiles_ShouldUploadAllAndReturnDtos()
    {
        var fileMock1 = new Mock<IFormFile>();
        fileMock1.Setup(f => f.FileName).Returns("file1.png");
        fileMock1.Setup(f => f.ContentType).Returns("image/png");
        fileMock1.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((s, ct) =>
            {
                var bytes = new byte[] { 1 };
                s.Write(bytes, 0, bytes.Length);
            })
            .Returns(Task.CompletedTask);

        var fileMock2 = new Mock<IFormFile>();
        fileMock2.Setup(f => f.FileName).Returns("file2.jpg");
        fileMock2.Setup(f => f.ContentType).Returns("image/jpeg");
        fileMock2.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .Callback<Stream, CancellationToken>((s, ct) =>
            {
                var bytes = new byte[] { 2, 3 };
                s.Write(bytes, 0, bytes.Length);
            })
            .Returns(Task.CompletedTask);

        var formFileCollection = new FormFileCollection { fileMock1.Object, fileMock2.Object };
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        var request = new UploadDigitalAssetRequest();

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAssets.Should().HaveCount(2);
        response.DigitalAssets[0].Name.Should().Be("file1.png");
        response.DigitalAssets[1].Name.Should().Be("file2.jpg");
        _mockContext.Verify(c => c.DigitalAssets.Add(It.IsAny<DigitalAsset>()), Times.Exactly(2));
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNoFiles_ShouldReturnEmptyList()
    {
        var formFileCollection = new FormFileCollection();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);
        _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(httpContext);

        var request = new UploadDigitalAssetRequest();

        var response = await _handler.Handle(request, CancellationToken.None);

        response.Should().NotBeNull();
        response.DigitalAssets.Should().BeEmpty();
        _mockContext.Verify(c => c.DigitalAssets.Add(It.IsAny<DigitalAsset>()), Times.Never);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
