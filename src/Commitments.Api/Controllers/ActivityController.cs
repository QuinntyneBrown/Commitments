// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.ActivityAggregate.Commands;
using Commitments.Core.AggregateModel.ActivityAggregate.Queries;
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
public class ActivityController
{
    private readonly ISender _sender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ActivityController(IHttpContextAccessor httpContextAccessor, ISender sender)
    {
        _sender = sender;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SaveActivityResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveActivityResponse>> Save(SaveActivityRequest request)
    {
        request.Activity.ProfileId = _httpContextAccessor.GetProfileId();
        return await _sender.Send(request);
    }

    [HttpDelete("{activityId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task Remove([FromRoute] RemoveActivityRequest request)
        => await _sender.Send(request);

    [HttpGet("{ActivityId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetActivityByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetActivityByIdResponse>> GetById([FromRoute] GetActivityByIdRequest request)
        => await _sender.Send(request);

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetActivitiesResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetActivitiesResponse>> Get()
        => await _sender.Send(new GetActivitiesRequest());

}