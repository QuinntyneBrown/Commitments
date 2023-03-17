using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Profiles;

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
    public async Task<ActionResult<SaveAvatarCommandResponse>> SaveAvatar(SaveAvatarCommandRequest request)
        => await _mediator.Send(request);

    [HttpPost("create")]
    public async Task<ActionResult<CreateProfileCommandResponse>> Create(CreateProfileCommandRequest request) {

        var response = await _mediator.Send(request);
        var dashboard = await _mediator.Send(new Dashboards.SaveDashboardCommandRequest()
        {
            Dashboard = new Dashboards.DashboardDto()
            {
                ProfileId = response.ProfileId,
                Name = "Default"
            }
        });
        return response;
    }

    [HttpGet("current")]
    public async Task<ActionResult<GetProfileByUsernameQueryResponse>> GetCurrent()
        => await _mediator.Send(new GetProfileByUsernameQueryRequest() { Username = _httpContextAccessor.HttpContext.User.Identity.Name });

    [HttpGet]
    public async Task<ActionResult<GetProfilesQueryResponse>> Get()
        => await _mediator.Send(new GetProfilesQueryRequest());

    [HttpDelete("{profileId}")]
    public async Task<ActionResult<RemoveProfileCommandResponse>> Remove([FromRoute]RemoveProfileCommandRequest request)
        => await _mediator.Send(request);

}
