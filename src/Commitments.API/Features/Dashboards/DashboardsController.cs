using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.Dashboards
{
    [Authorize]
    [ApiController]
    [Route("api/dashboards")]
    public class DashboardsController
    {
        private readonly IMediator _mediator;

        public DashboardsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveDashboardCommand.Response>> Save(SaveDashboardCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{Dashboard.DashboardId}")]
        public async Task Remove([FromRoute]RemoveDashboardCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{DashboardId}")]
        public async Task<ActionResult<GetDashboardByIdQuery.Response>> GetById([FromRoute]GetDashboardByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetDashboardsQuery.Response>> Get()
            => await _mediator.Send(new GetDashboardsQuery.Request());
    }
}
