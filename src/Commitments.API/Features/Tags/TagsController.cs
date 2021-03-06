using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Commitments.Api.Features.Tags
{
    [Authorize]
    [ApiController]
    [Route("api/tags")]
    public class TagsController
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<GetTagBySlugQuery.Response>> GetBySlug([FromRoute]GetTagBySlugQuery.Request request)
            => await _mediator.Send(request);

        [HttpPost]
        public async Task<ActionResult<SaveTagCommand.Response>> Add(SaveTagCommand.Request request)
            => await _mediator.Send(request);
        
        [HttpDelete("{tagId}")]
        public async Task Remove([FromRoute]RemoveTagCommand.Request request)
            => await _mediator.Send(request);            

        [HttpGet("{tagId}")]
        public async Task<ActionResult<GetTagByIdQuery.Response>> GetById([FromRoute]GetTagByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetTagsQuery.Response>> Get()
            => await _mediator.Send(new GetTagsQuery.Request());
    }
}