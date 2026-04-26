using Commitments.Shared;
using DigitalAssets.Data;
using DigitalAssets.Domain.DigitalAssetAggregate;
using Microsoft.EntityFrameworkCore;

namespace DigitalAssets.Data;

public class DigitalAssetsDbContext : BaseDbContext, IDigitalAssetsDbContext
{
    public DigitalAssetsDbContext(DbContextOptions<DigitalAssetsDbContext> options)
        : base(options)
    {
    }

    public DbSet<DigitalAsset> DigitalAssets { get; private set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("DigitalAssets");
        base.OnModelCreating(modelBuilder);
    }
}
