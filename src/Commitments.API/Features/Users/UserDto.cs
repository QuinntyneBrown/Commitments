// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel;


namespace Commitments.Api.Features.Users;

public class UserDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public static UserDto FromUser(User user)
        => new UserDto
        {
            UserId = user.UserId,
            Username = user.Username
        };
}

