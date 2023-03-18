// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.TagAggregate.Queries;

public class GetTagsRequest : IRequest<GetTagsResponse> { }

public class GetTagsResponse
{
    public IEnumerable<TagDto> Tags { get; set; }
}

public class GetTagsQueryHandler : IRequestHandler<GetTagsRequest, GetTagsResponse>
{
    private readonly ICommimentsDbContext _context;

    public GetTagsQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetTagsResponse> Handle(GetTagsRequest request, CancellationToken cancellationToken)
        => new GetTagsResponse()
        {
            Tags = await _context.Tags.Select(x => TagDto.FromTag(x)).ToListAsync()
        };
}

