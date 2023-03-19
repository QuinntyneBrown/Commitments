// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using NoteService.Core.AggregateModel.TagAggregate;

namespace NoteService.Core.AggregateModel.NoteAggregate.Commands;

public class CreateNoteRequestValidator : AbstractValidator<CreateNoteRequest>
{
    public CreateNoteRequestValidator()
    {

        RuleFor(x => x.Title).NotNull().NotEmpty();
        RuleFor(x => x.Body).NotNull();
    }
}


public class CreateNoteRequest : IRequest<CreateNoteResponse>
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public List<TagDto> Tags { get; set; }
}


public class CreateNoteResponse
{
    public required NoteDto Note { get; set; }
}


public class CreateNoteRequestHandler : IRequestHandler<CreateNoteRequest, CreateNoteResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<CreateNoteRequestHandler> _logger;

    public CreateNoteRequestHandler(ILogger<CreateNoteRequestHandler> logger, INoteServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CreateNoteResponse> Handle(CreateNoteRequest request, CancellationToken cancellationToken)
    {
        var note = new Note();

        _context.Notes.Add(note);

        note.Title = request.Title;
        note.Slug = request.Title.GenerateSlug();
        note.Body = request.Body;

        await _context.SaveChangesAsync(cancellationToken);

        return new()
        {
            Note = note.ToDto()
        };

    }

}



