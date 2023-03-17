using Commitments.Api.Features.Notes;
using Commitments.Api.Features.Tags;
using Commitments.Api.Hubs;
using Commitments.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;



namespace Commitments.Api.Behaviors;

public class EntityChangedBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>

{
    private readonly IHubContext<CommitmentsHub> _hubContext;
    private readonly ICommimentsDbContext _context;

    public EntityChangedBehavior(IHubContext<CommitmentsHub> hubContext, ICommimentsDbContext context)
    {
        _hubContext = hubContext;
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var response = await next();

        if (typeof(TRequest) == typeof(SaveNoteRequest))
            return await (HandleSaveNoteCommand(request as SaveNoteRequest, cancellationToken, response as SaveNoteResponse) as Task<TResponse>);

        if (typeof(TRequest) == typeof(RemoveNoteRequest))
            return await (HandleRemoveNoteCommand(request as RemoveNoteRequest, cancellationToken, response as RemoveNoteResponse) as Task<TResponse>);

        if (typeof(TRequest) == typeof(SaveTagRequest))
            return await (HandleSaveTagCommand(request as SaveTagRequest, cancellationToken, response as SaveTagResponse) as Task<TResponse>);

        if (typeof(TRequest) == typeof(RemoveTagRequest))
            return await (HandleRemoveTagCommand(request as RemoveTagRequest, cancellationToken, response as RemoveTagResponse) as Task<TResponse>);

        return response;
    }

    public async Task<SaveNoteResponse> HandleSaveNoteCommand(SaveNoteRequest request, CancellationToken cancellationToken, SaveNoteResponse response)
    {
        var note = await _context.Notes.FindAsync(response.NoteId);

        await _hubContext.Clients.All.SendAsync("message", new
        {
            Type = "[Note] Saved",
            Payload = new { note = NoteDto.FromNote(note) }
        });

        return response;
    }

    public async Task<RemoveNoteResponse> HandleRemoveNoteCommand(RemoveNoteRequest request, CancellationToken cancellationToken, RemoveNoteResponse response)
    {
        await _hubContext.Clients.All.SendAsync("message", new
        {
            Type = "[Note] Removed",
            Payload = new { noteId = request.NoteId }
        });

        return response;
    }

    public async Task<SaveTagResponse> HandleSaveTagCommand(SaveTagRequest request, CancellationToken cancellationToken, SaveTagResponse response)
    {
        var tag = await _context.Tags.FindAsync(response.TagId);

        await _hubContext.Clients.All.SendAsync("message", new
        {
            Type = "[Tag] Saved",
            Payload = new { tag = TagDto.FromTag(tag) }
        });

        return response;
    }

    public async Task<RemoveTagResponse> HandleRemoveTagCommand(RemoveTagRequest request, CancellationToken cancellationToken, RemoveTagResponse response)
    {
        await _hubContext.Clients.All.SendAsync("message", new
        {
            Type = "[Tag] Removed",
            Payload = new { tagId = request.TagId }
        });

        return response;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
} 
