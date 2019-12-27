using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.Api.Features.FrequencyTypes
{
    [Authorize]
    [ApiController]
    [Route("api/frequencyTypes")]
    public class FrequencyTypesController
    {
        private readonly IMediator _mediator;

        public FrequencyTypesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveFrequencyTypeCommand.Response>> Save(SaveFrequencyTypeCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{frequencyTypeId}")]
        public async Task Remove(RemoveFrequencyTypeCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{frequencyTypeId}")]
        public async Task<ActionResult<GetFrequencyTypeByIdQuery.Response>> GetById([FromRoute]GetFrequencyTypeByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetFrequencyTypesQuery.Response>> Get()
            => await _mediator.Send(new GetFrequencyTypesQuery.Request());
    }
}
