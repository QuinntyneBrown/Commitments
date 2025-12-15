// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Security.Claims;

namespace Commitments.Core.Services.Security;

public interface ITokenProvider
{
    string Get(string username, IEnumerable<Claim> customClaims = null);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    string GenerateRefreshToken();
}
