// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Features.GoalProgress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Commitments.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/goal-progress")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class GoalProgressController
{
    private readonly ISender _sender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GoalProgressController(IHttpContextAccessor httpContextAccessor, ISender sender)
    {
        _sender = sender;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("current")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetGoalProgressResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetGoalProgressResponse>> GetCurrent([FromQuery] Guid goalId)
    {
        return await _sender.Send(new GetGoalProgressRequest
        {
            GoalId = goalId,
            ProfileId = _httpContextAccessor.GetProfileId()
        });
    }

    [HttpGet("at")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetGoalProgressResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetGoalProgressResponse>> GetAt([FromQuery] Guid goalId, [FromQuery] DateTimeOffset asOf)
    {
        return await _sender.Send(new GetGoalProgressAtRequest
        {
            GoalId = goalId,
            ProfileId = _httpContextAccessor.GetProfileId(),
            AsOf = asOf
        });
    }

    [HttpGet("last14")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetGoalProgressLast14Response), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetGoalProgressLast14Response>> GetLast14([FromQuery] Guid goalId)
    {
        return await _sender.Send(new GetGoalProgressLast14Request
        {
            GoalId = goalId,
            ProfileId = _httpContextAccessor.GetProfileId()
        });
    }
}
