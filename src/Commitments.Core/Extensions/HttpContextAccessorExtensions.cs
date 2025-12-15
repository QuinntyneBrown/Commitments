// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace Commitments.Core.Extensions;

public static class HttpContextAccessorExtensions
{
    public static Guid GetProfileId(this IHttpContextAccessor httpContextAccessor)
    {
        var profileId = $"{httpContextAccessor.HttpContext.Request.Headers["ProfileId"]}";

        return new Guid(profileId);
    }
}