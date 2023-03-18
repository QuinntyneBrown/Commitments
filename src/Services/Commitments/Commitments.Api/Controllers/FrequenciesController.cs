// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.FrequencyAggregate.Commands;
using Commitments.Core.AggregateModel.FrequencyAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/frequencies")]
public class FrequenciesController
{
    private readonly IMediator _mediator;

    public FrequenciesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveFrequencyResponse>> Save(SaveFrequencyRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{frequencyId}")]
    public async Task Remove([FromRoute] RemoveFrequencyRequest request)
        => await _mediator.Send(request);

    [HttpGet("{frequencyId}")]
    public async Task<ActionResult<GetFrequencyByIdResponse>> GetById([FromRoute] GetFrequencyByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetFrequenciesResponse>> Get()
        => await _mediator.Send(new GetFrequenciesRequest());
}

