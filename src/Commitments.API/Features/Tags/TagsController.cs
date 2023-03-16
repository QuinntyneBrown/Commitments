using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Tags;

[Authorize]
[ApiController]
[Route("api/tags")]
public class TagsController
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<GetTagBySlugQueryResponse>> GetBySlug([FromRoute]GetTagBySlugQueryRequest request)
        => await _mediator.Send(request);

    [HttpPost]
    public async Task<ActionResult<SaveTagCommandResponse>> Add(SaveTagCommandRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{tagId}")]
    public async Task Remove([FromRoute]RemoveTagCommandRequest request)
        => await _mediator.Send(request);            

    [HttpGet("{tagId}")]
    public async Task<ActionResult<GetTagByIdQueryResponse>> GetById([FromRoute]GetTagByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetTagsQueryResponse>> Get()
        => await _mediator.Send(new GetTagsQueryRequest());
}
