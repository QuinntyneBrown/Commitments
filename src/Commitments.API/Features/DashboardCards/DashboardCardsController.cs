using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.DashboardCards
{
    [Authorize]
    [ApiController]
    [Route("api/dashboardCards")]
    public class DashboardCardsController
    {
        private readonly IMediator _mediator;

        public DashboardCardsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveDashboardCardCommand.Response>> Save(SaveDashboardCardCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{DashboardCard.DashboardCardId}")]
        public async Task Remove([FromRoute]RemoveDashboardCardCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{DashboardCardId}")]
        public async Task<ActionResult<GetDashboardCardByIdQuery.Response>> GetById([FromRoute]GetDashboardCardByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetDashboardCardsQuery.Response>> Get()
            => await _mediator.Send(new GetDashboardCardsQuery.Request());
    }
}
