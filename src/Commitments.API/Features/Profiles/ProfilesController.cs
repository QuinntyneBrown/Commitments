using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.Profiles
{
    [Authorize]
    [ApiController]
    [Route("api/profiles")]
    public class ProfilesController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public ProfilesController(IHttpContextAccessor httpContextAccessor, IMediator mediator) {
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateProfileCommand.Response>> Create(CreateProfileCommand.Request request)
            => await _mediator.Send(request);

        [HttpGet("current")]
        public async Task<ActionResult<GetProfileByUsernameQuery.Response>> GetCurrent()
            => await _mediator.Send(new GetProfileByUsernameQuery.Request() { Username = _httpContextAccessor.HttpContext.User.Identity.Name });

    }
}
