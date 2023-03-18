// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProfileService.Infrastructure.Data;
using System.IO;


namespace ProfileService.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProfileServiceDbContext>
{
    public ProfileServiceDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath("../ProfileService.Api");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<ProfileServiceDbContext>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString);

        return new ProfileServiceDbContext(builder.Options);
    }
}

