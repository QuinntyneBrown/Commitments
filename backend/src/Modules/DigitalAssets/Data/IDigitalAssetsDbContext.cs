using DigitalAssets.Domain.DigitalAssetAggregate;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssets.Data;

public interface IDigitalAssetsDbContext : IDisposable
{
    DbSet<DigitalAsset> DigitalAssets { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
