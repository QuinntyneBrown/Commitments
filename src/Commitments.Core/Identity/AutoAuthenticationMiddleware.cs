using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Commitments.Core.Identity;

public class AutoAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenProvider _tokenProvider;
    public AutoAuthenticationMiddleware(RequestDelegate next, ITokenProvider tokenProvider) {
        _next = next;
        _tokenProvider = tokenProvider;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var profileClaim = new Claim("ProfileId", "1"); 
        var token = _tokenProvider.Get("quinntynebrown@gmail.com", new List<Claim>() { profileClaim });
        httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");
        await _next.Invoke(httpContext);
    }
}
