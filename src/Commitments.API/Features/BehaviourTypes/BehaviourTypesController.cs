using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.BehaviourTypes
{
    [Authorize]
    [ApiController]
    [Route("api/behaviourTypes")]
    public class BehaviourTypesController
    {
        private readonly IMediator _mediator;

        public BehaviourTypesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveBehaviourTypeCommand.Response>> Save(SaveBehaviourTypeCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{BehaviourTypeId}")]
        public async Task Remove([FromRoute]RemoveBehaviourTypeCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{BehaviourTypeId}")]
        public async Task<ActionResult<GetBehaviourTypeByIdQuery.Response>> GetById([FromRoute]GetBehaviourTypeByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetBehaviourTypesQuery.Response>> Get()
            => await _mediator.Send(new GetBehaviourTypesQuery.Request());
    }
}
