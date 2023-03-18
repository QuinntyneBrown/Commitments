// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace Commitments.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CommitmentsDbContext>
{
    public CommitmentsDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath("../Commitments.Api");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<CommitmentsDbContext>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString);

        return new CommitmentsDbContext(builder.Options);
    }
}

