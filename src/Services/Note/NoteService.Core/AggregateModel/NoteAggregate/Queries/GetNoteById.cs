// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.NoteAggregate.Queries;

public class GetNoteByIdRequest : IRequest<GetNoteByIdResponse>
{
    public Guid NoteId { get; set; }
}


public class GetNoteByIdResponse
{
    public required NoteDto Note { get; set; }
}


public class GetNoteByIdRequestHandler : IRequestHandler<GetNoteByIdRequest, GetNoteByIdResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<GetNoteByIdRequestHandler> _logger;

    public GetNoteByIdRequestHandler(ILogger<GetNoteByIdRequestHandler> logger, INoteServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetNoteByIdResponse> Handle(GetNoteByIdRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            Note = (await _context.Notes
            .Include(x => x.Tags)
            .AsNoTracking().SingleOrDefaultAsync(x => x.NoteId == request.NoteId)).ToDto()
        };

    }

}



