using Microsoft.EntityFrameworkCore;

namespace DigitalAssets.Features.DigitalAsset;

public static class DigitalAssetExtensions
{
    public static DigitalAssetDto ToDto(this DigitalAssets.Domain.DigitalAssetAggregate.DigitalAsset digitalAsset)
    {
        return new DigitalAssetDto
        {
            DigitalAssetId = digitalAsset.DigitalAssetId,
            Bytes = digitalAsset.Bytes,
            ContentType = digitalAsset.ContentType,
            Name = digitalAsset.Name
        };
    }

    public static async Task<List<DigitalAssetDto>> ToDtosAsync(this IQueryable<DigitalAssets.Domain.DigitalAssetAggregate.DigitalAsset> digitalAssets, CancellationToken cancellationToken)
    {
        return await digitalAssets.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}
