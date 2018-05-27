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

        [HttpPost("avatar")]
        public async Task<ActionResult<SaveAvatarCommand.Response>> SaveAvatar(SaveAvatarCommand.Request request)
            => await _mediator.Send(request);

        [HttpPost("create")]
        public async Task<ActionResult<CreateProfileCommand.Response>> Create(CreateProfileCommand.Request request) {

            var response = await _mediator.Send(request);
            var dashboard = await _mediator.Send(new Dashboards.SaveDashboardCommand.Request()
            {
                Dashboard = new Dashboards.DashboardApiModel()
                {
                    ProfileId = response.ProfileId,
                    Name = "Default"
                }
            });
            return response;
        }

        [HttpGet("current")]
        public async Task<ActionResult<GetProfileByUsernameQuery.Response>> GetCurrent()
            => await _mediator.Send(new GetProfileByUsernameQuery.Request() { Username = _httpContextAccessor.HttpContext.User.Identity.Name });

        [HttpGet]
        public async Task<ActionResult<GetProfilesQuery.Response>> Get()
            => await _mediator.Send(new GetProfilesQuery.Request());

        [HttpDelete("{profileId}")]
        public async Task<ActionResult<RemoveProfileCommand.Response>> Remove([FromRoute]RemoveProfileCommand.Request request)
            => await _mediator.Send(request);

    }
}
