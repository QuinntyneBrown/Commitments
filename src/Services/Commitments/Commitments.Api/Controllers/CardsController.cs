// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.AggregateModel.CardAggregate.Commands;
using Commitments.Core.AggregateModel.CardAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Commitments.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class CardController
{
    private readonly IMediator _mediator;

    public CardController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveCardResponse>> Save(SaveCardRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{cardId}")]
    public async Task Remove([FromRoute] RemoveCardRequest request)
        => await _mediator.Send(request);

    [HttpGet("{cardId}")]
    public async Task<ActionResult<GetCardByIdResponse>> GetById([FromRoute] GetCardByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetCardsResponse>> Get()
        => await _mediator.Send(new GetCardsRequest());
}

