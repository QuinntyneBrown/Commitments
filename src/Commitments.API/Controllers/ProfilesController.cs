// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

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
    public async Task<ActionResult<SaveAvatarResponse>> SaveAvatar(SaveAvatarRequest request)
        => await _mediator.Send(request);

    [HttpPost("create")]
    public async Task<ActionResult<CreateProfileResponse>> Create(CreateProfileRequest request) {

        var response = await _mediator.Send(request);
        var dashboard = await _mediator.Send(new Dashboards.SaveDashboardRequest()
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
    public async Task<ActionResult<GetProfileByUsernameResponse>> GetCurrent()
        => await _mediator.Send(new GetProfileByUsernameRequest() { Username = _httpContextAccessor.HttpContext.User.Identity.Name });

    [HttpGet]
    public async Task<ActionResult<GetProfilesResponse>> Get()
        => await _mediator.Send(new GetProfilesRequest());

    [HttpDelete("{profileId}")]
    public async Task<ActionResult<RemoveProfileResponse>> Remove([FromRoute]RemoveProfileRequest request)
        => await _mediator.Send(request);

}

