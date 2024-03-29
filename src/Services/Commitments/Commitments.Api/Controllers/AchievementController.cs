// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.AchievementAggregate.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Commitments.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class AchievementController
{
    private readonly ISender _sender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AchievementController(IHttpContextAccessor httpContextAccessor, ISender sender)
    {
        _sender = sender;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetAchievementsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetAchievementsResponse>> Get()
    {
        return await _sender.Send(new GetAchievementsRequest()
        {
            ProfileId = _httpContextAccessor.GetProfileId()
        });
    }
}