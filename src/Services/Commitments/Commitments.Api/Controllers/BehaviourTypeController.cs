// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.BehaviourTypeAggregate.Commands;
using Commitments.Core.AggregateModel.BehaviourTypeAggregate.Queries;
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
public class BehaviourTypeController
{
    private readonly ISender _sender;

    public BehaviourTypeController(ISender sender) => _sender = sender;

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SaveBehaviourTypeResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveBehaviourTypeResponse>> Save(SaveBehaviourTypeRequest request)
        => await _sender.Send(request);

    [HttpDelete("{BehaviourTypeId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task Remove([FromRoute] RemoveBehaviourTypeRequest request)
        => await _sender.Send(request);

    [HttpGet("{BehaviourTypeId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetBehaviourTypeByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetBehaviourTypeByIdResponse>> GetById([FromRoute] GetBehaviourTypeByIdRequest request)
        => await _sender.Send(request);

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetBehaviourTypesResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetBehaviourTypesResponse>> Get()
        => await _sender.Send(new GetBehaviourTypesRequest());
}