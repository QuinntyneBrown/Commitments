// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.NoteAggregate.Queries;

public class GetNotesRequest: IRequest<GetNotesResponse> { }

public class GetNotesResponse
{
    public required List<NoteDto> Notes { get; set; }
}


public class GetNotesRequestHandler: IRequestHandler<GetNotesRequest,GetNotesResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<GetNotesRequestHandler> _logger;

    public GetNotesRequestHandler(ILogger<GetNotesRequestHandler> logger,INoteServiceDbContext context){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetNotesResponse> Handle(GetNotesRequest request,CancellationToken cancellationToken)
    {
        return new () {
            Notes = await _context.Notes
            .Include(x => x.Tags)
            .AsNoTracking().ToDtosAsync(cancellationToken)
        };

    }

}



