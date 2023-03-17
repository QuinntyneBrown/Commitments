// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.BehaviourTypes;

[Authorize]
[ApiController]
[Route("api/behaviourTypes")]
public class BehaviourTypesController
{
    private readonly IMediator _mediator;

    public BehaviourTypesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveBehaviourTypeResponse>> Save(SaveBehaviourTypeRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{BehaviourTypeId}")]
    public async Task Remove([FromRoute]RemoveBehaviourTypeRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{BehaviourTypeId}")]
    public async Task<ActionResult<GetBehaviourTypeByIdResponse>> GetById([FromRoute]GetBehaviourTypeByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetBehaviourTypesResponse>> Get()
        => await _mediator.Send(new GetBehaviourTypesRequest());
}

