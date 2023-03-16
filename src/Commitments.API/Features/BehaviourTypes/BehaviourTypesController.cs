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
    public async Task<ActionResult<SaveBehaviourTypeCommandResponse>> Save(SaveBehaviourTypeCommandRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{BehaviourTypeId}")]
    public async Task Remove([FromRoute]RemoveBehaviourTypeCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{BehaviourTypeId}")]
    public async Task<ActionResult<GetBehaviourTypeByIdQueryResponse>> GetById([FromRoute]GetBehaviourTypeByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetBehaviourTypesQueryResponse>> Get()
        => await _mediator.Send(new GetBehaviourTypesQueryRequest());
}
