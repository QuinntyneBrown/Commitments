// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.FrequencyTypeAggregate.Commands;
using Commitments.Core.AggregateModel.FrequencyTypeAggregate.Queries;
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
    private readonly ISender _sender;

    public FrequencyTypeController(ISender sender) => _sender = sender;

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(SaveFrequencyTypeResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveFrequencyTypeResponse>> Save(SaveFrequencyTypeRequest request)
        => await _sender.Send(request);

    [HttpDelete("{frequencyTypeId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task Remove(RemoveFrequencyTypeRequest request)
        => await _sender.Send(request);

    [HttpGet("{frequencyTypeId}")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetFrequencyTypeByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetFrequencyTypeByIdResponse>> GetById([FromRoute] GetFrequencyTypeByIdRequest request)
        => await _sender.Send(request);

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetFrequencyTypesResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetFrequencyTypesResponse>> Get()
        => await _sender.Send(new GetFrequencyTypesRequest());
}