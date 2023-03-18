// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core.AggregateModel.TagAggregate;

namespace NoteService.Core.AggregateModel.NoteAggregate.Commands;

public class UpdateNoteRequestValidator: AbstractValidator<UpdateNoteRequest>
{
    public UpdateNoteRequestValidator(){

        RuleFor(x => x.NoteId).NotEqual(default(Guid));
        RuleFor(x => x.Title).NotNull().NotEmpty();
        RuleFor(x => x.Body).NotNull();
        RuleFor(x => x.Tags).NotNull();

    }

}


public class UpdateNoteRequest: IRequest<UpdateNoteResponse>
{
    public Guid NoteId { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public List<TagDto> Tags { get; set; }
}


public class UpdateNoteResponse
{
    public required NoteDto Note { get; set; }
}


public class UpdateNoteRequestHandler: IRequestHandler<UpdateNoteRequest,UpdateNoteResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<UpdateNoteRequestHandler> _logger;

    public UpdateNoteRequestHandler(ILogger<UpdateNoteRequestHandler> logger,INoteServiceDbContext context){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<UpdateNoteResponse> Handle(UpdateNoteRequest request,CancellationToken cancellationToken)
    {
        var note = await _context.Notes
            .Include(x => x.Tags)
            .SingleAsync(x => x.NoteId == request.NoteId);

        note.NoteId = request.NoteId;
        note.Title = request.Title;
        note.Slug = request.Title.GenerateSlug();
        note.Body = request.Body;

        await _context.SaveChangesAsync(cancellationToken);

        return new ()
        {
            Note = note.ToDto()
        };

    }

}



