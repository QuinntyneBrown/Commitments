// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ProfileService.Core.Messages;

public class UserValidatedMessage : IRequest
{
    public string Username { get; set; }
}


