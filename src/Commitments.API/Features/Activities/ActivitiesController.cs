using Commitments.Core.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Activities;

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
    public async Task<ActionResult<SaveActivityCommandResponse>> Save(SaveActivityCommandRequest request) {
        request.Activity.ProfileId = _httpContextAccessor.GetProfileIdFromClaims();
        return await _mediator.Send(request);
    }

    [HttpDelete("{activityId}")]
    public async Task Remove([FromRoute]RemoveActivityCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{ActivityId}")]
    public async Task<ActionResult<GetActivityByIdQueryResponse>> GetById([FromRoute]GetActivityByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetActivitiesQueryResponse>> Get()
        => await _mediator.Send(new GetActivitiesQueryRequest());

}
