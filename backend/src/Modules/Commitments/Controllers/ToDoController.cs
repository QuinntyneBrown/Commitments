// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Features.GoalProgress;
using Commitments.Features.ToDo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Commitments.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/todos")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class ToDoController
{
    private readonly ISender _sender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ToDoController(IHttpContextAccessor httpContextAccessor, ISender sender)
    {
        _sender = sender;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("outstanding-count")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetOutstandingToDoCountResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetOutstandingToDoCountResponse>> GetOutstandingCount(
        [FromQuery] DateTimeOffset? asOf,
        [FromQuery] bool includeToday)
    {
        var response = _httpContextAccessor.HttpContext?.Response;
        if (response is not null)
            response.Headers.CacheControl = CacheControlPolicy.For(asOf, DateTimeOffset.UtcNow);

        return await _sender.Send(new GetOutstandingToDoCountRequest
        {
            ProfileId = _httpContextAccessor.GetProfileId(),
            AsOf = asOf,
            IncludeToday = includeToday
        });
    }

    [HttpGet]
    public async Task<ActionResult<GetToDosResponse>> Get()
        => await _sender.Send(new GetToDosRequest { ProfileId = _httpContextAccessor.GetProfileId() });

    [HttpPost]
    public async Task<ActionResult<SaveToDoResponse>> Save([FromBody] SaveToDoRequest request)
    {
        request.ToDo.ProfileId = _httpContextAccessor.GetProfileId();
        return await _sender.Send(request);
    }

    [HttpDelete("{toDoId:guid}")]
    public async Task Remove([FromRoute] Guid toDoId)
        => await _sender.Send(new RemoveToDoRequest { ToDoId = toDoId });

    [HttpPost("{toDoId:guid}/complete")]
    public async Task Complete([FromRoute] Guid toDoId)
        => await _sender.Send(new CompleteToDoRequest { ToDoId = toDoId });
}
