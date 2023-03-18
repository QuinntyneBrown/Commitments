// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;


namespace Commitments.Core.AggregateModel.DigitalAssetAggregate.Queries;

public class GetDigitalAssetsByIdsRequest : IRequest<GetDigitalAssetsByIdsResponse>
{
    public Guid[] DigitalAssetIds { get; set; }
}

public class GetDigitalAssetsByIdsResponse
{
    public IEnumerable<DigitalAssetDto> DigitalAssets { get; set; }
}

public class GetDigitalAssetsByIdsQueryHandler : IRequestHandler<GetDigitalAssetsByIdsRequest, GetDigitalAssetsByIdsResponse>
{
    public ICommimentsDbContext _context { get; set; }
    public GetDigitalAssetsByIdsQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetDigitalAssetsByIdsResponse> Handle(GetDigitalAssetsByIdsRequest request, CancellationToken cancellationToken)
        => new GetDigitalAssetsByIdsResponse()
        {
            DigitalAssets = await _context.DigitalAssets
            .Where(x => request.DigitalAssetIds.Contains(x.DigitalAssetId))
            .Select(x => DigitalAssetDto.FromDigitalAsset(x)).ToListAsync()
        };
}

