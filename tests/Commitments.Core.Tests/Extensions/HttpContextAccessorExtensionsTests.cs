// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Commitments.Core.Tests.Extensions;

public class HttpContextAccessorExtensionsTests
{
    [Fact]
    public void GetProfileId_ShouldReturnProfileIdFromClaims()
    {
        // Arrange
        var profileId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new Claim("ProfileId", profileId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.User).Returns(principal);

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        // Act
        var result = mockHttpContextAccessor.Object.GetProfileId();

        // Assert
        result.Should().Be(profileId);
    }

    [Fact]
    public void GetProfileId_ShouldReturnEmptyGuidWhenNoProfileIdClaim()
    {
        // Arrange
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.User).Returns(principal);

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        // Act
        var result = mockHttpContextAccessor.Object.GetProfileId();

        // Assert
        result.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetProfileId_ShouldReturnEmptyGuidWhenHttpContextIsNull()
    {
        // Arrange
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns((HttpContext?)null);

        // Act
        var result = mockHttpContextAccessor.Object.GetProfileId();

        // Assert
        result.Should().Be(Guid.Empty);
    }

    [Fact]
    public void GetProfileId_ShouldReturnEmptyGuidWhenClaimValueIsInvalid()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim("ProfileId", "invalid-guid")
        };
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(c => c.User).Returns(principal);

        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

        // Act
        var result = mockHttpContextAccessor.Object.GetProfileId();

        // Assert
        result.Should().Be(Guid.Empty);
    }
}
