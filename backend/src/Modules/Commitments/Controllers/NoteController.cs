using Commitments.Features.Note;
using Commitments.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Commitments.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/notes")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class NoteController
{
    private readonly ISender _sender;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NoteController(ISender sender, IHttpContextAccessor httpContextAccessor)
    {
        _sender = sender;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("currentuser")]
    [ProducesResponseType(typeof(GetNotesByCurrentUserResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<GetNotesByCurrentUserResponse>> GetByCurrentUser()
        => await _sender.Send(new GetNotesByCurrentUserRequest { ProfileId = _httpContextAccessor.GetProfileId() });

    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(GetNoteBySlugResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<GetNoteBySlugResponse>> GetBySlug([FromRoute] string slug)
    {
        var response = await _sender.Send(new GetNoteBySlugRequest { ProfileId = _httpContextAccessor.GetProfileId(), Slug = slug });
        if (response.Note is null) return new NotFoundResult();
        return response;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SaveNoteResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<SaveNoteResponse>> Save([FromBody] SaveNoteRequest request)
    {
        request.Note.ProfileId = _httpContextAccessor.GetProfileId();
        return await _sender.Send(request);
    }

    [HttpDelete("{noteId:guid}")]
    public async Task Remove([FromRoute] Guid noteId)
        => await _sender.Send(new RemoveNoteRequest { NoteId = noteId });
}
