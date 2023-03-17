// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

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
    public async Task<ActionResult<GetNoteBySlugResponse>> GetBySlug([FromRoute]GetNoteBySlugRequest request)
        => await _mediator.Send(request);

    [HttpPost]
    public async Task<ActionResult<SaveNoteResponse>> Save(SaveNoteRequest request)
        => await _mediator.Send(request);

    [HttpDelete("{noteId}")]
    public async Task Remove([FromRoute]RemoveNoteRequest request)
        => await _mediator.Send(request);

    [HttpGet("{noteId}")]
    public async Task<ActionResult<GetNoteByIdResponse>> GetById([FromRoute]GetNoteByIdRequest request)
        => await _mediator.Send(request);

    [HttpGet]
    public async Task<ActionResult<GetNotesResponse>> Get()
        => await _mediator.Send(new GetNotesRequest());
}

