// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace Commitments.Core.Hubs;

[Authorize(AuthenticationSchemes = "Bearer")]
public class CommitmentsHub : Hub<ICommitmentsClient>
{

}