// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace Commitments.Core.Identity;

public class AuthenticationSettings
{
    public string TokenPath { get; set; }
    public int ExpirationMinutes { get; set; }
    public string JwtKey { get; set; }
    public string JwtIssuer { get; set; }
    public string JwtAudience { get; set; }
    public string AuthType { get; set; }
}

