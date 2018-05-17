using Commitments.Core.Interfaces;
using Commitments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Commitments.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {                
        public static IServiceCollection AddDataStore(this IServiceCollection services,
                                               string connectionString)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();

            services.AddDbContext<AppDbContext>(options =>
            {                
                options
                .UseLoggerFactory(AppDbContext.ConsoleLoggerFactory)
                .UseSqlServer(connectionString, b=> b.MigrationsAssembly("Commitments.Infrastructure"));
            });

            return services;
        }
    }
}
