using Commitments.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommitmentsController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<ActionResult<SaveCommitmentCommand.Response>> Save(SaveCommitmentCommand.Request request) {
            request.Commitment.ProfileId = _httpContextAccessor.GetProfileIdFromClaims();
            return await _mediator.Send(request);
        }
        
        [HttpDelete("{commitmentId}")]
        public async Task Remove(RemoveCommitmentCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{CommitmentId}")]
        public async Task<ActionResult<GetCommitmentByIdQuery.Response>> GetById([FromRoute]GetCommitmentByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCommitmentsQuery.Response>> Get()
            => await _mediator.Send(new GetCommitmentsQuery.Request());

        [HttpGet("personal")]
        public async Task<ActionResult<GetPersonalCommitmentsQuery.Response>> GetPersonal()
            => await _mediator.Send(new GetPersonalCommitmentsQuery.Request() { ProfileId = _httpContextAccessor.GetProfileIdFromClaims() });

        [HttpGet("daily")]
        public async Task<ActionResult<GetDailyCommitmentsQuery.Response>> GetDaily() 
            => await _mediator.Send(new GetDailyCommitmentsQuery.Request() { ProfileId = _httpContextAccessor.GetProfileIdFromClaims() });
    }
}
