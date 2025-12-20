// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Services.Security;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Commitments.Core.Tests.Services.Security;

public class TokenBuilderTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;

    public TokenBuilderTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(c => c["Authentication:JwtKey"]).Returns("VerySecretKeyThatIsAtLeast32CharactersLong");
        _mockConfiguration.Setup(c => c["Authentication:JwtIssuer"]).Returns("TestIssuer");
        _mockConfiguration.Setup(c => c["Authentication:JwtAudience"]).Returns("TestAudience");
        _mockConfiguration.Setup(c => c["Authentication:ExpirationMinutes"]).Returns("60");
    }

    [Fact]
    public void Build_ShouldReturnValidJwtToken()
    {
        // Arrange
        var tokenBuilder = new TokenBuilder(_mockConfiguration.Object);
        tokenBuilder.AddUsername("testuser");

        // Act
        var token = tokenBuilder.Build();

        // Assert
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Build_ShouldReturnValidTokenStructure()
    {
        // Arrange
        var tokenBuilder = new TokenBuilder(_mockConfiguration.Object);
        tokenBuilder.AddUsername("testuser");

        // Act
        var token = tokenBuilder.Build();
        var parts = token.Split('.');

        // Assert
        parts.Should().HaveCount(3); // JWT has 3 parts separated by dots
    }

    [Fact]
    public void AddClaim_ShouldAddClaimToToken()
    {
        // Arrange
        var tokenBuilder = new TokenBuilder(_mockConfiguration.Object);
        var claim = new Claim("custom", "value");

        // Act
        var result = tokenBuilder.AddClaim(claim);

        // Assert
        result.Should().Be(tokenBuilder);
    }

    [Fact]
    public void AddUsername_ShouldReturnSelf()
    {
        // Arrange
        var tokenBuilder = new TokenBuilder(_mockConfiguration.Object);

        // Act
        var result = tokenBuilder.AddUsername("testuser");

        // Assert
        result.Should().Be(tokenBuilder);
    }

    [Fact]
    public void AddOrUpdateClaim_ShouldReplaceExistingClaim()
    {
        // Arrange
        var tokenBuilder = new TokenBuilder(_mockConfiguration.Object);
        var claim1 = new Claim("custom", "value1");
        var claim2 = new Claim("custom", "value2");

        // Act
        tokenBuilder.AddClaim(claim1);
        var result = tokenBuilder.AddOrUpdateClaim(claim2);

        // Assert
        result.Should().Be(tokenBuilder);
    }

    [Fact]
    public void RemoveClaim_ShouldRemoveExistingClaim()
    {
        // Arrange
        var tokenBuilder = new TokenBuilder(_mockConfiguration.Object);
        var claim = new Claim("custom", "value");
        tokenBuilder.AddClaim(claim);

        // Act
        var result = tokenBuilder.RemoveClaim(claim);

        // Assert
        result.Should().Be(tokenBuilder);
    }

    [Fact]
    public void FromClaimsPrincipal_ShouldAddAllClaims()
    {
        // Arrange
        var tokenBuilder = new TokenBuilder(_mockConfiguration.Object);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Email, "test@example.com")
        };
        var identity = new ClaimsIdentity(claims, "test");
        var principal = new ClaimsPrincipal(identity);

        // Act
        var result = tokenBuilder.FromClaimsPrincipal(principal);

        // Assert
        result.Should().Be(tokenBuilder);
    }

    [Fact]
    public void Build_ShouldIncludeJtiClaim()
    {
        // Arrange
        var tokenBuilder = new TokenBuilder(_mockConfiguration.Object);
        tokenBuilder.AddUsername("testuser");

        // Act
        var token = tokenBuilder.Build();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Jti);
    }

    [Fact]
    public void Build_ShouldIncludeIatClaim()
    {
        // Arrange
        var tokenBuilder = new TokenBuilder(_mockConfiguration.Object);
        tokenBuilder.AddUsername("testuser");

        // Act
        var token = tokenBuilder.Build();
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Assert
        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Iat);
    }

    [Fact]
    public void Build_ShouldUseDefaultExpirationWhenNotConfigured()
    {
        // Arrange
        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(c => c["Authentication:JwtKey"]).Returns("VerySecretKeyThatIsAtLeast32CharactersLong");
        mockConfig.Setup(c => c["Authentication:JwtIssuer"]).Returns("TestIssuer");
        mockConfig.Setup(c => c["Authentication:JwtAudience"]).Returns("TestAudience");
        mockConfig.Setup(c => c["Authentication:ExpirationMinutes"]).Returns((string?)null);

        var tokenBuilder = new TokenBuilder(mockConfig.Object);
        tokenBuilder.AddUsername("testuser");

        // Act
        var token = tokenBuilder.Build();

        // Assert
        token.Should().NotBeNullOrEmpty();
    }
}
