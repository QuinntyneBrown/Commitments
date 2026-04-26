using Commitments.Shared;
using DigitalAssets.Data;
using MediatR;

namespace DigitalAssets.Features.DigitalAsset;

public class UpdateDigitalAssetRequest : IRequest<UpdateDigitalAssetResponse>
{
    public DigitalAssetDto DigitalAsset { get; set; }
}

public class UpdateDigitalAssetResponse : ResponseBase
{
    public DigitalAssetDto DigitalAsset { get; set; }
}

public class UpdateDigitalAssetRequestHandler : IRequestHandler<UpdateDigitalAssetRequest, UpdateDigitalAssetResponse>
{
    private readonly IDigitalAssetsDbContext _context;

    public UpdateDigitalAssetRequestHandler(IDigitalAssetsDbContext context) => _context = context;

    public async Task<UpdateDigitalAssetResponse> Handle(UpdateDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var digitalAsset = await _context.DigitalAssets.FindAsync(request.DigitalAsset.DigitalAssetId);
        if (digitalAsset == null) return new UpdateDigitalAssetResponse();

        digitalAsset.Bytes = request.DigitalAsset.Bytes;
        digitalAsset.ContentType = request.DigitalAsset.ContentType;
        digitalAsset.Name = request.DigitalAsset.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateDigitalAssetResponse { DigitalAsset = digitalAsset.ToDto() };
    }
}
