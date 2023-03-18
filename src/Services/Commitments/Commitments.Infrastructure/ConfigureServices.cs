// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core.Interfaces;
using Commitments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Commitments.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<ICommimentsDbContext, CommitmentsDbContext>();

        services.AddDbContextPool<CommitmentsDbContext>(options =>
        {
            options
            .EnableThreadSafetyChecks(false)
            .UseSqlServer(connectionString, b => b.MigrationsAssembly("Commitments.Infrastructure"));
        });

        return services;
    }
}

