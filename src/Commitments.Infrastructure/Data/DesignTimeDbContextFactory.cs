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
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<CommitmentsDbContext>();

        var connectionString = configuration["Data:DefaultConnection:ConnectionString"];

        builder.UseSqlServer(connectionString);

        return new CommitmentsDbContext(builder.Options);
    }
}

