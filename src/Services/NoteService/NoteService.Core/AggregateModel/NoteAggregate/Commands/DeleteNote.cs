// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.NoteAggregate.Commands;

public class DeleteNoteRequestValidator: AbstractValidator<DeleteNoteRequest>
{
    public DeleteNoteRequestValidator(){

        RuleFor(x => x.NoteId).NotEqual(default(Guid));

    }

}


public class DeleteNoteRequest: IRequest<DeleteNoteResponse>
{
    public Guid NoteId { get; set; }
}


public class DeleteNoteResponse
{
    public required NoteDto Note { get; set; }
}


public class DeleteNoteRequestHandler: IRequestHandler<DeleteNoteRequest,DeleteNoteResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<DeleteNoteRequestHandler> _logger;

    public DeleteNoteRequestHandler(ILogger<DeleteNoteRequestHandler> logger,INoteServiceDbContext context){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DeleteNoteResponse> Handle(DeleteNoteRequest request,CancellationToken cancellationToken)
    {
        var note = await _context.Notes.FindAsync(request.NoteId);

        _context.Notes.Remove(note);

        await _context.SaveChangesAsync(cancellationToken);

        return new ()
        {
            Note = note.ToDto()
        };
    }

}



