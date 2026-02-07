using DigitalAssets.Core.Model.DigitalAssetAggregate;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssets.Core;

public interface IDigitalAssetsDbContext : IDisposable
{
    DbSet<DigitalAsset> DigitalAssets { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
