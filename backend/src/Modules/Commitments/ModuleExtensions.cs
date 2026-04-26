// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Commitments.Core;
using Commitments.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Commitments;

public static class ModuleExtensions
{
    public static IServiceCollection AddCommitmentsModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CommitmentsDb")
            ?? configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<CommitmentsDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Commitments.Module")));

        services.AddScoped<ICommitmentsDbContext>(sp => sp.GetRequiredService<CommitmentsDbContext>());

        return services;
    }
}
