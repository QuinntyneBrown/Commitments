using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.CardLayouts
{
    [Authorize]
    [ApiController]
    [Route("api/cardLayouts")]
    public class CardLayoutsController
    {
        private readonly IMediator _mediator;

        public CardLayoutsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveCardLayoutCommand.Response>> Save(SaveCardLayoutCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{cardLayoutId}")]
        public async Task Remove([FromRoute]RemoveCardLayoutCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{cardLayoutId}")]
        public async Task<ActionResult<GetCardLayoutByIdQuery.Response>> GetById([FromRoute]GetCardLayoutByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCardLayoutsQuery.Response>> Get()
            => await _mediator.Send(new GetCardLayoutsQuery.Request());
    }
}
