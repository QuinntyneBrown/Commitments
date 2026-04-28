using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Security;
using Microsoft.IdentityModel.Tokens;

namespace Commitments.Api.Security;

public class JwtAccessTokenIssuer : IAccessTokenIssuer
{
    private readonly IConfiguration _configuration;

    public JwtAccessTokenIssuer(IConfiguration configuration) => _configuration = configuration;

    public string Issue(Guid userId, string username)
    {
        var auth = _configuration.GetSection("Authentication");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(auth["JwtKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: auth["JwtIssuer"],
            audience: auth["JwtAudience"],
            claims: new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("username", username)
            },
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
