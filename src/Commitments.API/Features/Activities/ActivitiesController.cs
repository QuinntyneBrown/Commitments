using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.Activities
{
    [Authorize]
    [ApiController]
    [Route("api/activities")]
    public class ActivitiesController
    {
        private readonly IMediator _mediator;

        public ActivitiesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveActivityCommand.Response>> Save(SaveActivityCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{Activity.ActivityId}")]
        public async Task Remove([FromRoute]RemoveActivityCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{ActivityId}")]
        public async Task<ActionResult<GetActivityByIdQuery.Response>> GetById([FromRoute]GetActivityByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetActivitiesQuery.Response>> Get()
            => await _mediator.Send(new GetActivitiesQuery.Request());
    }
}
