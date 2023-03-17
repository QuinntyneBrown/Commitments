// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Cards;

[Authorize]
[ApiController]
[Route("api/cards")]
public class CardsController
{
    private readonly IMediator _mediator;

    public CardsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveCardResponse>> Save(SaveCardRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{cardId}")]
    public async Task Remove([FromRoute]RemoveCardRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{cardId}")]
    public async Task<ActionResult<GetCardByIdResponse>> GetById([FromRoute]GetCardByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetCardsResponse>> Get()
        => await _mediator.Send(new GetCardsRequest());
}

