using DigitalAssets.Core;
using DigitalAssets.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssets.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddDigitalAssetsInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDigitalAssetsDbContext, DigitalAssetsDbContext>();

        services.AddDbContextPool<DigitalAssetsDbContext>(options =>
        {
            options
            .EnableThreadSafetyChecks(false)
            .UseSqlServer(connectionString, b => b.MigrationsAssembly("DigitalAssets.Infrastructure"));
        });

        return services;
    }
}
