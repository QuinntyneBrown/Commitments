using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.API.Features.Cards
{
    [Authorize]
    [ApiController]
    [Route("api/cards")]
    public class CardsController
    {
        private readonly IMediator _mediator;

        public CardsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<SaveCardCommand.Response>> Save(SaveCardCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{cardId}")]
        public async Task Remove([FromRoute]RemoveCardCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{cardId}")]
        public async Task<ActionResult<GetCardByIdQuery.Response>> GetById([FromRoute]GetCardByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetCardsQuery.Response>> Get()
            => await _mediator.Send(new GetCardsQuery.Request());
    }
}
