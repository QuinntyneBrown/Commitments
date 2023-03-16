using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Dashboards;

[Authorize]
[ApiController]
[Route("api/dashboards")]
public class DashboardsController
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardsController(IHttpContextAccessor httpContextAccessor, IMediator mediator)
    {
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<SaveDashboardCommandResponse>> Save(SaveDashboardCommandRequest request)
        => await _mediator.Send(request);

    [HttpGet("currentProfile")]
    public async Task<ActionResult<GetDashboardByProfileIdQueryResponse>> Get()
    {
        var profileClaim = _httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == "ProfileId");
        var profileId = Convert.ToInt16(profileClaim.Value);
        return await _mediator.Send(new GetDashboardByProfileIdQueryRequest()
        {
            ProfileId = profileId
        });
    }

    [HttpDelete("{dashboardId}")]
    public async Task Remove([FromRoute]RemoveDashboardCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{dashboardId}")]
    public async Task<ActionResult<GetDashboardByIdQueryResponse>> GetById([FromRoute]GetDashboardByIdQueryRequest request)
        => await _mediator.Send(request);


}
