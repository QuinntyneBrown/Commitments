using Commitments.Core.Interfaces;
using Commitments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{                
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<ICommimentsDbContext, CommitmentsDbContext>();

        services.AddDbContextPool<CommitmentsDbContext>(options =>
        {                
            options
            .EnableThreadSafetyChecks(false)
            .UseSqlServer(connectionString, b=> b.MigrationsAssembly("Commitments.Infrastructure"));
        });

        return services;
    }
}