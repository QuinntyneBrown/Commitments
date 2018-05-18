using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.CommitmentFrequencies
{
    [Authorize]
    [ApiController]
    [Route("api/commitmentFrequencies")]
    public class CommitmentFrequenciesController
    {
        private readonly IMediator _mediator;

        public CommitmentFrequenciesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveCommitmentFrequencyCommand.Response>> Save(SaveCommitmentFrequencyCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{CommitmentFrequency.CommitmentFrequencyId}")]
        public async Task Remove([FromRoute]RemoveCommitmentFrequencyCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{CommitmentFrequencyId}")]
        public async Task<ActionResult<GetCommitmentFrequencyByIdQuery.Response>> GetById([FromRoute]GetCommitmentFrequencyByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCommitmentFrequenciesQuery.Response>> Get()
            => await _mediator.Send(new GetCommitmentFrequenciesQuery.Request());
    }
}
