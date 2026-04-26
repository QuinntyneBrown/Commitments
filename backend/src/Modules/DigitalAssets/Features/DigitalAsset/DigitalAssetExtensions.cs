using Microsoft.EntityFrameworkCore;

namespace DigitalAssets.Api.Features.DigitalAsset;

public static class DigitalAssetExtensions
{
    public static DigitalAssetDto ToDto(this DigitalAssets.Core.Model.DigitalAssetAggregate.DigitalAsset digitalAsset)
    {
        return new DigitalAssetDto
        {
            DigitalAssetId = digitalAsset.DigitalAssetId,
            Bytes = digitalAsset.Bytes,
            ContentType = digitalAsset.ContentType,
            Name = digitalAsset.Name
        };
    }

    public static async Task<List<DigitalAssetDto>> ToDtosAsync(this IQueryable<DigitalAssets.Core.Model.DigitalAssetAggregate.DigitalAsset> digitalAssets, CancellationToken cancellationToken)
    {
        return await digitalAssets.Select(x => x.ToDto()).ToListAsync(cancellationToken);
    }
}
