// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace DashboardService.Core.Messages;

public class UserCreatedMessage : IRequest
{
    public string Username { get; set; } = null!;
}