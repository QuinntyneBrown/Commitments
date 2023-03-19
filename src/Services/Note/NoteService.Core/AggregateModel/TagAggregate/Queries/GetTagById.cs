// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.TagAggregate.Queries;

public class GetTagByIdRequest : IRequest<GetTagByIdResponse>
{
    public Guid TagId { get; set; }
}


public class GetTagByIdResponse
{
    public required TagDto Tag { get; set; }
}


public class GetTagByIdRequestHandler : IRequestHandler<GetTagByIdRequest, GetTagByIdResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<GetTagByIdRequestHandler> _logger;

    public GetTagByIdRequestHandler(ILogger<GetTagByIdRequestHandler> logger, INoteServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetTagByIdResponse> Handle(GetTagByIdRequest request, CancellationToken cancellationToken)
    {
        return new()
        {
            Tag = (await _context.Tags.AsNoTracking().SingleOrDefaultAsync(x => x.TagId == request.TagId)).ToDto()
        };

    }

}



