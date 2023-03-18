// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.TagAggregate.Queries;

public class GetTagBySlugRequest : IRequest<GetTagBySlugResponse>
{
    public string Slug { get; set; }
}

public class GetTagBySlugResponse
{
    public TagDto Tag { get; set; }
}

public class GetTagBySlugQueryHandler : IRequestHandler<GetTagBySlugRequest, GetTagBySlugResponse>
{
    private readonly ICommimentsDbContext _context;

    public GetTagBySlugQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetTagBySlugResponse> Handle(GetTagBySlugRequest request, CancellationToken cancellationToken)
        => new GetTagBySlugResponse()
        {
            Tag = TagDto.FromTag(await _context.Tags
                .Include(x => x.NoteTags)
                .Include("NoteTags.Note")
                .Where(x => x.Slug == request.Slug)
                .SingleAsync())
        };
}

