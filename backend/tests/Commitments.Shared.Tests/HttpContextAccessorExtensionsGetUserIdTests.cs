using System.Security.Claims;
using Commitments.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Commitments.Shared.Tests;

public class HttpContextAccessorExtensionsGetUserIdTests
{
    [Fact]
    public void GetUserId_ValidSubClaim_ReturnsCorrectGuid()
    {
        var expectedId = Guid.NewGuid();
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("sub", expectedId.ToString())
            }))
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var result = accessor.Object.GetUserId();

        result.Should().Be(expectedId);
    }

    [Fact]
    public void GetUserId_MissingSubClaim_Throws()
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity())
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var act = () => accessor.Object.GetUserId();

        act.Should().Throw<FormatException>();
    }

    [Fact]
    public void GetUserId_InvalidSubClaim_Throws()
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("sub", "not-a-guid")
            }))
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var act = () => accessor.Object.GetUserId();

        act.Should().Throw<FormatException>();
    }
}
