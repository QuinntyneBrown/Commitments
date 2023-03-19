// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.BehaviourAggregate.Commands;
using Commitments.Core.AggregateModel.BehaviourAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;


namespace Commitments.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class BehaviourController
{
    private readonly IMediator _mediator;

    public BehaviourController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SaveBehaviourResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveBehaviourResponse>> Save(SaveBehaviourRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{behaviourId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task Remove([FromRoute] RemoveBehaviourRequest request)
        => await _mediator.Send(request);

    [HttpGet("{behaviourId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetBehaviourByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetBehaviourByIdResponse>> GetById([FromRoute] GetBehaviourByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetBehavioursResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetBehavioursResponse>> Get()
        => await _mediator.Send(new GetBehavioursRequest());
}

