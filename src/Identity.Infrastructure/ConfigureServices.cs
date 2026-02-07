using Identity.Core;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddIdentityInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IIdentityDbContext, IdentityDbContext>();

        services.AddDbContextPool<IdentityDbContext>(options =>
        {
            options
            .EnableThreadSafetyChecks(false)
            .UseSqlServer(connectionString, b => b.MigrationsAssembly("Identity.Infrastructure"));
        });

        return services;
    }
}
