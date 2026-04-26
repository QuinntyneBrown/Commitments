using Commitments.Shared;
using DigitalAssets.Data;
using MediatR;

namespace DigitalAssets.Features.DigitalAsset;

public class DeleteDigitalAssetRequest : IRequest<DeleteDigitalAssetResponse>
{
    public Guid DigitalAssetId { get; set; }
}

public class DeleteDigitalAssetResponse : ResponseBase { }

public class DeleteDigitalAssetRequestHandler : IRequestHandler<DeleteDigitalAssetRequest, DeleteDigitalAssetResponse>
{
    private readonly IDigitalAssetsDbContext _context;

    public DeleteDigitalAssetRequestHandler(IDigitalAssetsDbContext context) => _context = context;

    public async Task<DeleteDigitalAssetResponse> Handle(DeleteDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var digitalAsset = await _context.DigitalAssets.FindAsync(request.DigitalAssetId);
        if (digitalAsset != null)
        {
            _context.DigitalAssets.Remove(digitalAsset);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return new DeleteDigitalAssetResponse();
    }
}
