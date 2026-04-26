// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Dashboard.Core;
using Dashboard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dashboard;

public static class ModuleExtensions
{
    public static IServiceCollection AddDashboardModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DashboardDb")
            ?? configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<DashboardDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Dashboard.Module")));

        services.AddScoped<IDashboardDbContext>(sp => sp.GetRequiredService<DashboardDbContext>());

        return services;
    }
}
