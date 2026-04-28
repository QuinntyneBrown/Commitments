// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace Commitments.Shared;

public static class HttpContextAccessorExtensions
{
    public static Guid GetProfileId(this IHttpContextAccessor httpContextAccessor)
    {
        var profileId = $"{httpContextAccessor.HttpContext!.Request.Headers["ProfileId"]}";

        return new Guid(profileId);
    }

    public static bool TryGetProfileId(this HttpContext context, out Guid profileId)
    {
        var raw = context.Request.Headers["ProfileId"].FirstOrDefault();
        return Guid.TryParse(raw, out profileId) && profileId != Guid.Empty;
    }

    public static Guid GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var sub = httpContextAccessor.HttpContext!.User.FindFirst("sub")?.Value;
        return new Guid(sub ?? string.Empty);
    }
}
