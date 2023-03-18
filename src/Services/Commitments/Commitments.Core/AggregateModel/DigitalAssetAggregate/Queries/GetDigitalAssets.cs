// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Commitments.Core.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Commitments.Core.AggregateModel.DigitalAssetAggregate.Queries;

public class GetDigitalAssetsRequest : IRequest<GetDigitalAssetsResponse> { }

public class GetDigitalAssetsResponse
{
    public IEnumerable<DigitalAssetDto> DigitalAssets { get; set; }
}

public class GetDigitalAssetsQueryHandler : IRequestHandler<GetDigitalAssetsRequest, GetDigitalAssetsResponse>
{
    public ICommimentsDbContext _context { get; set; }

    public GetDigitalAssetsQueryHandler(ICommimentsDbContext context) => _context = context;

    public async Task<GetDigitalAssetsResponse> Handle(GetDigitalAssetsRequest request, CancellationToken cancellationToken)
        => new GetDigitalAssetsResponse()
        {
            DigitalAssets = await _context.DigitalAssets.Select(x => DigitalAssetDto.FromDigitalAsset(x)).ToListAsync()
        };
}

