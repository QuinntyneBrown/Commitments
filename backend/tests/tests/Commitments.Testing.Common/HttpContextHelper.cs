using Microsoft.AspNetCore.Http;

namespace Commitments.Testing.Common;

public static class HttpContextHelper
{
    public static DefaultHttpContext CreateWithProfileId(Guid profileId)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["ProfileId"] = profileId.ToString();
        return httpContext;
    }
}
