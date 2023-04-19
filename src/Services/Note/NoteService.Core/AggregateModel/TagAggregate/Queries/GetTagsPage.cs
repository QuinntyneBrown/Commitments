// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace NoteService.Core.AggregateModel.TagAggregate.Queries;

public class GetTagsPageRequest : IRequest<GetTagsPageResponse>
{
    public int PageSize { get; set; }
    public int Index { get; set; }
    public int Length { get; set; }
}


public class GetTagsPageResponse
{
    public required int Length { get; set; }
    public required List<TagDto> Entities { get; set; }
}


public class CreateTagRequestHandler : IRequestHandler<GetTagsPageRequest, GetTagsPageResponse>
{
    private readonly INoteServiceDbContext _context;

    private readonly ILogger<CreateTagRequestHandler> _logger;

    public CreateTagRequestHandler(ILogger<CreateTagRequestHandler> logger, INoteServiceDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetTagsPageResponse> Handle(GetTagsPageRequest request, CancellationToken cancellationToken)
    {
        var query = from tag in _context.Tags
                    select tag;

        var length = await _context.Tags.AsNoTracking().CountAsync();

        var tags = await query.Page(request.Index, request.PageSize).AsNoTracking()
            .Select(x => x.ToDto()).ToListAsync();

        return new()
        {
            Length = length,
            Entities = tags
        };

    }

}