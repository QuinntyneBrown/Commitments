// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.FrequencyTypes;

[Authorize]
[ApiController]
[Route("api/frequencyTypes")]
public class FrequencyTypesController
{
    private readonly IMediator _mediator;

    public FrequencyTypesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveFrequencyTypeResponse>> Save(SaveFrequencyTypeRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{frequencyTypeId}")]
    public async Task Remove(RemoveFrequencyTypeRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{frequencyTypeId}")]
    public async Task<ActionResult<GetFrequencyTypeByIdResponse>> GetById([FromRoute]GetFrequencyTypeByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetFrequencyTypesResponse>> Get()
        => await _mediator.Send(new GetFrequencyTypesRequest());
}

