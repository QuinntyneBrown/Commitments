// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using NoteService.Core;
using NoteService.Infrastructure.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<INoteServiceDbContext, NoteServiceDbContext>();
        services.AddDbContext<NoteServiceDbContext>(options =>
        {
            options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly("NoteService.Infrastructure"));
        });
    }

}