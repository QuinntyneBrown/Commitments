// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.CardLayouts;

[Authorize]
[ApiController]
[Route("api/cardLayouts")]
public class CardLayoutsController
{
    private readonly IMediator _mediator;

    public CardLayoutsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveCardLayoutResponse>> Save(SaveCardLayoutRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{cardLayoutId}")]
    public async Task Remove([FromRoute]RemoveCardLayoutRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{cardLayoutId}")]
    public async Task<ActionResult<GetCardLayoutByIdResponse>> GetById([FromRoute]GetCardLayoutByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetCardLayoutsResponse>> Get()
        => await _mediator.Send(new GetCardLayoutsRequest());
}

