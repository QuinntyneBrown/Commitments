using Commitments.Shared;
using DigitalAssets.Data;
using MediatR;

namespace DigitalAssets.Features.DigitalAsset;

public class CreateDigitalAssetRequest : IRequest<CreateDigitalAssetResponse>
{
    public DigitalAssetDto DigitalAsset { get; set; }
}

public class CreateDigitalAssetResponse : ResponseBase
{
    public DigitalAssetDto DigitalAsset { get; set; }
}

public class CreateDigitalAssetRequestHandler : IRequestHandler<CreateDigitalAssetRequest, CreateDigitalAssetResponse>
{
    private readonly IDigitalAssetsDbContext _context;

    public CreateDigitalAssetRequestHandler(IDigitalAssetsDbContext context) => _context = context;

    public async Task<CreateDigitalAssetResponse> Handle(CreateDigitalAssetRequest request, CancellationToken cancellationToken)
    {
        var digitalAsset = new DigitalAssets.Domain.DigitalAssetAggregate.DigitalAsset
        {
            Bytes = request.DigitalAsset.Bytes,
            ContentType = request.DigitalAsset.ContentType,
            Name = request.DigitalAsset.Name
        };

        _context.DigitalAssets.Add(digitalAsset);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateDigitalAssetResponse { DigitalAsset = digitalAsset.ToDto() };
    }
}
