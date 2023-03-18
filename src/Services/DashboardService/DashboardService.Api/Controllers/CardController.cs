// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DashboardService.Core.AggregateModel.CardAggregate.Commands;
using DashboardService.Core.AggregateModel.CardAggregate.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace DashboardService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class CardController
{
    private readonly IMediator _mediator;

    private readonly ILogger<CardController> _logger;

    public CardController(IMediator mediator, ILogger<CardController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(
        Summary = "Update Card",
        Description = @"Update Card"
    )]
    [HttpPut(Name = "updateCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateCardResponse>> Update([FromBody] UpdateCardRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Create Card",
        Description = @"Create Card"
    )]
    [HttpPost(Name = "createCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateCardResponse>> Create([FromBody] CreateCardRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Cards",
        Description = @"Get Cards"
    )]
    [HttpGet(Name = "getCards")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetCardsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetCardsResponse>> Get(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetCardsRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get CardId  by id",
        Description = @"Get CardId by id"
    )]
    [HttpGet("{toDoId:guid}", Name = "getCardIdById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetCardByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetCardByIdResponse>> GetById([FromRoute] Guid cardId, CancellationToken cancellationToken)
    {
        var request = new GetCardByIdRequest() { CardId = cardId };

        var response = await _mediator.Send(request, cancellationToken);

        if (response.Card == null)
        {
            return new NotFoundObjectResult(request.CardId);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Delete Card",
        Description = @"Delete Card"
    )]
    [HttpDelete("{toDoId:guid}", Name = "deleteCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteCardResponse>> Delete([FromRoute] Guid cardId, CancellationToken cancellationToken)
    {
        var request = new DeleteCardRequest() { CardId = cardId };

        return await _mediator.Send(request, cancellationToken);
    }

}


