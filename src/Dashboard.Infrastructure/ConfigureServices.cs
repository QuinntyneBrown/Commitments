using Dashboard.Core;
using Dashboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dashboard.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddDashboardInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDashboardDbContext, DashboardDbContext>();

        services.AddDbContextPool<DashboardDbContext>(options =>
        {
            options
            .EnableThreadSafetyChecks(false)
            .UseSqlServer(connectionString, b => b.MigrationsAssembly("Dashboard.Infrastructure"));
        });

        return services;
    }
}
