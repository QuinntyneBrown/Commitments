// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core.AggregateModel.NoteAggregate.Commands;
using NoteService.Core.AggregateModel.NoteAggregate.Queries;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Swashbuckle.AspNetCore.Annotations;

namespace NoteService.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class NoteController
{
    private readonly IMediator _mediator;

    private readonly ILogger<NoteController> _logger;

    public NoteController(IMediator mediator, ILogger<NoteController> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [SwaggerOperation(
        Summary = "Update Note",
        Description = @"Update Note"
    )]
    [HttpPut(Name = "updateNote")]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(UpdateNoteResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<UpdateNoteResponse>> Update([FromBody] UpdateNoteRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Create Note",
        Description = @"Create Note"
    )]
    [HttpPost(Name = "createNote")]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(CreateNoteResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<CreateNoteResponse>> Create([FromBody] CreateNoteRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Notes",
        Description = @"Get Notes"
    )]
    [HttpGet(Name = "getNotes")]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetNotesResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<GetNotesResponse>> Get(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetNotesRequest(), cancellationToken);
    }

    [SwaggerOperation(
        Summary = "Get Note by id",
        Description = @"Get Note by id"
    )]
    [HttpGet("{noteId:guid}", Name = "getNoteById")]
    [ProducesResponseType(typeof(string), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetNoteByIdResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<GetNoteByIdResponse>> GetById([FromRoute] Guid noteId, CancellationToken cancellationToken)
    {
        var request = new GetNoteByIdRequest() { NoteId = noteId };

        var response = await _mediator.Send(request, cancellationToken);

        if (response.Note == null)
        {
            return new NotFoundObjectResult(request.NoteId);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Get Note by slug",
        Description = @"Get Note by slug"
    )]
    [HttpGet("slug/{slug}", Name = "getNoteBySlug")]
    [ProducesResponseType(typeof(string), (int) HttpStatusCode.NotFound)]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(GetNoteByIdResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<GetNoteBySlugResponse>> GetBySlug([FromRoute] string slug, CancellationToken cancellationToken)
    {
        var request = new GetNoteBySlugRequest() { Slug = slug };

        var response = await _mediator.Send(request, cancellationToken);

        if (response.Note == null)
        {
            return new NotFoundObjectResult(request.Slug);
        }

        return response;
    }

    [SwaggerOperation(
        Summary = "Delete Note",
        Description = @"Delete Note"
    )]
    [HttpDelete("{noteId:guid}", Name = "deleteNote")]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(DeleteNoteResponse), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<DeleteNoteResponse>> Delete([FromRoute] Guid noteId, CancellationToken cancellationToken)
    {
        var request = new DeleteNoteRequest() { NoteId = noteId };

        return await _mediator.Send(request, cancellationToken);
    }

}