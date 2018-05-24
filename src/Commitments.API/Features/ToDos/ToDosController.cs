using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.ToDos
{
    [Authorize]
    [ApiController]
    [Route("api/toDos")]
    public class ToDosController
    {
        private readonly IMediator _mediator;

        public ToDosController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveToDoCommand.Response>> Save(SaveToDoCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{toDoId}")]
        public async Task Remove([FromRoute]RemoveToDoCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{toDoId}")]
        public async Task<ActionResult<GetToDoByIdQuery.Response>> GetById([FromRoute]GetToDoByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetToDosQuery.Response>> Get()
            => await _mediator.Send(new GetToDosQuery.Request());
    }
}
