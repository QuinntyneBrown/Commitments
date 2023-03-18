// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using ProfileService.Core;
using ProfileService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IProfileServiceDbContext, ProfileServiceDbContext>();

        services.AddDbContextPool<ProfileServiceDbContext>(options =>
        {
            options
            .EnableThreadSafetyChecks(false)
            .UseSqlServer(connectionString, b => b.MigrationsAssembly("ProfileService.Infrastructure"));
        });

        return services;
    }
}

