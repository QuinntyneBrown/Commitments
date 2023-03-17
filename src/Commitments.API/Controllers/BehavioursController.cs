using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Behaviours;

[Authorize]
[ApiController]
[Route("api/behaviours")]
public class BehavioursController
{
    private readonly IMediator _mediator;

    public BehavioursController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<SaveBehaviourResponse>> Save(SaveBehaviourRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{behaviourId}")]
    public async Task Remove([FromRoute]RemoveBehaviourRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{behaviourId}")]
    public async Task<ActionResult<GetBehaviourByIdResponse>> GetById([FromRoute]GetBehaviourByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetBehavioursResponse>> Get()
        => await _mediator.Send(new GetBehavioursRequest());
}
