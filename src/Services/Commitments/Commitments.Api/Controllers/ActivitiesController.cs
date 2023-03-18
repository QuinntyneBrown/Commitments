// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.ActivityAggregate.Commands;
using Commitments.Core.AggregateModel.ActivityAggregate.Queries;
using Commitments.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ActivitiesController
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ActivitiesController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<ActionResult<SaveActivityResponse>> Save(SaveActivityRequest request)
    {
        request.Activity.ProfileId = _httpContextAccessor.GetProfileId();
        return await _mediator.Send(request);
    }

    [HttpDelete("{activityId}")]
    public async Task Remove([FromRoute] RemoveActivityRequest request)
        => await _mediator.Send(request);

    [HttpGet("{ActivityId}")]
    public async Task<ActionResult<GetActivityByIdResponse>> GetById([FromRoute] GetActivityByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetActivitiesResponse>> Get()
        => await _mediator.Send(new GetActivitiesRequest());

}

