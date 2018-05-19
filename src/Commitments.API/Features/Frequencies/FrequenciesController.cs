using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.Frequencies
{
    [Authorize]
    [ApiController]
    [Route("api/frequencies")]
    public class FrequenciesController
    {
        private readonly IMediator _mediator;

        public FrequenciesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveFrequencyCommand.Response>> Save(SaveFrequencyCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{Frequency.FrequencyId}")]
        public async Task Remove([FromRoute]RemoveFrequencyCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{FrequencyId}")]
        public async Task<ActionResult<GetFrequencyByIdQuery.Response>> GetById([FromRoute]GetFrequencyByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetFrequenciesQuery.Response>> Get()
            => await _mediator.Send(new GetFrequenciesQuery.Request());
    }
}
