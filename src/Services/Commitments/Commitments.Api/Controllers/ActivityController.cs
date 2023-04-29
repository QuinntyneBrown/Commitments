// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.ActivityAggregate.Commands;
using Commitments.Core.AggregateModel.ActivityAggregate.Queries;
using MediatR;
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
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ActivityController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SaveActivityResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveActivityResponse>> Save(SaveActivityRequest request)
    {
        request.Activity.ProfileId = _httpContextAccessor.GetProfileId();
        return await _mediator.Send(request);
    }

    [HttpDelete("{activityId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task Remove([FromRoute] RemoveActivityRequest request)
        => await _mediator.Send(request);

    [HttpGet("{ActivityId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetActivityByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetActivityByIdResponse>> GetById([FromRoute] GetActivityByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetActivitiesResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetActivitiesResponse>> Get()
        => await _mediator.Send(new GetActivitiesRequest());

}