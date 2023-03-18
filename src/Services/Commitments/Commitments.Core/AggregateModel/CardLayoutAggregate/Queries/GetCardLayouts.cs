// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.CardLayoutAggregate.Queries;

public class GetCardLayoutsRequest : IRequest<GetCardLayoutsResponse> { }

public class GetCardLayoutsResponse
{
    public IEnumerable<CardLayoutDto> CardLayouts { get; set; }
}

public class GetCardLayoutsQueryHandler : IRequestHandler<GetCardLayoutsRequest, GetCardLayoutsResponse>
{
    public ICommimentsDbContext _context { get; set; }


    public async Task<GetCardLayoutsResponse> Handle(GetCardLayoutsRequest request, CancellationToken cancellationToken)
        => new GetCardLayoutsResponse()
        {
            CardLayouts = await _context.CardLayouts.Select(x => CardLayoutDto.FromCardLayout(x)).ToListAsync()
        };
}

