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
    public async Task<ActionResult<SaveCardCommandResponse>> Save(SaveCardCommandRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{cardId}")]
    public async Task Remove([FromRoute]RemoveCardCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{cardId}")]
    public async Task<ActionResult<GetCardByIdQueryResponse>> GetById([FromRoute]GetCardByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetCardsQueryResponse>> Get()
        => await _mediator.Send(new GetCardsQueryRequest());
}
