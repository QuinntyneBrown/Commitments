using Commitments.Shared;
using DigitalAssets.Core;
using MediatR;

namespace DigitalAssets.Api.Features.DigitalAsset;

public class GetDigitalAssetsRequest : IRequest<GetDigitalAssetsResponse> { }

public class GetDigitalAssetsResponse : ResponseBase
{
    public required List<DigitalAssetDto> DigitalAssets { get; set; }
}

public class GetDigitalAssetsRequestHandler : IRequestHandler<GetDigitalAssetsRequest, GetDigitalAssetsResponse>
{
    private readonly IDigitalAssetsDbContext _context;

    public GetDigitalAssetsRequestHandler(IDigitalAssetsDbContext context) => _context = context;

    public async Task<GetDigitalAssetsResponse> Handle(GetDigitalAssetsRequest request, CancellationToken cancellationToken)
    {
        return new GetDigitalAssetsResponse
        {
            DigitalAssets = await _context.DigitalAssets.ToDtosAsync(cancellationToken)
        };
    }
}
