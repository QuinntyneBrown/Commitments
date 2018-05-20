using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
        public async Task<ActionResult<SaveCommitmentCommand.Response>> Save(SaveCommitmentCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{commitmentId}")]
        public async Task Remove(RemoveCommitmentCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{CommitmentId}")]
        public async Task<ActionResult<GetCommitmentByIdQuery.Response>> GetById([FromRoute]GetCommitmentByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCommitmentsQuery.Response>> Get()
            => await _mediator.Send(new GetCommitmentsQuery.Request());

        [HttpGet("daily")]
        public async Task<ActionResult<GetDailyCommitmentsQuery.Response>> GetDaily() {
            var profileClaim = _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == "ProfileId");
            var profileId = Convert.ToInt16(profileClaim.Value);
            return await _mediator.Send(new GetDailyCommitmentsQuery.Request() { ProfileId = profileId });
        }
    }
}
