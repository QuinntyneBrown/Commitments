using Asp.Versioning;
using Dashboard.Features.Card;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace Dashboard.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cards")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class CardController
{
    private readonly ISender _sender;
    private readonly ILogger<CardController> _logger;

    public CardController(ISender sender, ILogger<CardController> logger)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(Summary = "Update Card")]
    [HttpPut(Name = "updateCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateCardResponse>> Update([FromBody] UpdateCardRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(Summary = "Create Card")]
    [HttpPost(Name = "createCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<CreateCardResponse>> Create([FromBody] CreateCardRequest request, CancellationToken cancellationToken)
    {
        return await _sender.Send(request, cancellationToken);
    }

    [SwaggerOperation(Summary = "Get Cards")]
    [HttpGet(Name = "getCards")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetCardsResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetCardsResponse>> Get(CancellationToken cancellationToken)
    {
        return await _sender.Send(new GetCardsRequest(), cancellationToken);
    }

    [SwaggerOperation(Summary = "Get Card by id")]
    [HttpGet("{cardId:guid}", Name = "getCardById")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetCardByIdResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetCardByIdResponse>> GetById([FromRoute] Guid cardId, CancellationToken cancellationToken)
    {
        var request = new GetCardByIdRequest() { CardId = cardId };
        var response = await _sender.Send(request, cancellationToken);
        if (response.Card == null) return new NotFoundObjectResult(request.CardId);
        return response;
    }

    [SwaggerOperation(Summary = "Delete Card")]
    [HttpDelete("{cardId:guid}", Name = "deleteCard")]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteCardResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteCardResponse>> Delete([FromRoute] Guid cardId, CancellationToken cancellationToken)
    {
        return await _sender.Send(new DeleteCardRequest() { CardId = cardId }, cancellationToken);
    }
}
