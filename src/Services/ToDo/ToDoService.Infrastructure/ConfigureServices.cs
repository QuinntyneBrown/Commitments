// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using ToDoService.Core;
using ToDoService.Infrastructure.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IToDoServiceDbContext, ToDoServiceDbContext>();
        services.AddDbContext<ToDoServiceDbContext>(options =>
        {
            options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("ToDoService.Infrastructure"));
        });
    }

}