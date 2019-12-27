using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.Api.Features.Behaviours
{
    [Authorize]
    [ApiController]
    [Route("api/behaviours")]
    public class BehavioursController
    {
        private readonly IMediator _mediator;

        public BehavioursController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveBehaviourCommand.Response>> Save(SaveBehaviourCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{behaviourId}")]
        public async Task Remove([FromRoute]RemoveBehaviourCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{behaviourId}")]
        public async Task<ActionResult<GetBehaviourByIdQuery.Response>> GetById([FromRoute]GetBehaviourByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetBehavioursQuery.Response>> Get()
            => await _mediator.Send(new GetBehavioursQuery.Request());
    }
}
