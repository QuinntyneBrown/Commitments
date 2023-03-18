// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.NoteAggregate.Queries;

public class GetNotesPageRequest: IRequest<GetNotesPageResponse>
{
    public int PageSize { get; set; }
    public int Index { get; set; }
    public int Length { get; set; }
}


public class GetNotesPageResponse
{
    public required int Length { get; set; }
    public required List<NoteDto> Entities  { get; set; }
}


public class CreateNoteRequestHandler: IRequestHandler<GetNotesPageRequest,GetNotesPageResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<CreateNoteRequestHandler> _logger;

    public CreateNoteRequestHandler(ILogger<CreateNoteRequestHandler> logger,INoteServiceDbContext context){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetNotesPageResponse> Handle(GetNotesPageRequest request,CancellationToken cancellationToken)
    {
        var query = from note in _context.Notes
            select note;

        var length = await _context.Notes
            .Include(x => x.Tags)
            .AsNoTracking().CountAsync();

        var notes = await query.Page(request.Index, request.PageSize).AsNoTracking()
            .Select(x => x.ToDto()).ToListAsync();

        return new ()
        {
            Length = length,
            Entities = notes
        };

    }

}



