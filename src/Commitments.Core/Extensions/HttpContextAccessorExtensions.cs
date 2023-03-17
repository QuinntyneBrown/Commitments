// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using System;
using System.Linq;


namespace Commitments.Core.Extensions;

public static class HttpContextAccessorExtensions
{
    public static int GetProfileIdFromClaims(this IHttpContextAccessor httpContextAccessor)
    {
        var claims = httpContextAccessor.HttpContext.User.Claims;
        return Convert.ToInt16(claims.Single(x => x.Type == "ProfileId").Value);
    }
}

