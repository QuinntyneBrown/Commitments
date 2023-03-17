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

        if (typeof(TRequest) == typeof(SaveNoteCommandRequest))
            return await (HandleSaveNoteCommand(request as SaveNoteCommandRequest, cancellationToken, response as SaveNoteCommandResponse) as Task<TResponse>);

        if (typeof(TRequest) == typeof(RemoveNoteCommandRequest))
            return await (HandleRemoveNoteCommand(request as RemoveNoteCommandRequest, cancellationToken, response as RemoveNoteCommandResponse) as Task<TResponse>);

        if (typeof(TRequest) == typeof(SaveTagCommandRequest))
            return await (HandleSaveTagCommand(request as SaveTagCommandRequest, cancellationToken, response as SaveTagCommandResponse) as Task<TResponse>);

        if (typeof(TRequest) == typeof(RemoveTagCommandRequest))
            return await (HandleRemoveTagCommand(request as RemoveTagCommandRequest, cancellationToken, response as RemoveTagCommandResponse) as Task<TResponse>);

        return response;
    }

    public async Task<SaveNoteCommandResponse> HandleSaveNoteCommand(SaveNoteCommandRequest request, CancellationToken cancellationToken, SaveNoteCommandResponse response)
    {
        var note = await _context.Notes.FindAsync(response.NoteId);

        await _hubContext.Clients.All.SendAsync("message", new
        {
            Type = "[Note] Saved",
            Payload = new { note = NoteDto.FromNote(note) }
        });

        return response;
    }

    public async Task<RemoveNoteCommandResponse> HandleRemoveNoteCommand(RemoveNoteCommandRequest request, CancellationToken cancellationToken, RemoveNoteCommandResponse response)
    {
        await _hubContext.Clients.All.SendAsync("message", new
        {
            Type = "[Note] Removed",
            Payload = new { noteId = request.NoteId }
        });

        return response;
    }

    public async Task<SaveTagCommandResponse> HandleSaveTagCommand(SaveTagCommandRequest request, CancellationToken cancellationToken, SaveTagCommandResponse response)
    {
        var tag = await _context.Tags.FindAsync(response.TagId);

        await _hubContext.Clients.All.SendAsync("message", new
        {
            Type = "[Tag] Saved",
            Payload = new { tag = TagDto.FromTag(tag) }
        });

        return response;
    }

    public async Task<RemoveTagCommandResponse> HandleRemoveTagCommand(RemoveTagCommandRequest request, CancellationToken cancellationToken, RemoveTagCommandResponse response)
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
