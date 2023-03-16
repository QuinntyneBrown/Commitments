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
    public async Task<ActionResult<SaveCardLayoutCommandResponse>> Save(SaveCardLayoutCommandRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{cardLayoutId}")]
    public async Task Remove([FromRoute]RemoveCardLayoutCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{cardLayoutId}")]
    public async Task<ActionResult<GetCardLayoutByIdQueryResponse>> GetById([FromRoute]GetCardLayoutByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetCardLayoutsQueryResponse>> Get()
        => await _mediator.Send(new GetCardLayoutsQueryRequest());
}
