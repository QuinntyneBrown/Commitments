using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DigitalAssets.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DigitalAssetsDbContext>
{
    public DigitalAssetsDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath("../../Commitments.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DigitalAssetsDbContext>();

        var connectionString = configuration.GetConnectionString("DigitalAssetsDb")
            ?? configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString);

        return new DigitalAssetsDbContext(builder.Options);
    }
}
