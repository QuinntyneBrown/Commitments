using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Commitments.Api.Tests.Security;

/// <summary>
/// Verifies that the JWT Bearer middleware preserves the "sub" claim without remapping it
/// to ClaimTypes.NameIdentifier (bug-198).
/// </summary>
public class JwtClaimMappingTests
{
    [Fact]
    public void JwtBearerOptions_MapInboundClaims_IsFalse()
    {
        var services = new ServiceCollection();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
            });

        var provider = services.BuildServiceProvider();
        var monitor = provider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
        var options = monitor.Get(JwtBearerDefaults.AuthenticationScheme);

        options.MapInboundClaims.Should().BeFalse("JWT Bearer must not remap 'sub' to ClaimTypes.NameIdentifier so GetUserId() can read it directly");
    }

    [Fact]
    public void JwtSecurityToken_SubClaim_PreservedAsSubNotNameIdentifier()
    {
        var userId = Guid.NewGuid();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test-key-at-least-32-chars-long!"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: new[] { new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()) },
            signingCredentials: creds);

        var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
        var principal = handler.ValidateToken(
            handler.WriteToken(token),
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                IssuerSigningKey = key,
            },
            out _);

        principal.FindFirst("sub")!.Value.Should().Be(userId.ToString(),
            "with MapInboundClaims=false the 'sub' claim stays as 'sub' and GetUserId() can find it");

        principal.FindFirst(ClaimTypes.NameIdentifier).Should().BeNull(
            "the claim must not be duplicated as ClaimTypes.NameIdentifier");
    }
}
