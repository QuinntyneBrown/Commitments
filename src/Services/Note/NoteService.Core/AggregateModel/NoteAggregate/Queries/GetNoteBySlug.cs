// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.NoteAggregate.Queries;

public class GetNoteBySlugRequest: IRequest<GetNoteBySlugResponse>
{
    public string Slug { get; set; }
}


public class GetNoteBySlugResponse
{
    public required NoteDto Note { get; set; }
}


public class GetNoteBySlugRequestHandler: IRequestHandler<GetNoteBySlugRequest,GetNoteBySlugResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<GetNoteBySlugRequestHandler> _logger;

    public GetNoteBySlugRequestHandler(
        ILogger<GetNoteBySlugRequestHandler> logger,
        INoteServiceDbContext context){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetNoteBySlugResponse> Handle(GetNoteBySlugRequest request,CancellationToken cancellationToken)
    {
        return new () {
            Note = (await _context.Notes.AsNoTracking().SingleOrDefaultAsync(x => x.Slug == request.Slug)).ToDto()
        };

    }

}



