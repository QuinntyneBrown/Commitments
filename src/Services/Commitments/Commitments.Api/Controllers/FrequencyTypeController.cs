// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.FrequencyTypeAggregate.Commands;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate.Queries;
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
public class FrequencyTypeController
{
    private readonly IMediator _mediator;

    public FrequencyTypeController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SaveFrequencyTypeResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<SaveFrequencyTypeResponse>> Save(SaveFrequencyTypeRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{frequencyTypeId}")]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task Remove(RemoveFrequencyTypeRequest request)
        => await _mediator.Send(request);

    [HttpGet("{frequencyTypeId}")]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetFrequencyTypeByIdResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<GetFrequencyTypeByIdResponse>> GetById([FromRoute] GetFrequencyTypeByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetFrequencyTypesResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<GetFrequencyTypesResponse>> Get()
        => await _mediator.Send(new GetFrequencyTypesRequest());
}