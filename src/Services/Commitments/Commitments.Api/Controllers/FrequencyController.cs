// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.FrequencyAggregate.Commands;
using Commitments.Core.AggregateModel.FrequencyAggregate.Queries;
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
public class FrequencyController
{
    private readonly ISender _sender;

    public FrequencyController(ISender sender) => _sender = sender;

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SaveFrequencyResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveFrequencyResponse>> Save(SaveFrequencyRequest request)
        => await _sender.Send(request);

    [HttpDelete("{frequencyId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task Remove([FromRoute] RemoveFrequencyRequest request)
        => await _sender.Send(request);

    [HttpGet("{frequencyId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetFrequencyByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetFrequencyByIdResponse>> GetById([FromRoute] GetFrequencyByIdRequest request)
        => await _sender.Send(request);

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetFrequenciesResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetFrequenciesResponse>> Get()
        => await _sender.Send(new GetFrequenciesRequest());
}