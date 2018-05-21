using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Commitments.API.Features.Activities
{
    [Authorize]
    [ApiController]
    [Route("api/activities")]
    public class ActivitiesController
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ActivitiesController(IHttpContextAccessor httpContextAccessor, IMediator mediator) {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<ActionResult<SaveActivityCommand.Response>> Save(SaveActivityCommand.Request request) {
            request.Activity.ProfileId = GetProfileId();
            return await _mediator.Send(request);
        }
        
        [HttpDelete("{Activity.ActivityId}")]
        public async Task Remove([FromRoute]RemoveActivityCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{ActivityId}")]
        public async Task<ActionResult<GetActivityByIdQuery.Response>> GetById([FromRoute]GetActivityByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetActivitiesQuery.Response>> Get()
            => await _mediator.Send(new GetActivitiesQuery.Request());
        public int GetProfileId()
        {
            var profileClaim = _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == "ProfileId");
            return Convert.ToInt16(profileClaim.Value);
        }
    }
}
