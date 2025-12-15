// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Commitments.Core.Services.Security;

public class TokenBuilder : ITokenBuilder
{
    private readonly IConfiguration _configuration;
    private readonly IList<Claim> _claims = new List<Claim>();

    public TokenBuilder(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenBuilder AddClaim(Claim claim)
    {
        _claims.Add(claim);
        return this;
    }

    public TokenBuilder AddOrUpdateClaim(Claim claim)
    {
        var existing = _claims.FirstOrDefault(x => x.Type == claim.Type);
        if (existing is not null)
        {
            _claims.Remove(existing);
        }

        _claims.Add(claim);
        return this;
    }

    public TokenBuilder RemoveClaim(Claim claim)
    {
        var existing = _claims.FirstOrDefault(x => x.Type == claim.Type && x.Value == claim.Value);
        if (existing is not null)
        {
            _claims.Remove(existing);
        }

        return this;
    }

    public TokenBuilder AddUsername(string username)
    {
        AddOrUpdateClaim(new Claim(JwtRegisteredClaimNames.UniqueName, username));
        AddOrUpdateClaim(new Claim(JwtRegisteredClaimNames.Sub, username));
        return this;
    }

    public TokenBuilder FromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
    {
        foreach (var claim in claimsPrincipal.Claims)
        {
            AddOrUpdateClaim(claim);
        }

        return this;
    }

    public string Build()
    {
        var now = DateTime.UtcNow;
        var nowOffset = new DateTimeOffset(now);

        // Standard JWT claims if not already present
        if (_claims.All(c => c.Type != JwtRegisteredClaimNames.Jti))
        {
            _claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        }

        if (_claims.All(c => c.Type != JwtRegisteredClaimNames.Iat))
        {
            _claims.Add(new Claim(JwtRegisteredClaimNames.Iat, nowOffset.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));
        }

        var key = _configuration[$"{nameof(Authentication)}:{nameof(Authentication.JwtKey)}"];
        var issuer = _configuration[$"{nameof(Authentication)}:{nameof(Authentication.JwtIssuer)}"];
        var audience = _configuration[$"{nameof(Authentication)}:{nameof(Authentication.JwtAudience)}"];
        var expirationMinutesValue = _configuration[$"{nameof(Authentication)}:{nameof(Authentication.ExpirationMinutes)}"];

        var expirationMinutes = string.IsNullOrWhiteSpace(expirationMinutesValue)
            ? 60
            : Convert.ToInt32(expirationMinutesValue);

        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: _claims,
            notBefore: now,
            expires: now.AddMinutes(expirationMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
