using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Commitments.Api.Features.Notes;

[Authorize]
[ApiController]
[Route("api/notes")]
public class NotesController
{
    private readonly IMediator _mediator;

    public NotesController(IMediator mediator) => _mediator = mediator;

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<GetNoteBySlugQueryResponse>> GetBySlug([FromRoute]GetNoteBySlugQueryRequest request)
        => await _mediator.Send(request);

    [HttpPost]
    public async Task<ActionResult<SaveNoteCommandResponse>> Save(SaveNoteCommandRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{noteId}")]
    public async Task Remove([FromRoute]RemoveNoteCommandRequest request)
        => await _mediator.Send(request);

    [HttpGet("{noteId}")]
    public async Task<ActionResult<GetNoteByIdQueryResponse>> GetById([FromRoute]GetNoteByIdQueryRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetNotesQueryResponse>> Get()
        => await _mediator.Send(new GetNotesQueryRequest());
}
