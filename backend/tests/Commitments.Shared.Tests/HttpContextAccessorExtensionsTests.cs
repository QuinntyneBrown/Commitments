using Commitments.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Commitments.Shared.Tests;

public class HttpContextAccessorExtensionsTests
{
    [Fact]
    public void GetProfileId_ValidHeader_ReturnsCorrectGuid()
    {
        var expectedId = Guid.NewGuid();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = expectedId.ToString();

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var result = accessor.Object.GetProfileId();

        result.Should().Be(expectedId);
    }

    [Fact]
    public void GetProfileId_MissingHeader_Throws()
    {
        var httpContext = new DefaultHttpContext();
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var act = () => accessor.Object.GetProfileId();

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void GetProfileId_InvalidHeader_Throws()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = "not-a-guid";
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var act = () => accessor.Object.GetProfileId();

        act.Should().Throw<FormatException>();
    }
}
