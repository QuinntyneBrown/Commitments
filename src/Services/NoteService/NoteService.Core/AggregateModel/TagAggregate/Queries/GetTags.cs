// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.TagAggregate.Queries;

public class GetTagsRequest: IRequest<GetTagsResponse> { }

public class GetTagsResponse
{
    public required List<TagDto> Tags { get; set; }
}


public class GetTagsRequestHandler: IRequestHandler<GetTagsRequest,GetTagsResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<GetTagsRequestHandler> _logger;

    public GetTagsRequestHandler(ILogger<GetTagsRequestHandler> logger,INoteServiceDbContext context){
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetTagsResponse> Handle(GetTagsRequest request,CancellationToken cancellationToken)
    {
        return new () {
            Tags = await _context.Tags.AsNoTracking().ToDtosAsync(cancellationToken)
        };

    }

}



