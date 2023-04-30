// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace IdentityService.Core.Messages;

public class UserCreateMessage: IRequest
{
    public UserCreateMessage(string username, string password)
    {
        Username = username;
        Password = password;
    }
    public string Username { get; set; }
    public string Password { get; set; }
}


