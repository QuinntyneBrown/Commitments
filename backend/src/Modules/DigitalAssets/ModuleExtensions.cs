// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DigitalAssets.Core;
using DigitalAssets.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalAssets;

public static class ModuleExtensions
{
    public static IServiceCollection AddDigitalAssetsModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DigitalAssetsDb")
            ?? configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DigitalAssetsDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DigitalAssets.Module")));

        services.AddScoped<IDigitalAssetsDbContext>(sp => sp.GetRequiredService<DigitalAssetsDbContext>());

        return services;
    }
}
