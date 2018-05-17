using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.Commitments
{
    [Authorize]
    [ApiController]
    [Route("api/commitments")]
    public class CommitmentsController
    {
        private readonly IMediator _mediator;

        public CommitmentsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveCommitmentCommand.Response>> Save(SaveCommitmentCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{Commitment.CommitmentId}")]
        public async Task Remove(RemoveCommitmentCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{CommitmentId}")]
        public async Task<ActionResult<GetCommitmentByIdQuery.Response>> GetById([FromRoute]GetCommitmentByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCommitmentsQuery.Response>> Get()
            => await _mediator.Send(new GetCommitmentsQuery.Request());
    }
}
