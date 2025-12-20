// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commitments.Api.Features.DigitalAsset;

public class GetDigitalAssetByIdRequest : IRequest<GetDigitalAssetByIdResponse>
{
    public Guid DigitalAssetId { get; set; }
}

public class GetDigitalAssetByIdResponse
{
    public DigitalAssetDto? DigitalAsset { get; set; }
}

public class GetDigitalAssetByIdRequestHandler : IRequestHandler<GetDigitalAssetByIdRequest, GetDigitalAssetByIdResponse>
{
    private readonly ICommitmentsDbContext _context;

    public GetDigitalAssetByIdRequestHandler(ICommitmentsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetDigitalAssetByIdResponse> Handle(GetDigitalAssetByIdRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.DigitalAssets
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DigitalAssetId == request.DigitalAssetId, cancellationToken);

        return new GetDigitalAssetByIdResponse
        {
            DigitalAsset = entity is null ? null : DigitalAssetDto.FromDigitalAsset(entity)
        };
    }
}
