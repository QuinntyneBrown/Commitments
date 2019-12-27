using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.Api.Features.DashboardCards
{
    [Authorize]
    [ApiController]
    [Route("api/dashboardCards")]
    public class DashboardCardsController
    {
        private readonly IMediator _mediator;

        public DashboardCardsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("range")]
        public async Task<ActionResult<SaveDashboardCardRangeCommand.Response>> SaveRange(SaveDashboardCardRangeCommand.Request request)
            => await _mediator.Send(request);

        [HttpGet("range")]
        public async Task<ActionResult<GetDashboardCardByIdsQuery.Response>> GetByIds([FromQuery]GetDashboardCardByIdsQuery.Request request)
            => await _mediator.Send(request);

        [HttpPost]
        public async Task<ActionResult<SaveDashboardCardCommand.Response>> Save(SaveDashboardCardCommand.Request request)
            => await _mediator.Send(request);

        [HttpDelete("{dashboardCardId}")]
        public async Task Remove([FromRoute]RemoveDashboardCardCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{dashboardCardId}")]
        public async Task<ActionResult<GetDashboardCardByIdQuery.Response>> GetById([FromRoute]GetDashboardCardByIdQuery.Request request)
            => await _mediator.Send(request);

        
        [HttpGet]
        public async Task<ActionResult<GetDashboardCardsQuery.Response>> Get()
            => await _mediator.Send(new GetDashboardCardsQuery.Request());
    }
}
