// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace IdentityService.Core.Messages;

public class UserCreatedMessage: IRequest
{
    public required string Username { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}


