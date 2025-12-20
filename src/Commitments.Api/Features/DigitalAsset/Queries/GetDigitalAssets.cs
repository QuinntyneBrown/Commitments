// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.DigitalAsset;

public class GetDigitalAssetsRequest : IRequest<GetDigitalAssetsResponse> { }

public class GetDigitalAssetsResponse
{
    public required IEnumerable<DigitalAssetDto> DigitalAssets { get; set; }
}

public class GetDigitalAssetsRequestHandler : IRequestHandler<GetDigitalAssetsRequest, GetDigitalAssetsResponse>
{
    private readonly ICommitmentsDbContext _context;

    public GetDigitalAssetsRequestHandler(ICommitmentsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetDigitalAssetsResponse> Handle(GetDigitalAssetsRequest request, CancellationToken cancellationToken)
        => new()
        {
            DigitalAssets = await _context.DigitalAssets
                .AsNoTracking()
                .Select(x => DigitalAssetDto.FromDigitalAsset(x))
                .ToListAsync(cancellationToken)
        };
}
