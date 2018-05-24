using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Commitments.Core.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static int GetProfileIdFromClaims(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext.User.Claims;
            return Convert.ToInt16(claims.Single(x => x.Type == "ProfileId").Value);
        }
    }
}
