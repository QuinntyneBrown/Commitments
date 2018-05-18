using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Commitments.Core.Identity
{
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
            var token = _tokenProvider.Get("quinntynebrown@gmail.com");
            httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");
            await _next.Invoke(httpContext);
        }
    }
}
