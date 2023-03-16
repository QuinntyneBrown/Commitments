using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.DashboardCards;

[Authorize]
[ApiController]
[Route("api/dashboardCards")]
public class DashboardCardsController
{
    private readonly IMediator _mediator;

    public DashboardCardsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("range")]
    public async Task<ActionResult<SaveDashboardCardRangeCommandResponse>> SaveRange(SaveDashboardCardRangeCommandRequest request)
        => await _mediator.Send(request);

    [HttpGet("range")]
    public async Task<ActionResult<GetDashboardCardByIdsQueryResponse>> GetByIds([FromQuery]GetDashboardCardByIdsQueryRequest request)
        => await _mediator.Send(request);

    [HttpPost]
    public async Task<ActionResult<SaveDashboardCardCommandResponse>> Save(SaveDashboardCardCommandRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{dashboardCardId}")]
    public async Task Remove([FromRoute]RemoveDashboardCardCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{dashboardCardId}")]
    public async Task<ActionResult<GetDashboardCardByIdQueryResponse>> GetById([FromRoute]GetDashboardCardByIdQueryRequest request)
        => await _mediator.Send(request);


    [HttpGet]
    public async Task<ActionResult<GetDashboardCardsQueryResponse>> Get()
        => await _mediator.Send(new GetDashboardCardsQueryRequest());
}
