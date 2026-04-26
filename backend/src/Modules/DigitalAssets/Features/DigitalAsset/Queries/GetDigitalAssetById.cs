using Commitments.Shared;
using DigitalAssets.Data;
using MediatR;

namespace DigitalAssets.Features.DigitalAsset;

public class GetDigitalAssetByIdRequest : IRequest<GetDigitalAssetByIdResponse>
{
    public Guid DigitalAssetId { get; set; }
}

public class GetDigitalAssetByIdResponse : ResponseBase
{
    public DigitalAssetDto? DigitalAsset { get; set; }
}

public class GetDigitalAssetByIdRequestHandler : IRequestHandler<GetDigitalAssetByIdRequest, GetDigitalAssetByIdResponse>
{
    private readonly IDigitalAssetsDbContext _context;

    public GetDigitalAssetByIdRequestHandler(IDigitalAssetsDbContext context) => _context = context;

    public async Task<GetDigitalAssetByIdResponse> Handle(GetDigitalAssetByIdRequest request, CancellationToken cancellationToken)
    {
        var digitalAsset = await _context.DigitalAssets.FindAsync(request.DigitalAssetId);
        return new GetDigitalAssetByIdResponse { DigitalAsset = digitalAsset?.ToDto() };
    }
}
